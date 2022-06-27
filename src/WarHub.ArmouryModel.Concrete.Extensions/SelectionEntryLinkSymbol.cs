using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class SelectionEntryLinkSymbol : SelectionEntryBaseSymbol, INodeDeclaredSymbol<EntryLinkNode>
{
    private ISelectionEntryContainerSymbol? lazyReference;

    public SelectionEntryLinkSymbol(
        ISymbol containingSymbol,
        EntryLinkNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        ContainerKind = Declaration.Type switch
        {
            EntryLinkKind.SelectionEntry => ContainerEntryKind.Selection,
            EntryLinkKind.SelectionEntryGroup => ContainerEntryKind.SelectionGroup,
            _ => ContainerEntryKind.Error,
        };
        if (ContainerKind is ContainerEntryKind.Error)
        {
            diagnostics.Add(ErrorCode.ERR_UnknownEnumerationValue, Declaration);
        }
    }

    public override EntryLinkNode Declaration { get; }

    public override ContainerEntryKind ContainerKind { get; }

    public override ISelectionEntryContainerSymbol ReferencedEntry => GetBoundField(ref lazyReference);

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);

        lazyReference = binder.BindSharedSelectionEntrySymbol(Declaration, diagnostics);
    }
}
