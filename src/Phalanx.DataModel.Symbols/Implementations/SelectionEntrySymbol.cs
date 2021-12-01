using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class SelectionEntrySymbol : SelectionEntryBaseSymbol, ISelectionEntrySymbol
{
    internal new SelectionEntryNode Declaration { get; }

    public SelectionEntrySymbol(
        ICatalogueItemSymbol containingSymbol,
        SelectionEntryNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.Entry;

    public override bool IsSelectionEntry => true;

    public SelectionEntryKind EntryKind => Declaration.Type;
}
