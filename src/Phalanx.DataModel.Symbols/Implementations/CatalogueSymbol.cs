using System.Collections.Immutable;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.SourceAnalysis;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class CatalogueSymbol : Symbol, ICatalogueSymbol
    {
        public CatalogueSymbol(GamesystemContext context, CatalogueNode node)
        {
        }

        public override SymbolKind Kind => SymbolKind.Catalogue;

        public override string Name { get; }

        public override string? Comment { get; }

        public override ISymbol ContainingSymbol { get; }

        public bool IsLibrary { get; }

        public bool IsGamesystem { get; }

        public ICatalogueSymbol Gamesystem { get; }
        public ImmutableArray<ICatalogueReferenceSymbol> Imports => throw new System.NotImplementedException();

        public ImmutableArray<ICatalogueItemSymbol> AllItems => throw new System.NotImplementedException();

        public ImmutableArray<IResourceDefinitionSymbol> ResourceDefinitions => throw new System.NotImplementedException();

        public ImmutableArray<ISelectionEntrySymbol> RootEntries => throw new System.NotImplementedException();

        public ImmutableArray<IEntrySymbol> SharedEntries => throw new System.NotImplementedException();
    }
}
