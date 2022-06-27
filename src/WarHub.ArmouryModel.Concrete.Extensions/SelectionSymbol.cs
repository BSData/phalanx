using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class SelectionSymbol : RosterEntryBasedSymbol, ISelectionSymbol, INodeDeclaredSymbol<SelectionNode>
{
    private ISelectionEntrySymbol? lazySelectionEntry;

    public SelectionSymbol(
        ISymbol? containingSymbol,
        SelectionNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        Costs = declaration.Costs.Select(x => new CostSymbol(this, x, diagnostics)).ToImmutableArray<ICostSymbol>();
        Resources = Costs.CastArray<IResourceEntrySymbol>().AddRange(CreateRosterEntryResources(diagnostics));
        Categories = declaration.Categories.Select(x => new CategorySymbol(this, x, diagnostics)).ToImmutableArray<ICategorySymbol>();
        ChildSelections = declaration.Selections.Select(x => new SelectionSymbol(this, x, diagnostics)).ToImmutableArray<ISelectionSymbol>();
        PrimaryCategory = Categories.FirstOrDefault(x => x.IsPrimaryCategory); // TODO diagnostic if count != 1 for root selection? (also what about NoCategory)
    }

    public override SelectionNode Declaration { get; }

    public override SymbolKind Kind => SymbolKind.Selection;

    public int Count => Declaration.Number;

    public override ISelectionEntrySymbol SourceEntry => GetBoundField(ref lazySelectionEntry);

    public ImmutableArray<ISelectionSymbol> ChildSelections { get; }

    public override ImmutableArray<IResourceEntrySymbol> Resources { get; }

    public SelectionEntryKind EntryKind => Declaration.Type;

    public ICategorySymbol? PrimaryCategory { get; }

    public ImmutableArray<ICategorySymbol> Categories { get; }

    public ImmutableArray<ICostSymbol> Costs { get; }

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);
        lazySelectionEntry = binder.BindSelectionEntry(Declaration, diagnostics);
    }

    protected override void InvokeForceCompleteOnChildren()
    {
        base.InvokeForceCompleteOnChildren();
        // Resources are done in base.Invoke... call
        // Costs are included in Resources
        InvokeForceComplete(Categories);
        InvokeForceComplete(ChildSelections);
    }
}
