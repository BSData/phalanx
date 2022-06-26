using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

/// <summary>
/// Separate symbol that is essentially a child of <see cref="EntrySymbol"/>.
/// </summary>
internal class PublicationReferenceSymbol : Symbol, IPublicationReferenceSymbol
{
    private IPublicationSymbol? lazyPublication;

    public PublicationReferenceSymbol(
        SourceDeclaredSymbol containingSymbol,
        IPublicationReferencingNode declaration,
        DiagnosticBag diagnostics)
    {
        Declaration = declaration;
        ContainingSymbol = containingSymbol;
        if (declaration.PublicationId is null)
        {
            // that's not what should happen, if publicationId is null,
            // the containing symbol should set its IPublicationRefernceSymbol property to null
            diagnostics.Add(
                ErrorCode.ERR_GenericError,
                containingSymbol.Declaration.GetLocation(),
                symbols: ImmutableArray.Create<Symbol>(this),
                args: "Publication reference must have a non-null publication ID.");
        }
    }

    public static PublicationReferenceSymbol? Create(
        SourceDeclaredSymbol containingSymbol,
        IPublicationReferencingNode declaration,
        DiagnosticBag diagnostics)
    {
        return declaration.PublicationId is null ? null : new PublicationReferenceSymbol(containingSymbol, declaration, diagnostics);
    }

    public IPublicationReferencingNode Declaration { get; }

    public override SymbolKind Kind => SymbolKind.Link;

    public override string? Id => null;

    public override string Name => string.Empty;

    public override string? Comment => null;

    public override SourceDeclaredSymbol ContainingSymbol { get; }

    internal override WhamCompilation DeclaringCompilation => ContainingSymbol.DeclaringCompilation;

    public IPublicationSymbol Publication => GetBoundField(ref lazyPublication);

    public string Page => Declaration.Page ?? "";

    internal override bool RequiresCompletion => true;

    protected override void BindReferences(WhamCompilation compilation, DiagnosticBag diagnostics)
    {
        base.BindReferences(compilation, diagnostics);

        var binder = compilation.GetBinder(ContainingSymbol.Declaration, ContainingSymbol);
        lazyPublication = binder.BindPublicationSymbol(Declaration, diagnostics);
    }
}
