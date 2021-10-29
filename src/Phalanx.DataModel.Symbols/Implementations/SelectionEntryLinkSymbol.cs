using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class SelectionEntryLinkSymbol : SelectionEntryBaseSymbol
    {
        private readonly EntryLinkNode declaration;

        public SelectionEntryLinkSymbol(
            ICatalogueItemSymbol containingSymbol,
            EntryLinkNode declaration,
            Binder binder,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, binder, diagnostics)
        {
            this.declaration = declaration;
            ReferencedEntry = null; // TODO bind
        }

        public override SymbolKind Kind => SymbolKind.Link;

        public override ISelectionEntryContainerSymbol? ReferencedEntry { get; }
    }
}
