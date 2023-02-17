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
        ContainerKind = declaration.Type switch
        {
            EntryLinkKind.SelectionEntry => ContainerKind.Selection,
            EntryLinkKind.SelectionEntryGroup => ContainerKind.SelectionGroup,
            _ => ContainerKind.Error,
        };
        if (ContainerKind is ContainerKind.Error)
        {
            diagnostics.Add(
                ErrorCode.ERR_UnknownEnumerationValue,
                declaration.GetLocation(),
                declaration.Type);
        }
    }

    public override EntryLinkNode Declaration { get; }

    public override ContainerKind ContainerKind { get; }

    public override ISelectionEntryContainerSymbol ReferencedEntry => GetBoundField(ref lazyReference);

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);

        lazyReference = binder.BindSelectionEntrySymbol(Declaration, diagnostics);
    }
}
