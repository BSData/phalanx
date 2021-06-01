using System.Collections.Immutable;
using System.Linq;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    internal class RootNodeClosure : IRootNodeClosure
    {
        private ImmutableArray<CatalogueBaseNode>? referencesAndRoot;

        public RootNodeClosure(CatalogueBaseNode root)
        {
            Root = root;
            References = ImmutableArray<CatalogueBaseNode>.Empty;
            Diagnostics = ImmutableArray<Diagnostic>.Empty;
        }

        public RootNodeClosure(
            CatalogueBaseNode root,
            ImmutableArray<CatalogueBaseNode> references,
            ImmutableArray<Diagnostic> diagnostics)
        {
            Root = root;
            References = references;
            Diagnostics = diagnostics;
        }

        public CatalogueBaseNode Root { get; }
        public ImmutableArray<CatalogueBaseNode> ReferencesAndRoot
            => referencesAndRoot ??= References.Prepend(Root).ToImmutableArray();
        public ImmutableArray<CatalogueBaseNode> References { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
    }
}
