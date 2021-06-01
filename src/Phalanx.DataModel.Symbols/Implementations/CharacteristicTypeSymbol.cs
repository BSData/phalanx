using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.SourceAnalysis;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class CharacteristicTypeSymbol : Symbol, ICharacteristicTypeSymbol
    {
        private readonly CharacteristicTypeNode node;

        public CharacteristicTypeSymbol(
            IProfileTypeSymbol containingSymbol,
            CharacteristicTypeNode node,
            GamesystemContext context,
            BindingDiagnosticContext diagnostics)
        {
            this.node = node;
            ContainingProfileType = containingSymbol;
        }

        public override SymbolKind Kind => SymbolKind.ResourceType;

        public override string Name => node.Name ?? "";

        public override string? Comment => node.Comment;

        public override ISymbol ContainingSymbol => ContainingProfileType;

        public IProfileTypeSymbol ContainingProfileType { get; }

        public string? Id => node.Id;

        public ICatalogueSymbol ContainingCatalogue => ContainingProfileType.ContainingCatalogue;
    }
}
