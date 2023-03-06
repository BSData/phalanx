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

    internal override ContainerEntryBaseSymbol? ContainingContainerSymbol => null;

    internal override EntrySymbol? ContainingEntrySymbol => null;

    internal override ForceSymbol? ContainingForceSymbol => null;

    internal override SelectionSymbol? ContainingSelectionSymbol => null;

    public ImmutableArray<ICatalogueSymbol> RootClosure => lazyRootClosure ??= CalculateRootClosure(Catalogue);

    internal override void LookupSymbolsInSingleBinder(
        LookupResult result,
        string symbolId,
        LookupOptions options,
        Binder originalBinder,
        bool diagnose,
        Symbol? qualifier)
    {
        if (options.HasFlag(LookupOptions.CatalogueOnly))
        {
            // no catalogues to bind here
            return;
        }
        if (qualifier is EntrySymbol entrySymbol)
        {
            LookupSymbolInQualifyingEntry(entrySymbol, result, symbolId, options, originalBinder, diagnose, RootClosure);
            if (result.IsMultiViable)
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
