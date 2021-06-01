using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.SourceAnalysis;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class CostTypeSymbol : Symbol, ICostTypeSymbol
    {
        private readonly CostTypeNode node;

        public CostTypeSymbol(ICatalogueSymbol containingSymbol, CostTypeNode node, GamesystemContext context, BindingDiagnosticContext diagnostics)
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
    }
}
