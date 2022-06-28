using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class ContainerEntryBaseSymbol : EntrySymbol, IContainerEntrySymbol
{
    protected ContainerEntryBaseSymbol(
        ISymbol containingSymbol,
        ContainerEntryBaseNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Constraints = ImmutableArray<IConstraintSymbol>.Empty; // TODO map
        Resources = CreateResourceEntries().ToImmutableArray();

        IEnumerable<ResourceEntryBaseSymbol> CreateResourceEntries()
        {
            var costs = declaration switch
            {
                SelectionEntryNode entry => entry.Costs.NodeList,
                EntryLinkNode link => link.Costs.NodeList,
                _ => default,
            };
            foreach (var item in costs)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
            foreach (var item in declaration.InfoGroups)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
            foreach (var item in declaration.InfoLinks)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
            foreach (var item in declaration.Profiles)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
            foreach (var item in declaration.Rules)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
        }
    }

    public sealed override SymbolKind Kind => SymbolKind.ContainerEntry;

    public abstract ContainerEntryKind ContainerKind { get; }

    public ImmutableArray<IConstraintSymbol> Constraints { get; }

    public ImmutableArray<ResourceEntryBaseSymbol> Resources { get; }

    ImmutableArray<IResourceEntrySymbol> IContainerEntrySymbol.Resources =>
        Resources.Cast<ResourceEntryBaseSymbol, IResourceEntrySymbol>();

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        // TODO .AddRange(Constraints)
        .AddRange(Resources);
}
