using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal class SelectionEntrySymbol : SelectionEntryBaseSymbol, ISelectionEntrySymbol
{
    internal new SelectionEntryNode Declaration { get; }

    public SelectionEntrySymbol(
        ISymbol containingSymbol,
        SelectionEntryNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override ContainerEntryKind ContainerKind => ContainerEntryKind.Category;

    public SelectionEntryKind EntryKind => Declaration.Type;
}
