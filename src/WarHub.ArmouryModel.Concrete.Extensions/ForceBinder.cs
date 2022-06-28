namespace WarHub.ArmouryModel.Concrete;

internal class ForceBinder : Binder
{
    public ForceSymbol ForceSymbol { get; }

    internal ForceBinder(Binder next, ForceSymbol forceSymbol) : base(next)
    {
        ForceSymbol = forceSymbol;
    }

    internal override Symbol? ContainingSymbol => ForceSymbol;
}
