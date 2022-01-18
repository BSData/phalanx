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

    public override SymbolKind Kind => SymbolKind.Link;

    public bool IsPrimaryCategory => Declaration.Primary;

    public ICategoryEntrySymbol? ReferencedEntry
    {
        get
        {
            ForceComplete();
            return lazyReference ?? throw new InvalidOperationException("Binding failed.");
        }
    }

    internal new CategoryLinkNode Declaration { get; }

    protected override IEntrySymbol? BaseReferencedEntry => ReferencedEntry;

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyReference = binder.BindCategoryEntrySymbol(Declaration.TargetId);
    }
}
