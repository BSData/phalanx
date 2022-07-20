using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ModifierGroupEffectSymbol : ModifierEffectBaseSymbol, IEffectSymbol, INodeDeclaredSymbol<ModifierGroupNode>
{
    public ModifierGroupEffectSymbol(
        ISymbol? containingSymbol,
        ModifierGroupNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        ChildrenWhenSatisfied = CreateChildEffects().ToImmutableArray();

        IEnumerable<ModifierEffectBaseSymbol> CreateChildEffects()
        {
            foreach (var item in declaration.Modifiers)
            {
                yield return Create(this, item, diagnostics);
            }
            foreach (var item in declaration.ModifierGroups)
            {
                yield return Create(this, item, diagnostics);
            }
        }
    }

    public new ModifierGroupNode Declaration { get; }

    public override ImmutableArray<ModifierEffectBaseSymbol> ChildrenWhenSatisfied { get; }

    public override EffectTargetKind TargetKind => EffectTargetKind.None;

    public override ISymbol? TargetMember => null;

    public override EffectOperation FunctionKind => EffectOperation.None;

    public override string? OperandValue => null;

    public override ISymbol? OperandSymbol => null;
}
