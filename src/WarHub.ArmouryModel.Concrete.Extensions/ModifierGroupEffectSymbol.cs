using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ModifierGroupEffectSymbol : EffectSymbol, IConditionalEffectSymbol, INodeDeclaredSymbol<ModifierGroupNode>
{
    public ModifierGroupEffectSymbol(
        ISymbol containingSymbol,
        ModifierGroupNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol)
    {
        Declaration = declaration;
        // BS_SPEC: modifier group creates a scope: all modifiers (and sub groups)
        // have their own conditions combined via AND with all conditions and condition groups
        // declared at that modifier's level. We achieve the same via IConditionalEffectSymbol:
        // any nested effects (sub modifiers and sub modifier groups) only execute if top condition
        // is satisfied, emulating the AND behavior.
        if (declaration.Repeats.Count > 0)
        {
            // TODO implement repeats
            // TODO consider what happens when there are both repeats and conditions
            // create a loop effect
            diagnostics.Add(ErrorCode.ERR_SyntaxSupportNotYetImplemented, declaration.Repeats);
        }
        Condition = new ModifierRootConditionSymbol(this, declaration, diagnostics);
        SatisfiedEffects = CreateChildEffects().ToImmutableArray();

        IEnumerable<IEffectSymbol> CreateChildEffects()
        {
            foreach (var item in declaration.Modifiers)
            {
                yield return CreateEffect(this, item, diagnostics);
            }
            foreach (var item in declaration.ModifierGroups)
            {
                yield return CreateEffect(this, item, diagnostics);
            }
        }
    }

    public ModifierGroupNode Declaration { get; }

    public IConditionSymbol Condition { get; }

    public ImmutableArray<IEffectSymbol> SatisfiedEffects { get; }

    public ImmutableArray<IEffectSymbol> UnsatisfiedEffects => ImmutableArray<IEffectSymbol>.Empty;
}
