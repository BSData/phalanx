using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ConditionGroupConditionSymbol : TupleOperationConditionSymbol
{
    internal ConditionGroupNode Declaration { get; }

    public ConditionGroupConditionSymbol(
        ISymbol containingSymbol,
        ConditionGroupNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol)
    {
        Declaration = declaration;
        Operation = declaration.Type switch
        {
            ConditionGroupKind.And => TupleOperation.And,
            ConditionGroupKind.Or => TupleOperation.Or,
            _ => TupleOperation.Unknown
        };
        if (Operation is TupleOperation.Unknown)
        {
            diagnostics.Add(ErrorCode.ERR_UnknownEnumerationValue, Declaration);
        }
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

    public override TupleOperation Operation { get; }

    public override ImmutableArray<IConditionSymbol> Conditions { get; }
}
