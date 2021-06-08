using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class SelectionEntrySymbol : EntrySymbol, ISelectionEntrySymbol
    {
        private readonly SelectionEntryNode declaration;

        public SelectionEntrySymbol(
            ICatalogueItemSymbol containingSymbol,
            SelectionEntryNode declaration,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, diagnostics)
        {
            this.declaration = declaration;
        }

        public override SymbolKind Kind => SymbolKind.Entry;

        public SelectionEntryKind EntryKind => declaration.Type;

        public ICategoryEntrySymbol? PrimaryCategory => throw new System.NotImplementedException();

        public ImmutableArray<ICategoryEntrySymbol> Categories => throw new System.NotImplementedException();

        public ImmutableArray<ISelectionEntryContainerSymbol> ChildSelectionEntries => throw new System.NotImplementedException();

        public ImmutableArray<IConstraintSymbol> Constraints => throw new System.NotImplementedException();

        public ImmutableArray<IResourceEntrySymbol> Resources => throw new System.NotImplementedException();

        ISelectionEntrySymbol? ISelectionEntrySymbol.ReferencedEntry => null;
    }
}
