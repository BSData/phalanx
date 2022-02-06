namespace WarHub.ArmouryModel.Concrete;

internal abstract class EffectSymbol : LogicSymbol, IEffectSymbol
{
    public EffectSymbol(ISymbol containingSymbol) : base(containingSymbol)
    {
    }
}
