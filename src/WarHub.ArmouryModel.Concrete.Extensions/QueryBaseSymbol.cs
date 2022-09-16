using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class QueryBaseSymbol : LogicBaseSymbol, IQuerySymbol
{
    private ISymbol? lazyValueType;
    private ISymbol? lazyScope;
    private ISymbol? lazyFilter;

    public QueryBaseSymbol(
        ISymbol containingSymbol,
        QueryBaseNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        ValueKind = declaration.Field switch
        {
            "forces" => QueryValueKind.ForceCount,
            "selections" => QueryValueKind.SelectionCount,
            { } id when id.StartsWith("limit::", StringComparison.Ordinal) => QueryValueKind.MemberValueLimit,
            { } id when !string.IsNullOrWhiteSpace(id) => QueryValueKind.MemberValue,
            _ => QueryValueKind.Unknown
        };
        if (ValueKind is QueryValueKind.Unknown)
        {
            diagnostics.Add(
                ErrorCode.ERR_UnknownEnumerationValue,
                declaration.GetLocation(),
                declaration.Field ?? "field");
        }

        ScopeKind = declaration.Scope switch
        {
            "self" => QueryScopeKind.Self,
            "parent" => QueryScopeKind.Parent,
            "ancestor" => QueryScopeKind.ContainingAncestor,
            "primary-category" => QueryScopeKind.PrimaryCategory,
            "primary-catalogue" => QueryScopeKind.PrimaryCatalogue,
            "force" => QueryScopeKind.ContainingForce,
            "roster" => QueryScopeKind.ContainingRoster,
            { } id when !string.IsNullOrWhiteSpace(id) => QueryScopeKind.ReferencedEntry,
            _ => QueryScopeKind.Unknown
        };
        if (ValueKind is QueryValueKind.Unknown)
        {
            diagnostics.Add(
                ErrorCode.ERR_UnknownEnumerationValue,
                declaration.GetLocation(),
                declaration.Scope ?? "scope");
        }

        ValueFilterKind = declaration is QueryFilteredBaseNode { } filtered
            ? filtered.ChildId switch
            {
                "any" => QueryFilterKind.Anything,
                "unit" => QueryFilterKind.UnitEntry,
                "model" => QueryFilterKind.ModelEntry,
                "upgrade" => QueryFilterKind.UpgradeEntry,
                { } id when !string.IsNullOrWhiteSpace(id) => QueryFilterKind.SpecifiedEntry,
                _ => QueryFilterKind.Unknown
            }
            : QueryFilterKind.Anything;
        if (ValueFilterKind is QueryFilterKind.Unknown)
        {
            diagnostics.Add(
                ErrorCode.ERR_UnknownEnumerationValue,
                declaration.GetLocation(),
                 (declaration as QueryFilteredBaseNode)?.ChildId ?? "childId");
        }
        Options = CreateOptions();
        // TODO more diagnostics

        QueryOptions CreateOptions()
        {
            var options = QueryOptions.None;
            if (declaration.Shared)
            {
                options |= QueryOptions.SharedConstraint;
            }
            if (declaration.IncludeChildForces)
            {
                options |= QueryOptions.IncludeDescendantForces;
            }
            if (declaration.IncludeChildSelections)
            {
                options |= QueryOptions.IncludeDescendantSelections;
            }
            if (declaration.IsValuePercentage)
            {
                options |= QueryOptions.ValuePercentage;
            }
            if (declaration is RepeatNode { RoundUp: true })
            {
                options |= QueryOptions.ValueRoundUp;
            }
            return options;
        }
    }

    public new QueryBaseNode Declaration { get; }

    public sealed override SymbolKind Kind => SymbolKind.Query;

    public abstract QueryComparisonType Comparison { get; }

    public decimal? ReferenceValue => Declaration.Value;

    public QueryValueKind ValueKind { get; }

    public ISymbol? ValueTypeSymbol => GetOptionalBoundField(ref lazyValueType);

    public QueryScopeKind ScopeKind { get; }

    public ISymbol? ScopeSymbol => GetOptionalBoundField(ref lazyScope);

    public QueryFilterKind ValueFilterKind { get; }

    public ISymbol? FilterSymbol => GetOptionalBoundField(ref lazyFilter);

    public QueryOptions Options { get; }

    public static QueryBaseSymbol Create(ISymbol containingSymbol, QueryBaseNode declaration, DiagnosticBag diagnostics)
    {
        return declaration switch
        {
            ConditionNode x => new ConditionQuerySymbol(containingSymbol, x, diagnostics),
            ConstraintNode x => new ConstraintQuerySymbol(containingSymbol, x, diagnostics),
            RepeatNode x => new RepeatQuerySymbol(containingSymbol, x, diagnostics),
            _ => throw new InvalidOperationException("Unsupported declaration type.")
        };
    }

    public sealed override void Accept(SymbolVisitor visitor) =>
        visitor.VisitQuery(this);

    public sealed override TResult Accept<TResult>(SymbolVisitor<TResult> visitor) =>
        visitor.VisitQuery(this);

    public sealed override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument) =>
        visitor.VisitQuery(this, argument);

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);
        if (ValueKind is QueryValueKind.MemberValue)
        {
            lazyValueType = binder.BindEntryMemberSymbol(Declaration, Declaration.Field, diagnostics);
        }
        else if (ValueKind is QueryValueKind.MemberValueLimit)
        {
            // limit:: prefix (e.g. limit::abc-123) is used to describe roster cost limit
            lazyValueType = binder.BindCostTypeSymbol(Declaration, Declaration.Field?["limit::".Length..], diagnostics);
        }
        if (ScopeKind is QueryScopeKind.ReferencedEntry)
        {
            lazyScope = binder.BindScopeEntrySymbol(Declaration, Declaration.Scope, diagnostics);
        }
        if (ValueFilterKind is QueryFilterKind.SpecifiedEntry)
        {
            lazyFilter = binder.BindFilterEntrySymbol(Declaration, Declaration.Scope, diagnostics);
        }
    }

    internal class ConditionQuerySymbol : QueryBaseSymbol
    {
        public ConditionQuerySymbol(
            ISymbol containingSymbol,
            ConditionNode declaration,
            DiagnosticBag diagnostics)
            : base(containingSymbol, declaration, diagnostics)
        {
            Comparison = declaration.Type switch
            {
                ConditionKind.EqualTo => QueryComparisonType.Equal,
                ConditionKind.LessThan => QueryComparisonType.LessThan,
                ConditionKind.GreaterThan => QueryComparisonType.GreaterThan,
                ConditionKind.NotEqualTo => QueryComparisonType.NotEqual,
                ConditionKind.AtLeast => QueryComparisonType.GreaterThanOrEqual,
                ConditionKind.AtMost => QueryComparisonType.LessThanOrEqual,
                ConditionKind.InstanceOf => QueryComparisonType.InstanceOf,
                ConditionKind.NotInstanceOf => QueryComparisonType.NotInstanceOf,
                _ => QueryComparisonType.None
            };
            if (Comparison is QueryComparisonType.None)
                diagnostics.Add(ErrorCode.ERR_UnknownEnumerationValue, Declaration);
        }

        public override QueryComparisonType Comparison { get; }
    }

    internal class ConstraintQuerySymbol : QueryBaseSymbol
    {
        public ConstraintQuerySymbol(
            ISymbol containingSymbol,
            ConstraintNode declaration,
            DiagnosticBag diagnostics)
            : base(containingSymbol, declaration, diagnostics)
        {
            Comparison = declaration.Type switch
            {
                ConstraintKind.Minimum => QueryComparisonType.GreaterThanOrEqual,
                ConstraintKind.Maximum => QueryComparisonType.LessThanOrEqual,
                _ => QueryComparisonType.None
            };
            if (Comparison is QueryComparisonType.None)
                diagnostics.Add(ErrorCode.ERR_UnknownEnumerationValue, Declaration);
        }

        public override QueryComparisonType Comparison { get; }
    }

    internal class RepeatQuerySymbol : QueryBaseSymbol
    {
        public RepeatQuerySymbol(
            ISymbol containingSymbol,
            QueryBaseNode declaration,
            DiagnosticBag diagnostics)
            : base(containingSymbol, declaration, diagnostics)
        {
        }

        public override QueryComparisonType Comparison => QueryComparisonType.None;
    }
}
