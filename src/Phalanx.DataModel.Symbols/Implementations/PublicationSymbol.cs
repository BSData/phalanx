using System;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class PublicationSymbol : CatalogueItemSymbol, IPublicationSymbol
    {
        private readonly PublicationNode declaration;

        public PublicationSymbol(ICatalogueSymbol containingSymbol, PublicationNode declaration, BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, diagnostics)
        {
            this.declaration = declaration;
        }

        public override SymbolKind Kind => SymbolKind.ResourceType;

        public override string Name => declaration.Name ?? "";

        public string? Id => declaration.Id;

        public string? ShortName => declaration.ShortName;

        public string? Publisher => declaration.Publisher;

        public DateTime? PublicationDate => DateTime.TryParse(declaration.PublicationDate, out var result) ? result : null;

        public Uri? PublicationUrl => Uri.TryCreate(declaration.PublisherUrl, UriKind.Absolute, out var result) ? result : null;
    }
}
