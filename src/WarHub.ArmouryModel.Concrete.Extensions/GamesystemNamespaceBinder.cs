namespace WarHub.ArmouryModel.Concrete;

internal class GamesystemNamespaceBinder : Binder
{
    internal GamesystemNamespaceBinder(Binder next, SourceGlobalNamespaceSymbol namespaceSymbol)
        : base(next)
    {
        NamespaceSymbol = namespaceSymbol;
    }

    internal override Symbol? ContainingSymbol => NamespaceSymbol;

    public SourceGlobalNamespaceSymbol NamespaceSymbol { get; }

    internal override void LookupSymbolsInSingleBinder(
        LookupResult result,
        string symbolId,
        LookupOptions options,
        Binder originalBinder,
        bool diagnose,
        ISymbol? qualifier)
    {
        if (options.CanConsiderCatalogues())
        {
            originalBinder.CheckViability(result, NamespaceSymbol.Catalogues, symbolId, options, diagnose);
        }
    }
}
