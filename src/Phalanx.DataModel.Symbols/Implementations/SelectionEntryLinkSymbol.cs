using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class SelectionEntryLinkSymbol : SelectionEntryBaseSymbol
{
    private readonly EntryLinkNode declaration;

    public SelectionEntryLinkSymbol(
        ICatalogueItemSymbol containingSymbol,
        EntryLinkNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        this.declaration = declaration;
        ReferencedEntry = null; // TODO bind
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public override ISelectionEntryContainerSymbol? ReferencedEntry { get; }
}
