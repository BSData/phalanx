using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class CategoryLinkSymbol : ContainerEntryBaseSymbol, ICategoryEntrySymbol
    {
        public CategoryLinkSymbol(
            ICatalogueItemSymbol containingSymbol,
            CategoryLinkNode declaration,
            Binder binder,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, binder, diagnostics)
        {
            Declaration = declaration;
            ReferencedEntry = null; // TODO bind
        }

        public override SymbolKind Kind => SymbolKind.Link;

        public bool IsPrimaryCategory => Declaration.Primary;

        public ICategoryEntrySymbol? ReferencedEntry { get; }

        internal new CategoryLinkNode Declaration { get; }
    }
}
