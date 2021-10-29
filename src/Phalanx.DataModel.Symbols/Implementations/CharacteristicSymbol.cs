using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class CharacteristicSymbol : SimpleResourceEntrySymbol, ICharacteristicSymbol
    {
        private readonly CharacteristicNode declaration;

        public CharacteristicSymbol(
            IProfileSymbol containingSymbol,
            CharacteristicNode declaration,
            Binder binder,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, diagnostics)
        {
            this.declaration = declaration;
            Type = null!; // TODO bind
        }

        public ICharacteristicTypeSymbol Type { get; }

        public string Value => declaration.Value ?? string.Empty;

        protected override IResourceDefinitionSymbol? BaseType => Type;
    }
}
