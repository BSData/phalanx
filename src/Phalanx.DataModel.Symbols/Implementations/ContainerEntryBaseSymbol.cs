using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

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
        Resources = CreateResourceEntries().ToImmutableArray();

        IEnumerable<IResourceEntrySymbol> CreateResourceEntries()
        {
            var costs = declaration switch
            {
                SelectionEntryNode entry => entry.Costs.NodeList,
                EntryLinkNode link => link.Costs.NodeList,
                _ => default,
            };
            foreach (var item in costs)
            {
                yield return CreateEntry(this, item, binder, diagnostics);
            }
            foreach (var item in declaration.InfoGroups)
            {
                yield return CreateEntry(this, item, binder, diagnostics);
            }
            foreach (var item in declaration.InfoLinks)
            {
                yield return CreateEntry(this, item, binder, diagnostics);
            }
            foreach (var item in declaration.Profiles)
            {
                yield return CreateEntry(this, item, binder, diagnostics);
            }
            foreach (var item in declaration.Rules)
            {
                yield return CreateEntry(this, item, binder, diagnostics);
            }
        }
    }

    public override SymbolKind Kind => SymbolKind.Entry;

    public ImmutableArray<IConstraintSymbol> Constraints { get; }

    public ImmutableArray<IResourceEntrySymbol> Resources { get; }
}
