using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class SelectionEntryGroupSymbol : SelectionEntryBaseSymbol, ISelectionEntryGroupSymbol, INodeDeclaredSymbol<SelectionEntryGroupNode>
{
    private ISelectionEntrySymbol? lazyDefaultEntry;

    public SelectionEntryGroupSymbol(
        ISymbol containingSymbol,
        SelectionEntryGroupNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override SelectionEntryGroupNode Declaration { get; }

    public override ContainerEntryKind ContainerKind => ContainerEntryKind.SelectionGroup;

    public ISelectionEntrySymbol? DefaultSelectionEntry => GetOptionalBoundField(ref lazyDefaultEntry);

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        if (Declaration.DefaultSelectionEntryId is not null)
        {
            lazyDefaultEntry = binder.BindSelectionEntryGroupDefaultEntrySymbol(Declaration, diagnosticBag);
        }
    }
}
