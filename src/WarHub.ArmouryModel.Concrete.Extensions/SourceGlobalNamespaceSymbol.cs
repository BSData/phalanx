using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class SourceGlobalNamespaceSymbol : Symbol, IGamesystemNamespaceSymbol
{
    public SourceGlobalNamespaceSymbol(
        ImmutableArray<SourceNode> rootDataNodes,
        WhamCompilation declaringCompilation)
    {
        DeclaringCompilation = declaringCompilation;
        DeclarationDiagnostics = DiagnosticBag.GetInstance();
        AllRootSymbols = rootDataNodes.Select(CreateSymbol).Where(x => x is not null).ToImmutableArray()!;
        Rosters = AllRootSymbols.OfType<RosterSymbol>().ToImmutableArray();
        Catalogues = AllRootSymbols.OfType<CatalogueBaseSymbol>().ToImmutableArray();
        RootCatalogue = GetOrCreateGamesystemSymbol();
        // TODO more diagnostics, e.g. all catalogues are from the same game system?

        ICatalogueSymbol GetOrCreateGamesystemSymbol()
        {
            var rootCandidates = Catalogues.Where(x => x.IsGamesystem).ToImmutableArray();
            if (rootCandidates.Length > 1)
            {
                foreach (var candidate in rootCandidates)
                    DeclarationDiagnostics.Add(ErrorCode.ERR_MultipleGamesystems, candidate.Declaration);
            }
            return rootCandidates.FirstOrDefault()
                ?? DeclaringCompilation.CreateMissingGamesystemSymbol(DeclarationDiagnostics);
        }

        Symbol? CreateSymbol(SourceNode node)
        {
            if (node is CatalogueNode catalogueNode)
            {
                return new CatalogueSymbol(this, catalogueNode, DeclarationDiagnostics);
            }
            else if (node is GamesystemNode gamesystemNode)
            {
                return new GamesystemSymbol(this, gamesystemNode, DeclarationDiagnostics);
            }
            else if (node is RosterNode rosterNode)
            {
                return new RosterSymbol(this, rosterNode, DeclarationDiagnostics);
            }
            else
            {
                DeclarationDiagnostics.Add(ErrorCode.ERR_UnknownCatalogueType, node);
                return null;
            }
        }
    }

    public override SymbolKind Kind => SymbolKind.Namespace;

    public override string? Id => RootCatalogue.Id;

    public override string Name => RootCatalogue.Name;

    public override string? Comment => null;

    public override ISymbol? ContainingSymbol => null;

    public ICatalogueSymbol RootCatalogue { get; }

    public ImmutableArray<Symbol> AllRootSymbols { get; }

    public ImmutableArray<CatalogueBaseSymbol> Catalogues { get; }

    public ImmutableArray<RosterSymbol> Rosters { get; }

    public override IGamesystemNamespaceSymbol? ContainingNamespace => null;

    public override ICatalogueSymbol? ContainingCatalogue => null;

    internal override WhamCompilation DeclaringCompilation { get; }

    internal DiagnosticBag DeclarationDiagnostics { get; }

    internal override bool RequiresCompletion => true;

    ImmutableArray<ICatalogueSymbol> IGamesystemNamespaceSymbol.Catalogues =>
        ImmutableArray<ICatalogueSymbol>.CastUp(Catalogues);

    ImmutableArray<IRosterSymbol> IGamesystemNamespaceSymbol.Rosters =>
        ImmutableArray<IRosterSymbol>.CastUp(Rosters);

    ImmutableArray<ISymbol> IGamesystemNamespaceSymbol.AllRootSymbols =>
        ImmutableArray<ISymbol>.CastUp(AllRootSymbols);

    protected override void InvokeForceCompleteOnChildren()
    {
        base.InvokeForceCompleteOnChildren();
        InvokeForceComplete(AllRootSymbols);
    }
}
