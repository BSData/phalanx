using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ModifyingEffectSymbol : EffectSymbol, IModifyingEffectSymbol
{
    internal ModifierNode Declaration { get; }

    public ModifyingEffectSymbol(
        ISymbol containingSymbol,
        ModifierNode declaration)
        : base(containingSymbol)
    {
        Declaration = declaration;
    }

    public override string Name => "";

    public override string? Comment => Declaration.Comment;

    public string? TargetField => Declaration.Field; // TODO bind, but how?

    public ModifierKind Type => Declaration.Type; // TODO other enum or different approach (DU?)

    public string? ModificationValue => Declaration.Value;
}
