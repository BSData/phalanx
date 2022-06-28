using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CategoryLinkSymbol : ContainerEntryBaseSymbol, ICategoryEntrySymbol, INodeDeclaredSymbol<CategoryLinkNode>
{
    private ICategoryEntrySymbol? lazyReference;

    public CategoryLinkSymbol(
        ISymbol containingSymbol,
        CategoryLinkNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override ContainerEntryKind ContainerKind => ContainerEntryKind.Category;

    public bool IsPrimaryCategory => Declaration.Primary;

    public override ICategoryEntrySymbol ReferencedEntry => GetBoundField(ref lazyReference);

    public override CategoryLinkNode Declaration { get; }

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);

        lazyReference = binder.BindCategoryEntrySymbol(Declaration, diagnostics);
    }
}
