using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class ModifierEffectSymbol : EffectSymbol, IConditionalEffectSymbol
{
    public ModifierEffectSymbol(
        ISymbol containingSymbol,
        ModifierNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol)
    {
        if (declaration.Repeats.Count > 0)
        {
            // TODO implement repeats
            // TODO consider what happens when there are both repeats and conditions
            // create a loop effect
            diagnostics.Add("Repeats not implemented, ignoring");
        }
        Condition = new ModifierRootConditionSymbol(this, declaration, diagnostics);
        var satisfiedEffect = new ModifyingEffectSymbol(this, declaration);
        SatisfiedEffects = ImmutableArray.Create<IEffectSymbol>(satisfiedEffect);
    }

    public IConditionSymbol Condition { get; }

    public ImmutableArray<IEffectSymbol> SatisfiedEffects { get; }

    public ImmutableArray<IEffectSymbol> UnsatisfiedEffects => ImmutableArray<IEffectSymbol>.Empty;
}
