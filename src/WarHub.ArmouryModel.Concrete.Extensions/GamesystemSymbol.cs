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

    public override ImmutableArray<ICatalogueReferenceSymbol> CatalogueReferences =>
        ImmutableArray<ICatalogueReferenceSymbol>.Empty;

    public override GamesystemNode Declaration { get; }
}
