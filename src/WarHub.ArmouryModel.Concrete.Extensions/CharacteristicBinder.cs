namespace WarHub.ArmouryModel.Concrete;

internal class CharacteristicBinder : Binder
{
    public CharacteristicBinder(Binder next, Symbol containingSymbol, IProfileTypeSymbol profileTypeSymbol) : base(next)
    {
        ContainingSymbol = containingSymbol;
        ProfileTypeSymbol = profileTypeSymbol;
    }

    public IProfileTypeSymbol ProfileTypeSymbol { get; }

    internal override Symbol? ContainingSymbol { get; }

    internal override void LookupSymbolsInSingleBinder(
        LookupResult result,
        string symbolId,
        LookupOptions options,
        Binder originalBinder,
        bool diagnose,
        ISymbol? qualifier)
    {
        if (options.CanConsiderResourceDefinitions())
        {
            originalBinder.CheckViability(result, ProfileTypeSymbol.CharacteristicTypes, symbolId, options, diagnose);
        }
    }
}
