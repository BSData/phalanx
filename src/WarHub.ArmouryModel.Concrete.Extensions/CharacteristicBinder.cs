namespace WarHub.ArmouryModel.Concrete;

internal class CharacteristicBinder : Binder
{
    private readonly ProfileSymbol profileSymbol;

    public CharacteristicBinder(Binder next, ProfileSymbol profileSymbol) : base(next)
    {
        this.profileSymbol = profileSymbol;
    }

    internal override Symbol? ContainingSymbol => profileSymbol;

    internal override void LookupSymbolsInSingleBinder(LookupResult result, string symbolId, LookupOptions options, Binder originalBinder, bool diagnose)
    {
        if (options.CanConsiderResourceDefinitions())
        {
            originalBinder.CheckViability(result, profileSymbol.Type.CharacteristicTypes, symbolId, options, diagnose);
        }
    }
}
