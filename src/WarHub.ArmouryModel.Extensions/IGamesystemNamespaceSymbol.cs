namespace WarHub.ArmouryModel;

/// <summary>
/// Gamesystem Namespace is a container for all entities
/// that belong to a single GameSystem ID (gamesystem, catalogues, roster(s)).
/// </summary>
public interface IGamesystemNamespaceSymbol : ISymbol
{
    ICatalogueSymbol RootCatalogue { get; }

    ImmutableArray<ICatalogueSymbol> Catalogues { get; }
}
