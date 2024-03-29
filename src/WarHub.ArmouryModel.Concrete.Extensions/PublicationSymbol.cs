using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class PublicationSymbol : ResourceDefinitionBaseSymbol, IPublicationSymbol, INodeDeclaredSymbol<PublicationNode>
{
    public PublicationSymbol(
        ISymbol? containingSymbol,
        PublicationNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        PublicationDate = DateOnly.TryParse(Declaration.PublicationDate, out var pubDate) ? pubDate : null;
        PublicationUrl = Uri.TryCreate(Declaration.PublisherUrl, UriKind.Absolute, out var uri) ? uri : null;
    }

    public override PublicationNode Declaration { get; }

    public override ResourceKind ResourceKind => ResourceKind.Publication;

    public string? ShortName => Declaration.ShortName;

    public string? Publisher => Declaration.Publisher;

    public DateOnly? PublicationDate { get; }

    public Uri? PublicationUrl { get; }
}
