using System.Collections.Immutable;
using System.Linq;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class ProfileTypeSymbol : CatalogueItemSymbol, IProfileTypeSymbol
    {
        private readonly ProfileTypeNode declaration;

        public ProfileTypeSymbol(
            ICatalogueSymbol containingSymbol,
            ProfileTypeNode declaration,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, diagnostics)
        {
            this.declaration = declaration;
            CharacteristicTypes = declaration.CharacteristicTypes
                .Select(x => new CharacteristicTypeSymbol(this, x, diagnostics))
                .ToImmutableArray<ICharacteristicTypeSymbol>();
        }

        public override SymbolKind Kind => SymbolKind.ResourceType;

        public ImmutableArray<ICharacteristicTypeSymbol> CharacteristicTypes { get; }
    }
}
