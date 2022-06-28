namespace WarHub.ArmouryModel.Concrete;

internal class CatalogueBaseBinder : Binder
{
    public CatalogueBaseSymbol Catalogue { get; }

    private ImmutableArray<ICatalogueSymbol>? lazyRootClosure;

    internal CatalogueBaseBinder(Binder next, CatalogueBaseSymbol catalogue) : base(next)
    {
        Catalogue = catalogue;
    }

    internal override Symbol? ContainingSymbol => Catalogue;

    public ImmutableArray<ICatalogueSymbol> RootClosure => lazyRootClosure ??= CalculateRootClosure(Catalogue);

    internal static ImmutableArray<ICatalogueSymbol> CalculateRootClosure(ICatalogueSymbol catalogue)
    {
        var closureItems = new List<ICatalogueSymbol>();
        var queuedForProcessing = new Queue<ICatalogueSymbol>();
        queuedForProcessing.Enqueue(catalogue);
        while (queuedForProcessing.TryDequeue(out var item) && !closureItems.Contains(item))
        {
            closureItems.Add(item);
            foreach (var import in item.CatalogueReferences)
            {
                queuedForProcessing.Enqueue(import.Catalogue);
            }
        }
        closureItems.Add(catalogue.Gamesystem);
        // TODO consider filtering out "missing"/error items
        return closureItems.ToImmutableArray();
    }

    internal override void LookupSymbolsInSingleBinder(LookupResult result, string symbolId, LookupOptions options, Binder originalBinder, bool diagnose)
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

    private static void LookupSymbolsInSingleCatalogue(
        ICatalogueSymbol catalogue,
        LookupResult result,
        string symbolId,
        LookupOptions options,
        Binder originalBinder,
        bool diagnose)
    {
        if (options.CanConsiderResourceDefinitions())
        {
            originalBinder.CheckViability(result, catalogue.ResourceDefinitions, symbolId, options, diagnose);
        }
        if (!options.HasFlag(LookupOptions.SharedOnly))
        {
            if (options.CanConsiderContainerEntries())
            {
                originalBinder.CheckViability(result, catalogue.RootContainerEntries, symbolId, options, diagnose);
            }
            if (options.CanConsiderResourceEntries())
            {
                originalBinder.CheckViability(result, catalogue.RootResourceEntries, symbolId, options, diagnose);
            }
        }
        if (!options.HasFlag(LookupOptions.RootOnly))
        {
            if (options.CanConsiderContainerEntries())
            {
                originalBinder.CheckViability(result, catalogue.SharedSelectionEntryContainers, symbolId, options, diagnose);
            }
            if (options.CanConsiderResourceEntries())
            {
                originalBinder.CheckViability(result, catalogue.SharedResourceEntries, symbolId, options, diagnose);
            }
        }
    }
}
