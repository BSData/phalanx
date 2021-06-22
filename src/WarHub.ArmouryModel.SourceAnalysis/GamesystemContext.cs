using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    public sealed class GamesystemContext : IRootNodeClosureResolver, IReferenceInfoProvider
    {
        private ImmutableArray<CatalogueBaseNode>? roots;

        private GamesystemContext(
            GamesystemNode? gamesystem,
            ImmutableArray<CatalogueNode> catalogues,
            ImmutableDictionary<CatalogueBaseNode, RootNodeClosure> closures,
            ImmutableArray<Diagnostic> diagnostics)
        {
            Gamesystem = gamesystem;
            Catalogues = catalogues;
            Closures = closures;
            Diagnostics = diagnostics;
            ReferenceInfoProvider = new ReferenceInfoProvider(this);
        }

        public GamesystemNode? Gamesystem { get; }

        private ImmutableArray<CatalogueBaseNode> RootsCalc =>
            ImmutableArray<CatalogueBaseNode>.CastUp(Catalogues) is var bases && Gamesystem is null
            ? bases
            : bases.Prepend(Gamesystem).ToImmutableArray();

        public ImmutableArray<CatalogueBaseNode> Roots => roots ??= RootsCalc;

        public ImmutableArray<CatalogueNode> Catalogues { get; }

        public ImmutableArray<Diagnostic> Diagnostics { get; }

        private ReferenceInfoProvider ReferenceInfoProvider { get; }

        private ImmutableDictionary<CatalogueBaseNode, RootNodeClosure> Closures { get; }

        public bool Contains(CatalogueBaseNode root) => Closures.ContainsKey(root);

        public IRootNodeClosure? GetRootClosure(SourceNode node)
        {
            var root = node.FirstAncestorOrSelf<CatalogueBaseNode>();
            if (root is { } && Closures.TryGetValue(root, out var ctx))
            {
                return ctx;
            }
            return null;
        }

        public IReferenceSourceIndex GetReferences(SourceNode node)
            => ReferenceInfoProvider.GetReferences(node);

        public static async Task<ImmutableArray<GamesystemContext>> CreateAsync(IWorkspace workspace)
        {
            var roots = await GetRootsAsync();
            return roots
                .GroupBy(x => GetGamesystemId(x, "unknown"))
                .Select(group => CreateSingle(group.ToImmutableArray()))
                .ToImmutableArray();

            async Task<List<CatalogueBaseNode>> GetRootsAsync()
            {
                var datafiles = workspace.Datafiles
                    .Where(x => x.DataKind == SourceKind.Catalogue || x.DataKind == SourceKind.Gamesystem);
                var roots = new List<CatalogueBaseNode>();
                foreach (var datafile in datafiles)
                {
                    var root = (CatalogueBaseNode?)await datafile.GetDataAsync();
                    if (root is { })
                    {
                        roots.Add(root);
                    }
                }
                return roots;
            }
        }

        public static GamesystemContext CreateSingle(params CatalogueBaseNode[] rootNodes)
            => CreateSingle(rootNodes.ToImmutableArray());

        public static GamesystemContext CreateSingle(ImmutableArray<CatalogueBaseNode> rootNodes)
        {
            var gstCount = rootNodes.Select(GetGamesystemId).Distinct().Count();
            if (gstCount != 1)
            {
                throw new ArgumentException(
                    "This method accepts only rootNodes of a single gamesystem.",
                    nameof(rootNodes));
            }
            var datafileIndex = rootNodes.ToImmutableDictionary(x => x.Id ?? "");
            var closures = rootNodes.ToImmutableDictionary(x => x, CreateClosure);
            var gamesystems = rootNodes.OfType<GamesystemNode>().ToImmutableArray();
            var catalogues = rootNodes.OfType<CatalogueNode>().ToImmutableArray();
            var closureDiagnostics = closures.Values.SelectMany(x => x.Diagnostics).ToImmutableArray();
            var diagnostics = gamesystems.Length == 1
                ? closureDiagnostics
                : closureDiagnostics.Add(CreateGamesystemCountDiagnostic());
            return new GamesystemContext(gamesystems.FirstOrDefault(), catalogues, closures, diagnostics);

            RootNodeClosure CreateClosure(CatalogueBaseNode root)
            {
                return RootNodeClosureBuilder.Build(root, ResolveDatafile);
            }

            Diagnostic CreateGamesystemCountDiagnostic()
            {
                return new GamesystemCountDiagnostic(gamesystems);
            }
            IReferenceTargetInfo<CatalogueBaseNode> ResolveDatafile(string targetId)
            {
                if (datafileIndex.TryGetValue(targetId, out var target))
                {
                    return ReferenceTargetInfo.Create(target);
                }
                else
                {
                    return ReferenceTargetInfo.Error<CatalogueBaseNode>(
                        new DatafileNotFoundDiagnostic(targetId));
                }
            }
        }

        private static string? GetGamesystemId(CatalogueBaseNode node) => GetGamesystemId(node, null);

        private static string? GetGamesystemId(CatalogueBaseNode node, string? fallbackId) =>
            node.Accept(new GamesystemIdVisitor(fallbackId));

        private class DatafileNotFoundDiagnostic : ReferenceErrorInfo
        {
            public DatafileNotFoundDiagnostic(string datafileId)
            {
                DatafileId = datafileId;
            }

            public string DatafileId { get; }

            public override string GetMessage() => $"Couldn't find datafile with id='{DatafileId}'.";
        }

        // private class DuplicateIdDiagnostic : Diagnostic
        // {
        //     public DuplicateIdDiagnostic(SourceNode root, IGrouping<string, IIdentifiableNode> group)
        //     {
        //         Root = root;
        //         Group = group;
        //     }

        //     public SourceNode Root { get; }

        //     public IGrouping<string, IIdentifiableNode> Group { get; }

        //     public override string GetMessage() => $"Duplicated ID '{Group.Key}' (found {Group.Count()} times).";
        // }

        private class GamesystemCountDiagnostic : Diagnostic
        {
            public GamesystemCountDiagnostic(ImmutableArray<GamesystemNode> nodes)
            {
                Nodes = nodes;
            }

            public ImmutableArray<GamesystemNode> Nodes { get; }

            public override string GetMessage() => $"Found {Nodes.Length} gamesystems (should be only 1).";
        }

        private class GamesystemIdVisitor : SourceVisitor<string?>
        {
            public GamesystemIdVisitor(string? fallbackId)
            {
                FallbackId = fallbackId;
            }
            public string? FallbackId { get; }
            public override string? DefaultVisit(SourceNode node) => FallbackId;
            public override string? VisitCatalogue(CatalogueNode node) => node.GamesystemId;
            public override string? VisitGamesystem(GamesystemNode node) => node.Id;
        }
    }
}
