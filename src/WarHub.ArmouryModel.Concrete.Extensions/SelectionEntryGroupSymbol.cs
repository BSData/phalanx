using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class SelectionEntryGroupSymbol : SelectionEntryBaseSymbol, ISelectionEntryGroupSymbol, INodeDeclaredSymbol<SelectionEntryGroupNode>
{
    private ISelectionEntryContainerSymbol? lazyDefaultEntry;

    public SelectionEntryGroupSymbol(
        ISymbol containingSymbol,
        SelectionEntryGroupNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override SelectionEntryGroupNode Declaration { get; }

    public override ContainerKind ContainerKind => ContainerKind.SelectionGroup;

    public ISelectionEntryContainerSymbol? DefaultSelectionEntry => GetOptionalBoundField(ref lazyDefaultEntry);

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);

        if (Declaration.DefaultSelectionEntryId is not null)
        {
            lazyDefaultEntry = binder.BindSelectionEntryGroupDefaultEntrySymbol(Declaration, this, diagnostics);
        }
    }
}
