using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class CategoryLinkSymbol : ContainerEntryBaseSymbol, ICategoryEntrySymbol
    {
        internal new CategoryLinkNode Declaration { get; }

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

        public ICategoryEntrySymbol? ReferencedEntry { get; }

        public bool IsPrimaryCategory => Declaration.Primary;
    }
}
