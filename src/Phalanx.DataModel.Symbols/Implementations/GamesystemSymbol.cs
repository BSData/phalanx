using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class GamesystemSymbol : CatalogueBaseSymbol
{
    public GamesystemSymbol(Compilation declaringCompilation, GamesystemNode declaration)
        : base(declaringCompilation, declaration)
    {
    }

    public override bool IsLibrary => false;

    public override bool IsGamesystem => true;

    public override ICatalogueSymbol Gamesystem => this;

    public override ImmutableArray<ICatalogueReferenceSymbol> Imports =>
        ImmutableArray<ICatalogueReferenceSymbol>.Empty;
}
