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
    }

    public override SymbolKind Kind => SymbolKind.ResourceType;

    public string? ShortName => Declaration.ShortName;

    public string? Publisher => Declaration.Publisher;

    public DateTime? PublicationDate => DateTime.TryParse(Declaration.PublicationDate, out var result) ? result : null;

    public Uri? PublicationUrl => Uri.TryCreate(Declaration.PublisherUrl, UriKind.Absolute, out var result) ? result : null;
}
