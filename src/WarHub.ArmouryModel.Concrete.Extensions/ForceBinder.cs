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
        ISymbol? qualifier)
    {
        if (options.HasFlag(LookupOptions.CatalogueOnly))
        {
            // no catalogues to bind here
            return;
        }
        if (qualifier is SelectionEntryBaseSymbol sourceEntrySymbol)
        {
            LookupSymbolInQualifyingEntryContainer(sourceEntrySymbol, result, symbolId, options, originalBinder, diagnose, RootClosure);
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
