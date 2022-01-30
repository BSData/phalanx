using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ModifierRootConditionSymbol : TupleOperationConditionSymbol
{
    public ModifierRootConditionSymbol(
        ISymbol containingSymbol,
        ModifierBaseNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol)
    {
        Conditions = GetChildSymbols().ToImmutableArray();

        IEnumerable<IConditionSymbol> GetChildSymbols()
        {
            foreach (var node in declaration.Conditions)
            {
                yield return new QueryConditionSymbol(this, node, diagnostics);
            }
            foreach (var node in declaration.ConditionGroups)
            {
                yield return new ConditionGroupConditionSymbol(this, node, diagnostics);
            }
        }
    }

    // BS_SPEC: All conditions and groups at the modifier's root combine as an AND operator.
    public override TupleOperation Operation => TupleOperation.And;

    public override ImmutableArray<IConditionSymbol> Conditions { get; }
}
