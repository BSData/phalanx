namespace Phalanx.DataModel.Symbols.Implementation
{
    public abstract class EffectSymbol : LogicSymbol, IEffectSymbol
    {
        public EffectSymbol(ICatalogueItemSymbol containingSymbol) : base(containingSymbol)
        {
        }
    }
}
