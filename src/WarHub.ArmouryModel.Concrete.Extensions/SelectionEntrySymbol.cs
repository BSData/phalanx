using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class SelectionEntrySymbol : SelectionEntryBaseSymbol, ISelectionEntrySymbol, INodeDeclaredSymbol<SelectionEntryNode>
{
    public SelectionEntrySymbol(
        ISymbol containingSymbol,
        SelectionEntryNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override SelectionEntryNode Declaration { get; }

    public override ContainerKind ContainerKind => ContainerKind.Selection;

    public SelectionEntryKind EntryKind => Declaration.Type;
}
