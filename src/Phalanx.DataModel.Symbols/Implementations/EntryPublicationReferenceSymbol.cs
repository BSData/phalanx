namespace Phalanx.DataModel.Symbols.Implementation;

public class EntryPublicationReferenceSymbol : Symbol, IPublicationReferenceSymbol
{
    private readonly EntrySymbol containingSymbol;

    public EntryPublicationReferenceSymbol(EntrySymbol containingSymbol, IPublicationSymbol publication)
    {
        this.containingSymbol = containingSymbol;
        Publication = publication;
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public override string Name => Publication.Name;

    public override string? Comment => null;

    public override ISymbol ContainingSymbol => ContainingEntrySymbol;

    public IEntrySymbol ContainingEntrySymbol => containingSymbol;

    public IPublicationSymbol Publication { get; }

    public string Page => containingSymbol.Declaration.Page ?? "";
}
