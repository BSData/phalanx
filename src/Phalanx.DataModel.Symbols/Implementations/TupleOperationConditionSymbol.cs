using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public abstract class TupleOperationConditionSymbol : EffectSymbol, ITupleOperationConditionSymbol
    {
        protected TupleOperationConditionSymbol(ICatalogueItemSymbol containingSymbol) : base(containingSymbol)
        {
        }

        public abstract TupleOperation Operation { get; }

        public abstract ImmutableArray<IConditionSymbol> Conditions { get; }
    }
}
