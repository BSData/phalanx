namespace Phalanx.DataModel.Symbols.Implementation;

internal static class ErrorSymbols
{
    internal class InvalidIdSymbolBase : ISymbol
    {
        public SymbolKind Kind => SymbolKind.Error;

        public string? Id { get; init; }

        public string Name { get; init; } = "Error";

        public string? Comment => null;

        public ISymbol? ContainingSymbol => null;

        public ICatalogueSymbol? ContainingCatalogue => null;

        public IGamesystemNamespaceSymbol? ContainingNamespace => null;
    }

    internal class InvalidIdCharacteristicTypeSymbol : InvalidIdSymbolBase, ICharacteristicTypeSymbol
    {
    }

    internal class InvalidIdCostTypeSymbol : InvalidIdSymbolBase, ICostTypeSymbol
    {
    }

    internal class InvalidIdProfileTypeSymbol : InvalidIdSymbolBase, IProfileTypeSymbol
    {
        ImmutableArray<ICharacteristicTypeSymbol> IProfileTypeSymbol.CharacteristicTypes =>
            ImmutableArray<ICharacteristicTypeSymbol>.Empty;
    }

    internal class InvalidGamesystemSymbol : InvalidCatalogueBaseSymbol
    {
        public override bool IsGamesystem => true;

        public override ICatalogueSymbol Gamesystem => this;
    }

    internal class InvalidCatalogueSymbol : InvalidCatalogueBaseSymbol
    {
        private readonly Func<ICatalogueSymbol> gamesystemFunc;

        public InvalidCatalogueSymbol(Func<ICatalogueSymbol> gamesystemFunc)
        {
            this.gamesystemFunc = gamesystemFunc;
        }

        public override bool IsGamesystem => false;

        public override ICatalogueSymbol Gamesystem => gamesystemFunc();
    }

    internal abstract class InvalidCatalogueBaseSymbol : InvalidIdSymbolBase, ICatalogueSymbol
    {
        bool ICatalogueSymbol.IsLibrary => false;

        public abstract bool IsGamesystem { get; }

        public abstract ICatalogueSymbol Gamesystem { get; }

        ImmutableArray<ICatalogueReferenceSymbol> ICatalogueSymbol.Imports =>
            ImmutableArray<ICatalogueReferenceSymbol>.Empty;

        ImmutableArray<ISymbol> ICatalogueSymbol.AllItems =>
            ImmutableArray<ISymbol>.Empty;

        ImmutableArray<IResourceDefinitionSymbol> ICatalogueSymbol.ResourceDefinitions =>
            ImmutableArray<IResourceDefinitionSymbol>.Empty;

        ImmutableArray<IContainerEntrySymbol> ICatalogueSymbol.RootContainerEntries =>
            ImmutableArray<IContainerEntrySymbol>.Empty;

        ImmutableArray<IResourceEntrySymbol> ICatalogueSymbol.RootResourceEntries =>
            ImmutableArray<IResourceEntrySymbol>.Empty;

        ImmutableArray<ISelectionEntryContainerSymbol> ICatalogueSymbol.SharedSelectionEntryContainers =>
            ImmutableArray<ISelectionEntryContainerSymbol>.Empty;

        ImmutableArray<IResourceEntrySymbol> ICatalogueSymbol.SharedResourceEntries =>
            ImmutableArray<IResourceEntrySymbol>.Empty;
    }
}
