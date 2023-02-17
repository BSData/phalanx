using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class ConditionGroupingBaseSymbol : ConditionBaseSymbol, IConditionSymbol
{
    protected ConditionGroupingBaseSymbol(
        ISymbol containingSymbol,
        SourceNode declaration) : base(containingSymbol, declaration)
    {
    }

    public sealed override QueryBaseSymbol? Query => null;

    public static ConditionGroupingBaseSymbol Create(
        ISymbol containingSymbol,
        SourceNode declaration,
        DiagnosticBag diagnostics) => declaration switch
        {
            ConditionGroupNode x => new ConditionGroupSymbol(containingSymbol, x, diagnostics),
            ModifierBaseNode x => new ModifierConditionGroupSymbol(containingSymbol, x, diagnostics),
            _ => throw new InvalidOperationException("Unsupported declaration type.")
        };

    internal class ConditionGroupSymbol : ConditionGroupingBaseSymbol, INodeDeclaredSymbol<ConditionGroupNode>
    {
        public ConditionGroupSymbol(
            ISymbol containingSymbol,
            ConditionGroupNode declaration,
            DiagnosticBag diagnostics)
            : base(containingSymbol, declaration)
        {
            Declaration = declaration;
            ChildrenOperator = declaration.Type switch
            {
                ConditionGroupKind.And => LogicalOperator.Conjunction,
                ConditionGroupKind.Or => LogicalOperator.Disjunction,
                _ => LogicalOperator.Unknown,
            };
            if (ChildrenOperator is LogicalOperator.Unknown)
            {
                diagnostics.Add(
                    ErrorCode.ERR_UnknownEnumerationValue,
                    declaration.GetLocation(),
                    declaration.Type);
            }
            Children = GetChildSymbols().ToImmutableArray();

            IEnumerable<ConditionBaseSymbol> GetChildSymbols()
            {
                foreach (var node in declaration.Conditions)
                {
                    yield return new ConditionSymbol(this, node, diagnostics);
                }
                foreach (var node in declaration.ConditionGroups)
                {
                    yield return new ConditionGroupSymbol(this, node, diagnostics);
                }
            }
        }

        public new ConditionGroupNode Declaration { get; }

        public override LogicalOperator ChildrenOperator { get; }

        public override ImmutableArray<ConditionBaseSymbol> Children { get; }
    }

    internal class ModifierConditionGroupSymbol : ConditionGroupingBaseSymbol, INodeDeclaredSymbol<ModifierBaseNode>
    {
        public ModifierConditionGroupSymbol(
            ISymbol containingSymbol,
            ModifierBaseNode declaration,
            DiagnosticBag diagnostics)
            : base(containingSymbol, declaration)
        {
            Declaration = declaration;

            Children = GetChildSymbols().ToImmutableArray();

            IEnumerable<ConditionBaseSymbol> GetChildSymbols()
            {
                foreach (var node in declaration.Conditions)
                {
                    yield return new ConditionSymbol(this, node, diagnostics);
                }
                foreach (var node in declaration.ConditionGroups)
                {
                    yield return new ConditionGroupSymbol(this, node, diagnostics);
                }
            }
        }

        public new ModifierBaseNode Declaration { get; }

        // BS_SPEC: All conditions and groups at the modifier's root combine as an AND operator.
        // BS_SPEC: modifier group creates a scope: all modifiers (and sub groups)
        // have their own conditions combined via AND with all conditions and condition groups
        // declared at that modifier's level. We achieve the same via IConditionalEffectSymbol:
        // any nested effects (sub modifiers and sub modifier groups) only execute if top condition
        // is satisfied, emulating the AND behavior.
        public override LogicalOperator ChildrenOperator => LogicalOperator.Conjunction;

        public override ImmutableArray<ConditionBaseSymbol> Children { get; }
    }
}
