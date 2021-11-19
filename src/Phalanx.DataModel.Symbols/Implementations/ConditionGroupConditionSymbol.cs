using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class ConditionGroupConditionSymbol : TupleOperationConditionSymbol
{
    public ConditionGroupConditionSymbol(
        ICatalogueItemSymbol containingSymbol,
        ConditionGroupNode declaration,
        BindingDiagnosticContext diagnostics)
        : base(containingSymbol)
    {
        Operation = declaration.Type switch
        {
            ConditionGroupKind.And => TupleOperation.And,
            ConditionGroupKind.Or => TupleOperation.Or,
            _ => TupleOperation.Unknown
        };
        if (Operation is TupleOperation.Unknown)
        {
            diagnostics.Add("Unknown condition group type");
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
