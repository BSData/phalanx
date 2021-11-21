using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class PublicationSymbol : SourceCatalogueItemSymbol, IPublicationSymbol
{
    private readonly PublicationNode declaration;

    public PublicationSymbol(
        ICatalogueSymbol containingSymbol,
        PublicationNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        this.declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.ResourceType;

    public string? ShortName => declaration.ShortName;

    public string? Publisher => declaration.Publisher;

    public DateTime? PublicationDate => DateTime.TryParse(declaration.PublicationDate, out var result) ? result : null;

    public Uri? PublicationUrl => Uri.TryCreate(declaration.PublisherUrl, UriKind.Absolute, out var result) ? result : null;
}
