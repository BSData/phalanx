using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CategorySymbol : RosterEntryBasedSymbol, ICategorySymbol, INodeDeclaredSymbol<CategoryNode>
{
    private ICategoryEntrySymbol? lazyCategoryEntry;

    public CategorySymbol(
        ISymbol? containingSymbol,
        CategoryNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        Resources = CreateRosterEntryResources(diagnostics).ToImmutableArray();
    }

    public override CategoryNode Declaration { get; }

    public override ICategoryEntrySymbol SourceEntry => GetBoundField(ref lazyCategoryEntry);

    public override ImmutableArray<ResourceEntryBaseSymbol> Resources { get; }

    public override SymbolKind Kind => SymbolKind.Category;

    public bool IsPrimaryCategory => Declaration.Primary;

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);
        lazyCategoryEntry = binder.BindCategoryEntrySymbol(Declaration, diagnostics);
    }
}
