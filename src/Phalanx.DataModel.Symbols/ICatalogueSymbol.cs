using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Catalogue of data items.
/// BS catalogue/gamesystem.
/// WHAM <see cref="WarHub.ArmouryModel.Source.CatalogueBaseNode" />.
/// </summary>
public interface ICatalogueSymbol : ISymbol, ICatalogueItemSymbol
{
    bool IsLibrary { get; }

    bool IsGamesystem { get; }

    ICatalogueSymbol Gamesystem { get; }

    /// <summary>
    /// Represents catalogues linked using CatalogueLink syntax.
    /// </summary>
    ImmutableArray<ICatalogueReferenceSymbol> Imports { get; }

    ImmutableArray<ICatalogueItemSymbol> AllItems { get; }

    ImmutableArray<IResourceDefinitionSymbol> ResourceDefinitions { get; }

    ImmutableArray<IContainerEntrySymbol> RootContainerEntries { get; }

    ImmutableArray<IResourceEntrySymbol> RootResourceEntries { get; }

    ImmutableArray<ISelectionEntryContainerSymbol> SharedSelectionEntryContainers { get; }

    ImmutableArray<IResourceEntrySymbol> SharedResourceEntries { get; }
}
