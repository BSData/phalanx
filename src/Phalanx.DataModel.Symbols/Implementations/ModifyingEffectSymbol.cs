using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class ModifyingEffectSymbol : EffectSymbol, IModifyingEffectSymbol
    {
        private readonly ModifierNode declaration;

        public ModifyingEffectSymbol(
            ICatalogueItemSymbol containingSymbol,
            ModifierNode declaration)
            : base(containingSymbol)
        {
            this.declaration = declaration;
        }
    }
}
