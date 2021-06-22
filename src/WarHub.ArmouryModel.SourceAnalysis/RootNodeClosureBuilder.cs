using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    internal class RootNodeClosureBuilder
    {
        private readonly CatalogueBaseNode root;
        private readonly Func<string, IReferenceTargetInfo<CatalogueBaseNode>> referenceResolver;
        private RootNodeClosure? closure;
        private readonly HashSet<string> imported = new HashSet<string>();
        private readonly Queue<string> required = new Queue<string>();
        private readonly List<CatalogueBaseNode> nodes = new List<CatalogueBaseNode>();
        private readonly List<Diagnostic> errors = new List<Diagnostic>();

        public RootNodeClosureBuilder(
            CatalogueBaseNode root,
            Func<string, IReferenceTargetInfo<CatalogueBaseNode>> referenceResolver)
        {
            this.root = root;
            this.referenceResolver = referenceResolver;
        }

        public static RootNodeClosure Build(
            CatalogueBaseNode root,
            Func<string, IReferenceTargetInfo<CatalogueBaseNode>> referenceResolver)
        {
            return new RootNodeClosureBuilder(root, referenceResolver).Build();
        }

        public RootNodeClosure Build()
        {
            return closure ??= root switch
            {
                CatalogueNode catalogue => BuildCatalogueClosure(catalogue),
                _ => new RootNodeClosure(root)
            };
        }

        private void SkipRequiredAlreadyImported()
        {
            while (required.Count > 0)
            {
                var nextRequired = required.Peek();
                if (!imported.Contains(nextRequired))
                {
                    return;
                }
                required.Dequeue();
            }
        }

        private void AddRequired(string? targetId)
        {
            if (targetId is { } && !imported.Contains(targetId))
            {
                required.Enqueue(targetId);
            }
        }

        private bool MoreToImport()
        {
            SkipRequiredAlreadyImported();
            return required.Count > 0;
        }

        private void ImportNext()
        {
            var next = required.Dequeue();
            imported.Add(next);
            var result = referenceResolver(next);
            if (result.IsResolved)
            {
                if (result.TargetNode is { } node)
                {
                    nodes.Add(node);
                }
                if (result.TargetNode is CatalogueNode catalogue)
                {
                    foreach (var link in catalogue.CatalogueLinks)
                    {
                        AddRequired(link.TargetId);
                    }
                }
            }
            else if (result.ErrorInfo is { } error)
            {
                errors.Add(error);
            }
        }

        private void ResolveRoot(CatalogueNode catalogue)
        {
            AddRequired(catalogue.Id);
            AddRequired(catalogue.GamesystemId);
            while (MoreToImport())
            {
                ImportNext();
            }
            nodes.Remove(catalogue);
        }

        private RootNodeClosure BuildCatalogueClosure(CatalogueNode catalogue)
        {
            ResolveRoot(catalogue);
            var references = nodes.ToImmutableArray();
            var diagnostics = errors.ToImmutableArray();
            return new RootNodeClosure(catalogue, references, diagnostics);
        }
    }
}
