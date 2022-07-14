using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class EntrySymbol : SourceDeclaredSymbol, IEntrySymbol
{
    protected EntrySymbol(
        ISymbol? containingSymbol,
        SourceNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        PublicationReference = PublicationReferenceSymbol.Create(this, declaration, diagnostics);
        if (declaration is EntryBaseNode entryNode)
        {
            IsHidden = entryNode.Hidden;
            Effects = LogicSymbol.CreateEffects(this, entryNode, diagnostics);
        }
    }

    public bool IsReference => ReferencedEntry is not null;

    public bool IsHidden { get; }

    public virtual IEntrySymbol? ReferencedEntry => null;

    IEntrySymbol? IEntrySymbol.ReferencedEntry => ReferencedEntry;

    public PublicationReferenceSymbol? PublicationReference { get; }

    IPublicationReferenceSymbol? IEntrySymbol.PublicationReference => PublicationReference;

    public ImmutableArray<EffectSymbol> Effects { get; } = ImmutableArray<EffectSymbol>.Empty;

    ImmutableArray<IEffectSymbol> IEntrySymbol.Effects => Effects.Cast<EffectSymbol, IEffectSymbol>();

    public abstract ImmutableArray<ResourceEntryBaseSymbol> Resources { get; }

    ImmutableArray<IResourceEntrySymbol> IEntrySymbol.Resources =>
        Resources.Cast<ResourceEntryBaseSymbol, IResourceEntrySymbol>();

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        (PublicationReference is null ? ImmutableArray<Symbol>.Empty : ImmutableArray.Create<Symbol>(PublicationReference))
        .AddRange(base.MakeAllMembers(diagnostics))
        .AddRange(Resources.Cast<ResourceEntryBaseSymbol, Symbol>())
        .AddRange(Effects.Cast<EffectSymbol, Symbol>());

    public static SelectionEntryLinkSymbol CreateEntry(
        ISymbol containingSymbol,
        EntryLinkNode item,
        DiagnosticBag diagnostics)
    {
        return new SelectionEntryLinkSymbol(containingSymbol, item, diagnostics);
    }

    public static RuleSymbol CreateEntry(
        ISymbol containingSymbol,
        RuleNode item,
        DiagnosticBag diagnostics)
    {
        return new RuleSymbol(containingSymbol, item, diagnostics);
    }

    public static ProfileSymbol CreateEntry(
        ISymbol containingSymbol,
        ProfileNode item,
        DiagnosticBag diagnostics)
    {
        return new ProfileSymbol(containingSymbol, item, diagnostics);
    }

    public static ResourceLinkSymbol CreateEntry(
        ISymbol containingSymbol,
        InfoLinkNode item,
        DiagnosticBag diagnostics)
    {
        return new ResourceLinkSymbol(containingSymbol, item, diagnostics);
    }

    public static ResourceGroupSymbol CreateEntry(
        ISymbol containingSymbol,
        InfoGroupNode item,
        DiagnosticBag diagnostics)
    {
        return new ResourceGroupSymbol(containingSymbol, item, diagnostics);
    }

    public static CostSymbol CreateEntry(
        ISymbol containingSymbol,
        CostNode item,
        DiagnosticBag diagnostics)
    {
        return new CostSymbol(containingSymbol, item, diagnostics);
    }

    public static CategoryEntrySymbol CreateEntry(
        ISymbol containingSymbol,
        CategoryEntryNode item,
        DiagnosticBag diagnostics)
    {
        return new CategoryEntrySymbol(containingSymbol, item, diagnostics);
    }

    public static CategoryLinkSymbol CreateEntry(
        ISymbol containingSymbol,
        CategoryLinkNode item,
        DiagnosticBag diagnostics)
    {
        return new CategoryLinkSymbol(containingSymbol, item, diagnostics);
    }

    public static ForceEntrySymbol CreateEntry(
        ISymbol containingSymbol,
        ForceEntryNode item,
        DiagnosticBag diagnostics)
    {
        return new ForceEntrySymbol(containingSymbol, item, diagnostics);
    }

    public static SelectionEntryBaseSymbol CreateEntry(
        ISymbol containingSymbol,
        SelectionEntryNode node,
        DiagnosticBag diagnostics)
    {
        return new SelectionEntrySymbol(containingSymbol, node, diagnostics);
    }

    public static SelectionEntryBaseSymbol CreateEntry(
        ISymbol containingSymbol,
        SelectionEntryGroupNode node,
        DiagnosticBag diagnostics)
    {
        return new SelectionEntryGroupSymbol(containingSymbol, node, diagnostics);
    }
}
