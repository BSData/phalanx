namespace Phalanx.DataModel.Symbols.Implementation;

public class EntryPublicationReferenceSymbol : Symbol, IPublicationReferenceSymbol
{
    private readonly EntrySymbol containingSymbol;

    public EntryPublicationReferenceSymbol(EntrySymbol containingSymbol)
    {
        this.containingSymbol = containingSymbol;
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public override string Name => Publication.Name;

    public override string? Comment => null;

    public override ISymbol ContainingSymbol => ContainingEntrySymbol;

    public IEntrySymbol ContainingEntrySymbol => containingSymbol;

    public IPublicationSymbol Publication { get; } = null!; // TODO bind

    public string Page => containingSymbol.Declaration.Page ?? "";

    internal override Compilation DeclaringCompilation => containingSymbol.DeclaringCompilation;
}
