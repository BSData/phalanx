using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class GamesystemSymbol : CatalogueBaseSymbol, INodeDeclaredSymbol<GamesystemNode>
{
    public GamesystemSymbol(
        SourceGlobalNamespaceSymbol containingSymbol,
        GamesystemNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override bool IsLibrary => false;

    public override bool IsGamesystem => true;

    public override ICatalogueSymbol Gamesystem => this;

    public override ImmutableArray<CatalogueReferenceSymbol> CatalogueReferences =>
        ImmutableArray<CatalogueReferenceSymbol>.Empty;

    public override GamesystemNode Declaration { get; }
}
