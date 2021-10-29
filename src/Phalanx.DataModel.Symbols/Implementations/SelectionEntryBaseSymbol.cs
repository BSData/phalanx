using System.Collections.Immutable;
using System.Linq;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public abstract class SelectionEntryBaseSymbol : ContainerEntryBaseSymbol, ISelectionEntryContainerSymbol
    {
        protected SelectionEntryBaseSymbol(
            ICatalogueItemSymbol containingSymbol,
            SelectionEntryBaseNode declaration,
            Binder binder,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, binder, diagnostics)
        {
            Categories = CreateCategoryEntries(this, declaration, binder, diagnostics);
            PrimaryCategory = Categories.FirstOrDefault(x => x.IsPrimaryCategory);
            ChildSelectionEntries = CreateSelectionEntryContainers(this, declaration, binder, diagnostics);
        }

        public virtual ISelectionEntryContainerSymbol? ReferencedEntry => null;

        public ICategoryEntrySymbol? PrimaryCategory { get; }

        public ImmutableArray<ICategoryEntrySymbol> Categories { get; }

        public ImmutableArray<ISelectionEntryContainerSymbol> ChildSelectionEntries { get; }

        protected sealed override IEntrySymbol? BaseReferencedEntry => ReferencedEntry;
    }
}
