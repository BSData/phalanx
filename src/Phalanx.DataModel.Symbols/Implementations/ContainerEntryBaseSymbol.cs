using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public abstract class ContainerEntryBaseSymbol : EntrySymbol, IContainerEntrySymbol
    {
        protected ContainerEntryBaseSymbol(
            ICatalogueItemSymbol containingSymbol,
            ContainerEntryBaseNode declaration,
            Binder binder,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, binder, diagnostics)
        {
            Constraints = ImmutableArray<IConstraintSymbol>.Empty; // TODO map
            Resources = CreateResourceEntries(this, declaration, binder, diagnostics);
        }

        public ImmutableArray<IConstraintSymbol> Constraints { get; }

        public ImmutableArray<IResourceEntrySymbol> Resources { get; }
    }
}
