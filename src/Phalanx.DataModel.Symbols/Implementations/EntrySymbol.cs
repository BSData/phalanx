using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class EntrySymbol : SourceCatalogueItemSymbol, IEntrySymbol
{
    internal EntryBaseNode Declaration { get; }

    protected EntrySymbol(
        ICatalogueItemSymbol containingSymbol,
        EntryBaseNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        if (declaration.PublicationId is not null)
        {
            PublicationReference = new EntryPublicationReferenceSymbol(this);
        }
        Effects = LogicSymbol.CreateEffects(this, diagnostics);
    }

    public bool IsHidden => Declaration.Hidden;

    protected virtual IEntrySymbol? BaseReferencedEntry => null;

    public IPublicationReferenceSymbol? PublicationReference { get; }

    public ImmutableArray<IEffectSymbol> Effects { get; }

    public bool IsReference => BaseReferencedEntry is not null;

    IEntrySymbol? IEntrySymbol.ReferencedEntry => BaseReferencedEntry;

    public static ISelectionEntryContainerSymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        EntryLinkNode item,
        DiagnosticBag diagnostics)
    {
        return new SelectionEntryLinkSymbol(containingSymbol, item, diagnostics);
    }

    public static IRuleSymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        RuleNode item,
        DiagnosticBag diagnostics)
    {
        return new RuleSymbol(containingSymbol, item, diagnostics);
    }

    public static IProfileSymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        ProfileNode item,
        DiagnosticBag diagnostics)
    {
        return new ProfileSymbol(containingSymbol, item, diagnostics);
    }

    public static IResourceEntrySymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        InfoLinkNode item,
        DiagnosticBag diagnostics)
    {
        return new ResourceLinkSymbol(containingSymbol, item, diagnostics);
    }

    public static IResourceGroupSymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        InfoGroupNode item,
        DiagnosticBag diagnostics)
    {
        return new ResourceGroupSymbol(containingSymbol, item, diagnostics);
    }

    public static ICostSymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        CostNode item,
        DiagnosticBag diagnostics)
    {
        return new CostSymbol(containingSymbol, item, diagnostics);
    }

    public static ICategoryEntrySymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        CategoryEntryNode item,
        DiagnosticBag diagnostics)
    {
        return new CategoryEntrySymbol(containingSymbol, item, diagnostics);
    }

    public static ICategoryEntrySymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        CategoryLinkNode item,
        DiagnosticBag diagnostics)
    {
        return new CategoryLinkSymbol(containingSymbol, item, diagnostics);
    }

    public static IForceEntrySymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        ForceEntryNode item,
        DiagnosticBag diagnostics)
    {
        return new ForceEntrySymbol(containingSymbol, item, diagnostics);
    }

    public static ISelectionEntryContainerSymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        SelectionEntryNode node,
        DiagnosticBag diagnostics)
    {
        return new SelectionEntrySymbol(containingSymbol, node, diagnostics);
    }

    public static ISelectionEntryContainerSymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        SelectionEntryGroupNode node,
        DiagnosticBag diagnostics)
    {
        return new SelectionEntryGroupSymbol(containingSymbol, node, diagnostics);
    }
}
