using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CategorySymbol : ContainerSymbol, ICategorySymbol, INodeDeclaredSymbol<CategoryNode>
{
    private ICategoryEntrySymbol? lazyCategoryEntry;

    public CategorySymbol(
        ISymbol? containingSymbol,
        CategoryNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public new CategoryNode Declaration { get; }

    public override ICategoryEntrySymbol SourceEntry => GetBoundField(ref lazyCategoryEntry);

    public override ContainerKind ContainerKind => ContainerKind.Category;

    public bool IsPrimaryCategory => Declaration.Primary;

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);
        lazyCategoryEntry = binder.BindCategoryEntrySymbol(Declaration, diagnostics);
    }
}
