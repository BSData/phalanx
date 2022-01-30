namespace WarHub.ArmouryModel.Concrete;

internal abstract class TupleOperationConditionSymbol : EffectSymbol, ITupleOperationConditionSymbol
{
    protected TupleOperationConditionSymbol(ISymbol containingSymbol)
        : base(containingSymbol)
    {
    }

    public abstract TupleOperation Operation { get; }

    public abstract ImmutableArray<IConditionSymbol> Conditions { get; }
}
