using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class ResourceGroupSymbol : EntrySymbol, IResourceGroupSymbol
    {
        public ResourceGroupSymbol(
            ISymbol containingSymbol,
            InfoGroupNode declaration,
            Binder binder,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, binder, diagnostics)
        {
            Resources = CreateResourceEntries(this, declaration, binder, diagnostics);
        }

        public override SymbolKind Kind => SymbolKind.Resource;

        public ImmutableArray<IResourceEntrySymbol> Resources { get; }

        public IResourceDefinitionSymbol? Type => null;

        public IResourceEntrySymbol? ReferencedEntry => null;
    }
}
