namespace WarHub.ArmouryModel.Concrete;

internal class CatalogueBaseBinder : Binder
{
    private ImmutableArray<ICatalogueSymbol>? lazyRootClosure;

    internal CatalogueBaseBinder(Binder next, CatalogueBaseSymbol catalogue) : base(next)
    {
        Catalogue = catalogue;
    }

    public CatalogueBaseSymbol Catalogue { get; }

    internal override Symbol? ContainingSymbol => Catalogue;

    public ImmutableArray<ICatalogueSymbol> RootClosure => lazyRootClosure ??= CalculateRootClosure(Catalogue);

    internal override void LookupSymbolsInSingleBinder(
        LookupResult result,
        string symbolId,
        LookupOptions options,
        Binder originalBinder,
        bool diagnose,
        ISymbol? qualifier)
    {
        if (options.HasFlag(LookupOptions.CatalogueOnly))
        {
            // no catalogues to bind here
            return;
        }
        foreach (var catalogue in RootClosure)
        {
            LookupSymbolsInSingleCatalogue(catalogue, result, symbolId, options, originalBinder, diagnose);
            if (result.IsMultiViable)
                return;
        }
    }
}
