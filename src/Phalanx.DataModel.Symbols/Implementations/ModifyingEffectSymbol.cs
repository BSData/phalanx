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

        public override string Name => "";

        public override string? Comment => declaration.Comment;

        public string? TargetField => declaration.Field; // TODO resolve modifier target

        public ModifierKind Type => declaration.Type; // TODO other enum or different approach (DU?)

        public string? ModificationValue => declaration.Value;
    }
}
