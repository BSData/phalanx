using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
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

        ImmutableArray<ICatalogueItemSymbol> AllItems { get; }

        ImmutableArray<IResourceDefinitionSymbol> ResourceDefinitions { get; }

        ImmutableArray<IEntrySymbol> RootEntries { get; }

        ImmutableArray<IEntrySymbol> SharedEntries { get; }
    }
}
