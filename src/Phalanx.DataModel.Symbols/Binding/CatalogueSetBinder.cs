using Phalanx.DataModel.Symbols.Implementation;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Binding;

public class CatalogueSetBinder : Binder
{
    internal CatalogueSetBinder(Binder next, ImmutableArray<CatalogueSymbol> catalogues)
        : base(next)
    {
        Catalogues = catalogues;
    }

    public ImmutableArray<CatalogueSymbol> Catalogues { get; }

    internal override ICatalogueSymbol? BindCatalogueSymbol(string? targetId, CatalogueLinkKind type)
    {
        foreach (var catalogue in Catalogues)
        {
            if (catalogue.Id == targetId)
                return catalogue;
        }
        return NextRequired.BindCatalogueSymbol(targetId, type);
    }

    internal override ICatalogueSymbol? BindGamesystemSymbol(string? gamesystemId)
    {
        foreach (var catalogue in Catalogues)
        {
            if (catalogue.IsGamesystem && catalogue.Id == gamesystemId)
                return catalogue;
        }
        return NextRequired.BindGamesystemSymbol(gamesystemId);
    }
}
