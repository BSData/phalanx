using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal class GamesystemSymbol : CatalogueBaseSymbol
{
    public GamesystemSymbol(
        SourceGlobalNamespaceSymbol containingSymbol,
        GamesystemNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
    }

    public override bool IsLibrary => false;

    public override bool IsGamesystem => true;

    public override ICatalogueSymbol Gamesystem => this;

    public override ImmutableArray<ICatalogueReferenceSymbol> Imports =>
        ImmutableArray<ICatalogueReferenceSymbol>.Empty;
}
