using System.Collections.Immutable;
using System.Linq;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.SourceAnalysis;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class ProfileTypeSymbol : Symbol, IProfileTypeSymbol
    {
        private readonly ProfileTypeNode node;

        public ProfileTypeSymbol(
            ICatalogueSymbol containingSymbol,
            ProfileTypeNode node,
            GamesystemContext context,
            BindingDiagnosticContext diagnostics)
        {
            this.node = node;
            ContainingCatalogue = containingSymbol;
            CharacteristicTypes = node.CharacteristicTypes
                .Select(x => new CharacteristicTypeSymbol(this, x, context, diagnostics))
                .ToImmutableArray<ICharacteristicTypeSymbol>();
        }

        public override SymbolKind Kind => SymbolKind.ResourceType;

        public override string Name => node.Name ?? "";

        public override string? Comment => node.Comment;

        public override ISymbol ContainingSymbol => ContainingCatalogue;

        public string? Id => node.Id;

        public ICatalogueSymbol ContainingCatalogue { get; }

        public ImmutableArray<ICharacteristicTypeSymbol> CharacteristicTypes { get; }
    }
}
