namespace WarHub.ArmouryModel;

/// <summary>
/// Catalogue of data items.
/// BS catalogue/gamesystem.
/// WHAM <see cref="Source.CatalogueBaseNode" />.
/// </summary>
public interface ICatalogueSymbol : ISymbol
{
    bool IsLibrary { get; }

    bool IsGamesystem { get; }

    ICatalogueSymbol Gamesystem { get; }

    /// <summary>
    /// Represents catalogues linked using CatalogueLink syntax.
    /// </summary>
    ImmutableArray<ICatalogueReferenceSymbol> CatalogueReferences { get; }

    ImmutableArray<ISymbol> AllItems { get; }

    ImmutableArray<IResourceDefinitionSymbol> ResourceDefinitions { get; }

    ImmutableArray<IContainerEntrySymbol> RootContainerEntries { get; }

    ImmutableArray<IResourceEntrySymbol> RootResourceEntries { get; }

    ImmutableArray<ISelectionEntryContainerSymbol> SharedSelectionEntryContainers { get; }

    ImmutableArray<IResourceEntrySymbol> SharedResourceEntries { get; }
}
