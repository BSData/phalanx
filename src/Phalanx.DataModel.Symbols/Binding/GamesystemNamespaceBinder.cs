using Phalanx.DataModel.Symbols.Implementation;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Binding;

public class GamesystemNamespaceBinder : Binder
{
    internal GamesystemNamespaceBinder(Binder next, SourceGlobalNamespaceSymbol namespaceSymbol)
        : base(next)
    {
        NamespaceSymbol = namespaceSymbol;
    }

    public SourceGlobalNamespaceSymbol NamespaceSymbol { get; }

    internal override ICatalogueSymbol? BindCatalogueSymbol(string? targetId, CatalogueLinkKind type)
    {
        foreach (var catalogue in NamespaceSymbol.Catalogues)
        {
            if (catalogue.Id == targetId)
                return catalogue;
        }
        return NextRequired.BindCatalogueSymbol(targetId, type);
    }

    internal override ICatalogueSymbol? BindGamesystemSymbol(string? gamesystemId)
    {
        foreach (var catalogue in NamespaceSymbol.Catalogues)
        {
            if (catalogue.IsGamesystem && catalogue.Id == gamesystemId)
                return catalogue;
        }
        return NextRequired.BindGamesystemSymbol(gamesystemId);
    }
}
