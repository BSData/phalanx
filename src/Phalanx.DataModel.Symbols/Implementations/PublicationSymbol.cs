using System;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.SourceAnalysis;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class PublicationSymbol : Symbol, IPublicationSymbol
    {
        private readonly PublicationNode node;

        public PublicationSymbol(ICatalogueSymbol containingSymbol, PublicationNode node, GamesystemContext context, BindingDiagnosticContext diagnostics)
        {
            this.node = node;
            ContainingCatalogue = containingSymbol;
        }

        public override SymbolKind Kind => SymbolKind.ResourceType;

        public override string Name => node.Name ?? "";

        public override string? Comment => node.Comment;

        public override ISymbol ContainingSymbol => ContainingCatalogue;

        public string? Id => node.Id;

        public ICatalogueSymbol ContainingCatalogue { get; }

        public string? ShortName => node.ShortName;

        public string? Publisher => node.Publisher;

        public DateTime? PublicationDate => DateTime.TryParse(node.PublicationDate, out var result) ? result : null;

        public Uri? PublicationUrl => Uri.TryCreate(node.PublisherUrl, UriKind.Absolute, out var result) ? result : null;
    }
}
