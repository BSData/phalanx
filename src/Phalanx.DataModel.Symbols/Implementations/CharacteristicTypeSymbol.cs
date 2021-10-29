using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class CharacteristicTypeSymbol : CatalogueItemSymbol, ICharacteristicTypeSymbol
    {
        private readonly CharacteristicTypeNode declaration;

        public CharacteristicTypeSymbol(
            IProfileTypeSymbol containingSymbol,
            CharacteristicTypeNode declaration,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, diagnostics)
        {
            this.declaration = declaration;
            ContainingProfileType = containingSymbol;
        }

        public override SymbolKind Kind => SymbolKind.ResourceType;

        public IProfileTypeSymbol ContainingProfileType { get; }
    }
}
