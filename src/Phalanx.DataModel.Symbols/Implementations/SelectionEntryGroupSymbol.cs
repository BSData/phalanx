using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class SelectionEntryGroupSymbol : SelectionEntryBaseSymbol, ISelectionEntryGroupSymbol
    {
        private readonly SelectionEntryGroupNode declaration;

        public SelectionEntryGroupSymbol(
            ICatalogueItemSymbol containingSymbol,
            SelectionEntryGroupNode declaration,
            Binder binder,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, binder, diagnostics)
        {
            this.declaration = declaration;
            DefaultSelectionEntry = null; // TODO bind
        }

        public override SymbolKind Kind => SymbolKind.Entry;

        public ISelectionEntrySymbol? DefaultSelectionEntry { get; }
    }
}
