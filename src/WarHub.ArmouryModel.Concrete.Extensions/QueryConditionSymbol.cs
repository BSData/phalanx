using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class QueryConditionSymbol : EffectSymbol, IQueryConditionSymbol
{
    internal ConditionNode Declaration { get; }

    public QueryConditionSymbol(
        ISymbol containingSymbol,
        ConditionNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol)
    {
        Declaration = declaration;
        Comparison = Declaration.Type switch
        {
            ConditionKind.EqualTo => QueryComparisonType.Equal,
            ConditionKind.LessThan => QueryComparisonType.LessThan,
            ConditionKind.GreaterThan => QueryComparisonType.GreaterThan,
            ConditionKind.NotEqualTo => QueryComparisonType.NotEqual,
            ConditionKind.AtLeast => QueryComparisonType.GreaterThanOrEqual,
            ConditionKind.AtMost => QueryComparisonType.LessThanOrEqual,
            ConditionKind.InstanceOf => throw new NotImplementedException(), // TODO consider separate class?
            ConditionKind.NotInstanceOf => throw new NotImplementedException(),
            _ => QueryComparisonType.Unknown
        };
        if (Comparison is QueryComparisonType.Unknown)
            diagnostics.Add(ErrorCode.ERR_UnknownEnumerationValue, Declaration);
        Query = new QuerySymbol(containingSymbol, Declaration, diagnostics);
    }

    public QueryComparisonType Comparison { get; }

    public decimal ComparisonValue => Declaration.Value; // TODO what about percentage?

    public IQuerySymbol Query { get; }
}
