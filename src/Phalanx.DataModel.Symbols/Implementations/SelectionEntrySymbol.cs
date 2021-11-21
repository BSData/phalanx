using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class SelectionEntrySymbol : SelectionEntryBaseSymbol, ISelectionEntrySymbol
{
    private readonly SelectionEntryNode declaration;

    public SelectionEntrySymbol(
        ICatalogueItemSymbol containingSymbol,
        SelectionEntryNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        this.declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.Entry;

    public SelectionEntryKind EntryKind => declaration.Type;
}
