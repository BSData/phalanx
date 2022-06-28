using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

/// <summary>
/// Separate symbol that is essentially a child of <see cref="EntrySymbol"/>.
/// </summary>
internal class PublicationReferenceSymbol : SourceDeclaredSymbol, IPublicationReferenceSymbol
{
    private IPublicationSymbol? lazyPublication;

    private PublicationReferenceSymbol(
        SourceDeclaredSymbol containingSymbol,
        IPublicationReferencingNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, (SourceNode)declaration)
    {
        PublicationRefDeclaration = declaration;
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
        SourceNode declaration,
        DiagnosticBag diagnostics)
    {
        return declaration is IPublicationReferencingNode { PublicationId: { } pubId } node
            ? new PublicationReferenceSymbol(containingSymbol, node, diagnostics)
            : null;
    }

    public IPublicationReferencingNode PublicationRefDeclaration { get; }

    public override SymbolKind Kind => SymbolKind.Link;

    public override string? Id => null;

    public override string Name => string.Empty;

    public override string? Comment => null;

    public IPublicationSymbol Publication => GetBoundField(ref lazyPublication);

    public string Page => PublicationRefDeclaration.Page ?? string.Empty;

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);
        lazyPublication = binder.BindPublicationSymbol(PublicationRefDeclaration, diagnostics);
    }
}
