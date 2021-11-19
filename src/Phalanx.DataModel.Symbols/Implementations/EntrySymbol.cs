using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class EntrySymbol : CatalogueItemSymbol, IEntrySymbol
{
    internal EntryBaseNode Declaration { get; }

    protected EntrySymbol(
        ICatalogueItemSymbol containingSymbol,
        EntryBaseNode declaration,
        Binder binder,
        BindingDiagnosticContext diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        // consider binding at later stage
        var publication = binder.BindPublicationSymbol(declaration);
        if (publication is not null)
        {
            PublicationReference = new EntryPublicationReferenceSymbol(this, publication);
        }
        Effects = LogicSymbol.CreateEffects(this, binder, diagnostics);
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
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return new SelectionEntryLinkSymbol(containingSymbol, item, binder, diagnostics);
    }

    public static IRuleSymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        RuleNode item,
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return new RuleSymbol(containingSymbol, item, binder, diagnostics);
    }

    public static IProfileSymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        ProfileNode item,
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return new ProfileSymbol(containingSymbol, item, binder, diagnostics);
    }

    public static IResourceEntrySymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        InfoLinkNode item,
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return new ResourceLinkSymbol(containingSymbol, item, binder, diagnostics);
    }

    public static IResourceGroupSymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        InfoGroupNode item,
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return new ResourceGroupSymbol(containingSymbol, item, binder, diagnostics);
    }

    public static ICostSymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        CostNode item,
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return new CostSymbol(containingSymbol, item, binder, diagnostics);
    }

    public static ICategoryEntrySymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        CategoryEntryNode item,
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return new CategoryEntrySymbol(containingSymbol, item, binder, diagnostics);
    }

    public static ICategoryEntrySymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        CategoryLinkNode item,
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return new CategoryLinkSymbol(containingSymbol, item, binder, diagnostics);
    }

    public static IForceEntrySymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        ForceEntryNode item,
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return new ForceEntrySymbol(containingSymbol, item, binder, diagnostics);
    }

    public static ISelectionEntryContainerSymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        SelectionEntryNode node,
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return new SelectionEntrySymbol(containingSymbol, node, binder, diagnostics);
    }

    public static ISelectionEntryContainerSymbol CreateEntry(
        ICatalogueItemSymbol containingSymbol,
        SelectionEntryGroupNode node,
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return new SelectionEntryGroupSymbol(containingSymbol, node, binder, diagnostics);
    }
}
