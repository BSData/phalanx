using Phalanx.DataModel.Symbols.Implementation;

namespace Phalanx.DataModel.Symbols.Binding;

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
        var closureItems = new List<ICatalogueSymbol>();
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
        // TODO consider filtering out "missing" items
        return closureItems.ToImmutableArray();
    }

    internal override void LookupSymbolsInSingleBinder(LookupResult result, string symbolId, LookupOptions options, Binder originalBinder, bool diagnose)
    {
        if (options.CanConsiderResourceDefinitions())
        {
            originalBinder.CheckViability(result, Catalogue.ResourceDefinitions, symbolId, options, diagnose);
        }
        if (!options.HasFlag(LookupOptions.SharedEntryOnly))
        {
            if (options.CanConsiderContainerEntries())
            {
                originalBinder.CheckViability(result, Catalogue.RootContainerEntries, symbolId, options, diagnose);
            }
            if (options.CanConsiderResourceEntries())
            {
                originalBinder.CheckViability(result, Catalogue.RootResourceEntries, symbolId, options, diagnose);
            }
        }
        if (!options.HasFlag(LookupOptions.RootEntryOnly))
        {
            if (options.CanConsiderContainerEntries())
            {
                originalBinder.CheckViability(result, Catalogue.SharedSelectionEntryContainers, symbolId, options, diagnose);
            }
            if (options.CanConsiderResourceEntries())
            {
                originalBinder.CheckViability(result, Catalogue.SharedResourceEntries, symbolId, options, diagnose);
            }
        }
        // TODO visit closure? or separate into binder?
    }
}
