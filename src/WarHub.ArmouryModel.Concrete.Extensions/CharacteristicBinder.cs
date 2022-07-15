namespace WarHub.ArmouryModel.Concrete;

internal class CharacteristicBinder : Binder
{
    public CharacteristicBinder(Binder next, Symbol containingSymbol, IResourceDefinitionSymbol profileTypeSymbol) : base(next)
    {
        ContainingSymbol = containingSymbol;
        ProfileTypeSymbol = profileTypeSymbol;
    }

    public IResourceDefinitionSymbol ProfileTypeSymbol { get; }

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
            originalBinder.CheckViability(result, ProfileTypeSymbol.Definitions, symbolId, options, diagnose);
        }
    }
}
