using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal class CategoryLinkSymbol : ContainerEntryBaseSymbol, ICategoryEntrySymbol
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

    public ICategoryEntrySymbol ReferencedEntry => GetBoundField(ref lazyReference);

    internal new CategoryLinkNode Declaration { get; }

    protected override IEntrySymbol? BaseReferencedEntry => ReferencedEntry;

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyReference = binder.BindCategoryEntrySymbol(Declaration, diagnosticBag);
    }
}
