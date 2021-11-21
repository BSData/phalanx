using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class ResourceLinkSymbol : EntrySymbol, IResourceEntrySymbol
{
    public ResourceLinkSymbol(
        ICatalogueItemSymbol containingSymbol,
        InfoLinkNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        ReferencedEntry = null; // TODO bind
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public IResourceDefinitionSymbol? Type => null;

    public IResourceEntrySymbol? ReferencedEntry { get; }
}
