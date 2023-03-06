namespace WarHub.ArmouryModel.Concrete;

internal class ForceBinder : Binder
{
    private ImmutableArray<ICatalogueSymbol>? lazyRootClosure;

    internal ForceBinder(Binder next, ForceSymbol force) : base(next)
    {
        Force = force;
    }

    public ForceSymbol Force { get; }

    internal override ForceSymbol? ContainingForceSymbol => Force;

    internal override SelectionSymbol? ContainingSelectionSymbol => null;

    internal override ContainerEntryBaseSymbol? ContainingContainerSymbol => null;

    internal override EntrySymbol? ContainingEntrySymbol => null;

    public ImmutableArray<ICatalogueSymbol> RootClosure =>
        lazyRootClosure ??= CalculateRootClosure(Force.CatalogueReference.Catalogue);

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
        if (options.HasFlag(LookupOptions.CategoryEntryOnly) && symbolId == Compilation.NoCategorySymbolId)
        {
            result.MergeEqual(LookupResult.Good(Compilation.NoCategoryEntrySymbol));
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
