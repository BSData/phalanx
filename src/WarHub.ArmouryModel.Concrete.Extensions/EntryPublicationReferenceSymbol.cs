namespace WarHub.ArmouryModel.Concrete;

/// <summary>
/// Separate symbol that is essentially a child of <see cref="EntrySymbol"/>.
/// TODO consider merging with the parent, or deriving from <see cref="SourceDeclaredSymbol"/>.
/// </summary>
internal class EntryPublicationReferenceSymbol : Symbol, IPublicationReferenceSymbol
{
    private readonly EntrySymbol containingSymbol;

    private IPublicationSymbol? lazyPublication;

    public EntryPublicationReferenceSymbol(EntrySymbol containingSymbol, DiagnosticBag diagnostics)
    {
        if (containingSymbol.Declaration.PublicationId is null)
        {
            // that's not what should happen, if publicationId is null,
            // the containing symbol should set its IPublicationRefernceSymbol property to null
            diagnostics.Add(
                ErrorCode.ERR_GenericError,
                containingSymbol.Declaration.GetLocation(),
                ImmutableArray.Create<Symbol>(this));
        }
        this.containingSymbol = containingSymbol;
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public override string? Id => null;

    public override string Name => string.Empty;

    public override string? Comment => null;

    public override ISymbol ContainingSymbol => ContainingEntrySymbol;

    public IEntrySymbol ContainingEntrySymbol => containingSymbol;

    internal override WhamCompilation DeclaringCompilation => containingSymbol.DeclaringCompilation;

    public IPublicationSymbol Publication => GetBoundField(ref lazyPublication);

    public string Page => containingSymbol.Declaration.Page ?? "";

    internal override bool RequiresCompletion => true;

    protected override void BindReferences(WhamCompilation compilation, DiagnosticBag diagnostics)
    {
        base.BindReferences(compilation, diagnostics);

        var binder = compilation.GetBinder(containingSymbol.Declaration, ContainingSymbol);
        lazyPublication = binder.BindPublicationSymbol(containingSymbol.Declaration, diagnostics);
    }
}
