using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class ResourceGroupSymbol : EntrySymbol, IResourceGroupSymbol
{
    public ResourceGroupSymbol(
        ICatalogueItemSymbol containingSymbol,
        InfoGroupNode declaration,
        Binder binder,
        BindingDiagnosticContext diagnostics)
        : base(containingSymbol, declaration, binder, diagnostics)
    {
        Resources = CreateResourceEntries().ToImmutableArray();

        IEnumerable<IResourceEntrySymbol> CreateResourceEntries()
        {
            foreach (var item in declaration.InfoGroups)
            {
                yield return CreateEntry(containingSymbol, item, binder, diagnostics);
            }
            foreach (var item in declaration.InfoLinks)
            {
                yield return CreateEntry(containingSymbol, item, binder, diagnostics);
            }
            foreach (var item in declaration.Profiles)
            {
                yield return CreateEntry(containingSymbol, item, binder, diagnostics);
            }
            foreach (var item in declaration.Rules)
            {
                yield return CreateEntry(containingSymbol, item, binder, diagnostics);
            }
        }
    }

    public override SymbolKind Kind => SymbolKind.Resource;

    public ImmutableArray<IResourceEntrySymbol> Resources { get; }

    public IResourceDefinitionSymbol? Type => null;

    public IResourceEntrySymbol? ReferencedEntry => null;
}
