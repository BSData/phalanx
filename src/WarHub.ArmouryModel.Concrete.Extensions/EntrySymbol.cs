using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class EntrySymbol : SourceDeclaredSymbol, IEntrySymbol
{
    protected EntrySymbol(
        ISymbol containingSymbol,
        EntryBaseNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        PublicationReference = declaration.PublicationId is not null
            ? new EntryPublicationReferenceSymbol(this, diagnostics) : null;
        Effects = LogicSymbol.CreateEffects(this, diagnostics);
    }

    public override EntryBaseNode Declaration { get; }

    public bool IsHidden => Declaration.Hidden;

    protected virtual IEntrySymbol? BaseReferencedEntry => null;

    public EntryPublicationReferenceSymbol? PublicationReference { get; }

    IPublicationReferenceSymbol? IEntrySymbol.PublicationReference => PublicationReference;

    public ImmutableArray<IEffectSymbol> Effects { get; }

    public bool IsReference => BaseReferencedEntry is not null;

    IEntrySymbol? IEntrySymbol.ReferencedEntry => BaseReferencedEntry;

    protected override void InvokeForceCompleteOnChildren()
    {
        base.InvokeForceCompleteOnChildren();
        PublicationReference?.ForceComplete();
        InvokeForceComplete(Effects);
    }

    public static ISelectionEntryContainerSymbol CreateEntry(
        ISymbol containingSymbol,
        EntryLinkNode item,
        DiagnosticBag diagnostics)
    {
        return new SelectionEntryLinkSymbol(containingSymbol, item, diagnostics);
    }

    public static IRuleSymbol CreateEntry(
        ISymbol containingSymbol,
        RuleNode item,
        DiagnosticBag diagnostics)
    {
        return new RuleSymbol(containingSymbol, item, diagnostics);
    }

    public static IProfileSymbol CreateEntry(
        ISymbol containingSymbol,
        ProfileNode item,
        DiagnosticBag diagnostics)
    {
        return new ProfileSymbol(containingSymbol, item, diagnostics);
    }

    public static IResourceEntrySymbol CreateEntry(
        ISymbol containingSymbol,
        InfoLinkNode item,
        DiagnosticBag diagnostics)
    {
        return new ResourceLinkSymbol(containingSymbol, item, diagnostics);
    }

    public static IResourceGroupSymbol CreateEntry(
        ISymbol containingSymbol,
        InfoGroupNode item,
        DiagnosticBag diagnostics)
    {
        return new ResourceGroupSymbol(containingSymbol, item, diagnostics);
    }

    public static ICostSymbol CreateEntry(
        ISymbol containingSymbol,
        CostNode item,
        DiagnosticBag diagnostics)
    {
        return new CostSymbol(containingSymbol, item, diagnostics);
    }

    public static ICategoryEntrySymbol CreateEntry(
        ISymbol containingSymbol,
        CategoryEntryNode item,
        DiagnosticBag diagnostics)
    {
        return new CategoryEntrySymbol(containingSymbol, item, diagnostics);
    }

    public static ICategoryEntrySymbol CreateEntry(
        ISymbol containingSymbol,
        CategoryLinkNode item,
        DiagnosticBag diagnostics)
    {
        return new CategoryLinkSymbol(containingSymbol, item, diagnostics);
    }

    public static IForceEntrySymbol CreateEntry(
        ISymbol containingSymbol,
        ForceEntryNode item,
        DiagnosticBag diagnostics)
    {
        return new ForceEntrySymbol(containingSymbol, item, diagnostics);
    }

    public static ISelectionEntryContainerSymbol CreateEntry(
        ISymbol containingSymbol,
        SelectionEntryNode node,
        DiagnosticBag diagnostics)
    {
        return new SelectionEntrySymbol(containingSymbol, node, diagnostics);
    }

    public static ISelectionEntryContainerSymbol CreateEntry(
        ISymbol containingSymbol,
        SelectionEntryGroupNode node,
        DiagnosticBag diagnostics)
    {
        return new SelectionEntryGroupSymbol(containingSymbol, node, diagnostics);
    }
}
