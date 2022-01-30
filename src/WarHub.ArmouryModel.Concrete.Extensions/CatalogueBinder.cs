namespace WarHub.ArmouryModel.Concrete;

internal class CatalogueBaseBinder : Binder
{
    public CatalogueBaseSymbol Catalogue { get; }

    private ImmutableArray<ICatalogueSymbol> lazyRootClosure;

    internal CatalogueBaseBinder(Binder next, CatalogueBaseSymbol catalogue) : base(next)
    {
        Catalogue = catalogue;
    }

    internal override Symbol? ContainingSymbol => Catalogue;

    public ImmutableArray<ICatalogueSymbol> RootClosure =>
        lazyRootClosure.IsDefault ? (lazyRootClosure = CalculateRootClosure()) : lazyRootClosure;

    internal ImmutableArray<ICatalogueSymbol> CalculateRootClosure()
    {
        var processed = new HashSet<ICatalogueSymbol>();
        var closureItems = new List<ICatalogueSymbol>
        {
            // this is required, it's the base catalogue
            Catalogue,
        };
        var queuedForProcessing = new Queue<ICatalogueSymbol>();
        queuedForProcessing.Enqueue(Catalogue);
        while (queuedForProcessing.TryDequeue(out var item))
        {
            if (processed.Add(item))
            {
                closureItems.Add(item);
                foreach (var import in item.Imports)
                {
                    queuedForProcessing.Enqueue(import.Catalogue);
                }
            }
        }
        closureItems.Add(Catalogue.Gamesystem);
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
        if (!options.HasFlag(LookupOptions.SharedEntryOnly))
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
        if (!options.HasFlag(LookupOptions.RootEntryOnly))
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
