namespace Phalanx.DataModel.Symbols.Implementation;

internal abstract class EffectSymbol : LogicSymbol, IEffectSymbol
{
    public EffectSymbol(ISymbol containingSymbol) : base(containingSymbol)
    {
    }
}
