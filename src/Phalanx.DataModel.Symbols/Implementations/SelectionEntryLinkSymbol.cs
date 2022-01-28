using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal class SelectionEntryLinkSymbol : SelectionEntryBaseSymbol
{
    private ISelectionEntryContainerSymbol? lazyReference;

    internal new EntryLinkNode Declaration { get; }

    public SelectionEntryLinkSymbol(
        ISymbol containingSymbol,
        EntryLinkNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override ContainerEntryKind ContainerKind => Declaration.Type switch
    {
        EntryLinkKind.SelectionEntry => ContainerEntryKind.Selection,
        EntryLinkKind.SelectionEntryGroup => ContainerEntryKind.SelectionGroup,
        _ => throw new NotSupportedException($"Unknown value '{Declaration.Type}'"),
    };

    public override ISelectionEntryContainerSymbol? ReferencedEntry => GetBoundField(ref lazyReference);

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyReference = binder.BindSharedSelectionEntrySymbol(Declaration.TargetId, Declaration.Type);
    }
}
