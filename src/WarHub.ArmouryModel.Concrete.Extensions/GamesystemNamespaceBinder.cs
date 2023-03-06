namespace WarHub.ArmouryModel.Concrete;

internal class GamesystemNamespaceBinder : Binder
{
    internal GamesystemNamespaceBinder(Binder next, SourceGlobalNamespaceSymbol namespaceSymbol)
        : base(next)
    {
        NamespaceSymbol = namespaceSymbol;
    }

    internal override Symbol? ContainingSymbol => NamespaceSymbol;

    internal override ContainerEntryBaseSymbol? ContainingContainerSymbol => null;

    internal override EntrySymbol? ContainingEntrySymbol => null;

    internal override ForceSymbol? ContainingForceSymbol => null;

    internal override SelectionSymbol? ContainingSelectionSymbol => null;

    public SourceGlobalNamespaceSymbol NamespaceSymbol { get; }

    internal override void LookupSymbolsInSingleBinder(
        LookupResult result,
        string symbolId,
        LookupOptions options,
        Binder originalBinder,
        bool diagnose,
        Symbol? qualifier)
    {
        if (options.CanConsiderCatalogues())
        {
            originalBinder.CheckViability(result, NamespaceSymbol.Catalogues, symbolId, options, diagnose);
        }
    }
}
