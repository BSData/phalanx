using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class ModifierGroupEffectSymbol : EffectSymbol, IConditionalEffectSymbol
{
    public ModifierGroupEffectSymbol(
        ICatalogueItemSymbol containingSymbol,
        ModifierGroupNode declaration,
        Binder parentBinder,
        BindingDiagnosticContext diagnostics)
        : base(containingSymbol)
    {
        // BS_SPEC: modifier group creates a scope: all modifiers (and sub groups)
        // have their own conditions combined via AND with all conditions and condition groups
        // declared at that modifier's level. We achieve the same via IConditionalEffectSymbol:
        // any nested effects (sub modifiers and sub modifier groups) only execute if top condition
        // is satisfied, emulating the AND behavior.
        if (declaration.Repeats.Count > 0)
        {
            // TODO consider what happens when there are both repeats and conditions
            // create a loop effect
            diagnostics.Add("Repeats not implemented, ignoring");
        }
        Condition = new ModifierRootConditionSymbol(this, declaration, diagnostics);
        SatisfiedEffects = CreateChildEffects().ToImmutableArray();

        IEnumerable<IEffectSymbol> CreateChildEffects()
        {
            foreach (var item in declaration.Modifiers)
            {
                yield return CreateEffect(this, item, parentBinder, diagnostics);
            }
            foreach (var item in declaration.ModifierGroups)
            {
                yield return CreateEffect(this, item, parentBinder, diagnostics);
            }
        }
    }

    public IConditionSymbol Condition { get; }

    public ImmutableArray<IEffectSymbol> SatisfiedEffects { get; }

    public ImmutableArray<IEffectSymbol> UnsatisfiedEffects => ImmutableArray<IEffectSymbol>.Empty;
}
