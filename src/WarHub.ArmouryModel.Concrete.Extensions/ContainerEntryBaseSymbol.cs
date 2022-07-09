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
        Costs = CreateCosts().ToImmutableArray();
        Resources = CreateResourceEntries().ToImmutableArray();

        IEnumerable<CostSymbol> CreateCosts()
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
        }

        IEnumerable<ResourceEntryBaseSymbol> CreateResourceEntries()
        {
            foreach (var item in Costs)
            {
                yield return item;
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

    public ImmutableArray<CostSymbol> Costs { get; }

    ImmutableArray<IResourceEntrySymbol> IContainerEntrySymbol.Resources =>
        Resources.Cast<ResourceEntryBaseSymbol, IResourceEntrySymbol>();

    ImmutableArray<ICostSymbol> IContainerEntrySymbol.Costs =>
        Costs.Cast<CostSymbol, ICostSymbol>();

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        // TODO .AddRange(Constraints)
        .AddRange(Resources);
}
