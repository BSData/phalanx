using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal class PublicationSymbol : SourceDeclaredSymbol, IPublicationSymbol
{
    internal new PublicationNode Declaration { get; }

    public PublicationSymbol(
        ICatalogueSymbol containingSymbol,
        PublicationNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        PublicationDate = DateOnly.TryParse(Declaration.PublicationDate, out var pubDate) ? pubDate : null;
        PublicationUrl = Uri.TryCreate(Declaration.PublisherUrl, UriKind.Absolute, out var uri) ? uri : null;
    }

    public override SymbolKind Kind => SymbolKind.ResourceDefinition;

    public ResourceKind ResourceKind => ResourceKind.Publication;

    public string? ShortName => Declaration.ShortName;

    public string? Publisher => Declaration.Publisher;

    public DateOnly? PublicationDate { get; }

    public Uri? PublicationUrl { get; }
}
