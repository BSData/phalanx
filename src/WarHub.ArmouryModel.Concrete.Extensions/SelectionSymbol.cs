using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class SelectionSymbol : EntryInstanceSymbol, ISelectionSymbol, INodeDeclaredSymbol<SelectionNode>
{
    public SelectionSymbol(
        ISymbol? containingSymbol,
        SelectionNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        Costs = declaration.Costs.Select(x => new CostSymbol(this, x, diagnostics)).ToImmutableArray();
        Categories = declaration.Categories.Select(x => new CategorySymbol(this, x, diagnostics)).ToImmutableArray();
        ChildSelections = declaration.Selections.Select(x => new SelectionSymbol(this, x, diagnostics)).ToImmutableArray();
        PrimaryCategory = Categories.FirstOrDefault(x => x.IsPrimaryCategory); // TODO diagnostic if count != 1 for root selection?
    }

    public override SelectionNode Declaration { get; }

    public override SymbolKind Kind => SymbolKind.Selection;

    public int Count => Declaration.Number;

    public override ISelectionEntrySymbol SourceEntry =>
        (ISelectionEntrySymbol)SourceEntryPath.SourceEntries.Last();

    public ImmutableArray<SelectionSymbol> ChildSelections { get; }

    public SelectionEntryKind EntryKind => Declaration.Type;

    public ICategorySymbol? PrimaryCategory { get; }

    public ImmutableArray<CategorySymbol> Categories { get; }

    public ImmutableArray<CostSymbol> Costs { get; }

    ImmutableArray<ICategorySymbol> ISelectionSymbol.Categories =>
        Categories.Cast<CategorySymbol, ICategorySymbol>();

    ImmutableArray<ICostSymbol> ISelectionSymbol.Costs =>
        Costs.Cast<CostSymbol, ICostSymbol>();

    ImmutableArray<ISelectionSymbol> ISelectionContainerSymbol.Selections =>
        ChildSelections.Cast<SelectionSymbol, ISelectionSymbol>();

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        .AddRange(Costs.Cast<CostSymbol, Symbol>())
        .AddRange(Categories.Cast<CategorySymbol, Symbol>())
        .AddRange(ChildSelections.Cast<SelectionSymbol, Symbol>());
}
