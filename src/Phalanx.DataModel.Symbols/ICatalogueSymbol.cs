namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Catalogue of data items.
/// BS catalogue/gamesystem.
/// WHAM <see cref="WarHub.ArmouryModel.Source.CatalogueBaseNode" />.
/// </summary>
public interface ICatalogueSymbol : ISymbol
{
    bool IsLibrary { get; }

    bool IsGamesystem { get; }

    ICatalogueSymbol Gamesystem { get; }

    /// <summary>
    /// Represents catalogues linked using CatalogueLink syntax.
    /// </summary>
    ImmutableArray<ICatalogueReferenceSymbol> Imports { get; }

    ImmutableArray<ISymbol> AllItems { get; }

    ImmutableArray<IResourceDefinitionSymbol> ResourceDefinitions { get; }

    ImmutableArray<IContainerEntrySymbol> RootContainerEntries { get; }

    ImmutableArray<IResourceEntrySymbol> RootResourceEntries { get; }

    ImmutableArray<ISelectionEntryContainerSymbol> SharedSelectionEntryContainers { get; }

    ImmutableArray<IResourceEntrySymbol> SharedResourceEntries { get; }
}

/// <summary>
/// Gamesystem Namespace is a container for all entities
/// that belong to a single GameSystem ID (gamesystem, catalogues, roster(s)).
/// </summary>
public interface IGamesystemNamespaceSymbol : ISymbol
{
    ICatalogueSymbol RootCatalogue { get; }

    ImmutableArray<ICatalogueSymbol> Catalogues { get; }
}
