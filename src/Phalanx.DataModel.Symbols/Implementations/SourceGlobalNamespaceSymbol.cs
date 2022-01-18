using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal class SourceGlobalNamespaceSymbol : Symbol, IGamesystemNamespaceSymbol
{
    public SourceGlobalNamespaceSymbol(
        ImmutableArray<CatalogueBaseNode> catalogues,
        Compilation declaringCompilation)
    {
        DeclaringCompilation = declaringCompilation;
        DeclarationDiagnostics = DiagnosticBag.GetInstance();
        Catalogues = catalogues.Select(CreateSymbol).Where(x => x is not null).ToImmutableArray()!;
        var rootCandidates = Catalogues.Where(x => x.IsGamesystem).ToImmutableArray();
        RootCatalogue = rootCandidates.FirstOrDefault()
            ?? DeclaringCompilation.CreateMissingGamesystemSymbol(DeclarationDiagnostics);
        if (rootCandidates.Length > 1)
        {
            foreach (var candidate in rootCandidates)
                DeclarationDiagnostics.Add(ErrorCode.ERR_MultipleGamesystems, candidate.Declaration);
        }

        CatalogueBaseSymbol? CreateSymbol(CatalogueBaseNode node)
        {
            if (node is CatalogueNode catalogueNode)
            {
                return new CatalogueSymbol(this, catalogueNode, DeclarationDiagnostics);
            }
            else if (node is GamesystemNode gamesystemNode)
            {
                return new GamesystemSymbol(this, gamesystemNode, DeclarationDiagnostics);
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

    public ImmutableArray<CatalogueBaseSymbol> Catalogues { get; }

    ImmutableArray<ICatalogueSymbol> IGamesystemNamespaceSymbol.Catalogues =>
        Catalogues.CastArray<ICatalogueSymbol>();

    public override IGamesystemNamespaceSymbol? ContainingNamespace => null;

    public override ICatalogueSymbol? ContainingCatalogue => null;

    internal override Compilation DeclaringCompilation { get; }

    internal DiagnosticBag DeclarationDiagnostics { get; }
}
