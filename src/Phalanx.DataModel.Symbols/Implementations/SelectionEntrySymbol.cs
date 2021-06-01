using System.Collections.Immutable;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class SelectionEntrySymbol : Symbol, ISelectionEntrySymbol
    {
        public SelectionEntryKind EntryKind => throw new System.NotImplementedException();

        public ISelectionEntrySymbol? ReferencedEntry => throw new System.NotImplementedException();

        public ICategoryEntrySymbol? PrimaryCategory => throw new System.NotImplementedException();

        public ImmutableArray<ICategoryEntrySymbol> Categories => throw new System.NotImplementedException();

        public ImmutableArray<ISelectionEntryContainerSymbol> ChildSelectionEntries => throw new System.NotImplementedException();

        public ImmutableArray<IConstraintSymbol> Constraints => throw new System.NotImplementedException();

        public ImmutableArray<IResourceEntrySymbol> Resources => throw new System.NotImplementedException();

        public string? Id => throw new System.NotImplementedException();

        public bool IsHidden => throw new System.NotImplementedException();

        public bool IsReference => throw new System.NotImplementedException();

        public IPublicationSymbol? Publication => throw new System.NotImplementedException();

        public ImmutableArray<IEffectSymbol> Effects => throw new System.NotImplementedException();

        public ICatalogueSymbol ContainingCatalogue => throw new System.NotImplementedException();

        public override SymbolKind Kind => throw new System.NotImplementedException();

        public override string Name => throw new System.NotImplementedException();

        public override string? Comment => throw new System.NotImplementedException();

        public override ISymbol ContainingSymbol => throw new System.NotImplementedException();

        IEntrySymbol? IEntrySymbol.ReferencedEntry => throw new System.NotImplementedException();
    }
}
