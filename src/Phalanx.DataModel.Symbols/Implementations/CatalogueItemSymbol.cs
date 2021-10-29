using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public abstract class CatalogueItemSymbol : Symbol, ICatalogueItemSymbol
    {
        protected CatalogueItemSymbol(
            ISymbol containingSymbol,
            SourceNode declaration,
            BindingDiagnosticContext diagnostics)
        {
            ContainingSymbol = containingSymbol;
            ContainingCatalogue = (containingSymbol as ICatalogueSymbol)
                ?? ((ICatalogueItemSymbol)containingSymbol).ContainingCatalogue;
            Id = (declaration as IIdentifiableNode)?.Id;
            Name = (declaration as INameableNode)?.Name ?? string.Empty;
            Comment = (declaration as CommentableNode)?.Comment;
        }

        public string? Id { get; }

        public sealed override string Name { get; }

        public sealed override string? Comment { get; }

        public sealed override ISymbol ContainingSymbol { get; }

        public ICatalogueSymbol ContainingCatalogue { get; }
    }
}
