namespace Phalanx.DataModel.Symbols.Implementation;

public class EntryPublicationReferenceSymbol : Symbol, IPublicationReferenceSymbol
{
    private readonly EntrySymbol containingSymbol;
    private IPublicationSymbol? lazyPublication;

    public EntryPublicationReferenceSymbol(EntrySymbol containingSymbol)
    {
        this.containingSymbol = containingSymbol;
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public override string Name => Publication.Name;

    public override string? Comment => null;

    public override ISymbol ContainingSymbol => ContainingEntrySymbol;

    public IEntrySymbol ContainingEntrySymbol => containingSymbol;

    public IPublicationSymbol Publication
    {
        get
        {
            ForceComplete();
            return lazyPublication ?? throw new InvalidOperationException("Binding failed.");
        }
    }

    public string Page => containingSymbol.Declaration.Page ?? "";

    internal override Compilation DeclaringCompilation => containingSymbol.DeclaringCompilation;

    internal override bool RequiresCompletion => true;

    private bool BindingDone { get; set; }

    internal override void ForceComplete()
    {
        if (!BindingDone)
        {
            BindReferences();
        }
    }

    protected void BindReferences()
    {
        if (BindingDone)
            throw new InvalidOperationException("Already bound!");
        BindReferencesCore(DeclaringCompilation.GetBinder(containingSymbol.Declaration), DiagnosticBag.GetInstance());
        BindingDone = true;
    }

    protected virtual void BindReferencesCore(Binding.Binder binder, DiagnosticBag diagnosticBag)
    {
        lazyPublication = binder.BindPublicationSymbol(containingSymbol.Declaration.PublicationId);
    }
}
