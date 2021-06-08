using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public abstract class CatalogueItemSymbol : Symbol, ICatalogueItemSymbol
    {
        private readonly CommentableNode declaration;
        private readonly BindingDiagnosticContext diagnostics;

        public CatalogueItemSymbol(
            ISymbol containingSymbol,
            CommentableNode declaration,
            BindingDiagnosticContext diagnostics)
            : this(
                containingSymbol,
                containingSymbol is ICatalogueSymbol c ? c : ((ICatalogueItemSymbol)containingSymbol).ContainingCatalogue,
                declaration,
                diagnostics)
        {
        }

        private CatalogueItemSymbol(
            ISymbol containingSymbol,
            ICatalogueSymbol containingCatalogue,
            CommentableNode declaration,
            BindingDiagnosticContext diagnostics)
        {
            ContainingSymbol = containingSymbol;
            ContainingCatalogue = containingCatalogue;
            this.declaration = declaration;
            this.diagnostics = diagnostics;
        }

        public override string? Comment => declaration.Comment;

        public override ISymbol ContainingSymbol { get; }

        public ICatalogueSymbol ContainingCatalogue { get; }
    }
}
