namespace WarHub.ArmouryModel;

/// <summary>
/// Catalogue of data items.
/// BS catalogue/gamesystem.
/// WHAM <see cref="Source.CatalogueBaseNode" />.
/// </summary>
public interface ICatalogueSymbol : IModuleSymbol
{
    bool IsLibrary { get; }

    bool IsGamesystem { get; }

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
