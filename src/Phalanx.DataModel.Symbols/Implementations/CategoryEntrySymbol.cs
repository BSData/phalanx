using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class CategoryEntrySymbol : ContainerEntryBaseSymbol, ICategoryEntrySymbol
    {
        public CategoryEntrySymbol(
            ICatalogueItemSymbol containingSymbol,
            CategoryEntryNode declaration,
            Binder binder,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, binder, diagnostics)
        {
            Declaration = declaration;
        }

        public bool IsPrimaryCategory => false;

        public ICategoryEntrySymbol? ReferencedEntry => null;

        internal new CategoryEntryNode Declaration { get; }
    }
}
