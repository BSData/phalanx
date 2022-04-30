using System.Diagnostics;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class Binder
{
    protected Binder(Compilation compilation)
    {
        Compilation = compilation;
        if (this is not BuckStopsHereBinder)
            throw new InvalidOperationException();
    }

    protected Binder(Binder next)
    {
        Next = next ?? throw new ArgumentNullException(nameof(next));
        Compilation = next.Compilation;
    }

    internal Compilation Compilation { get; }

    protected Binder? Next { get; }

    protected Binder NextRequired => Next ?? throw new InvalidOperationException("Must have Next!");

    internal virtual Symbol? ContainingSymbol => NextRequired.ContainingSymbol;

    internal virtual ContainerEntryBaseSymbol? ContainingContainerSymbol =>
        ContainingSymbol as ContainerEntryBaseSymbol ?? NextRequired.ContainingContainerSymbol;

    internal virtual EntrySymbol? ContainingEntrySymbol =>
        ContainingSymbol as EntrySymbol ?? NextRequired.ContainingEntrySymbol;

    internal IProfileTypeSymbol BindProfileTypeSymbol(ProfileNode node, DiagnosticBag diagnostics) =>
        BindSimple<IProfileTypeSymbol, ErrorSymbols.ErrorProfileTypeSymbol>(
            node, diagnostics, node.TypeId, LookupOptions.ProfileTypeOnly);

    internal IPublicationSymbol BindPublicationSymbol(EntryBaseNode node, DiagnosticBag diagnostics) =>
        BindSimple<IPublicationSymbol, ErrorSymbols.ErrorPublicationSymbol>(
            node, diagnostics, node.PublicationId, LookupOptions.PublicationOnly);

    internal ICharacteristicTypeSymbol BindCharacteristicTypeSymbol(CharacteristicNode node, DiagnosticBag diagnostics) =>
        BindSimple<ICharacteristicTypeSymbol, ErrorSymbols.ErrorCharacteristicTypeSymbol>(
            node, diagnostics, node.TypeId, LookupOptions.CharacteristicTypeOnly);

    internal ICatalogueSymbol BindCatalogueSymbol(CatalogueLinkNode node, DiagnosticBag diagnostics) =>
        BindSimple<ICatalogueSymbol, ErrorSymbols.ErrorCatalogueSymbol>(
            node, diagnostics, node.TargetId, LookupOptions.CatalogueOnly);

    internal ICatalogueSymbol BindGamesystemSymbol(CatalogueNode node, DiagnosticBag diagnostics) =>
        BindSimple<ICatalogueSymbol, ErrorSymbols.ErrorGamesystemSymbol>(
            node, diagnostics, node.GamesystemId, LookupOptions.CatalogueOnly);

    internal ICatalogueSymbol BindGamesystemSymbol(RosterNode node, DiagnosticBag diagnostics) =>
        BindSimple<ICatalogueSymbol, ErrorSymbols.ErrorGamesystemSymbol>(
            node, diagnostics, node.GameSystemId, LookupOptions.CatalogueOnly);

    internal ICostTypeSymbol BindCostTypeSymbol(CostNode node, DiagnosticBag diagnostics) =>
        BindSimple<ICostTypeSymbol, ErrorSymbols.ErrorCostTypeSymbol>(
            node, diagnostics, node.TypeId, LookupOptions.CostTypeOnly);

    internal IResourceEntrySymbol BindSharedResourceEntrySymbol(InfoLinkNode node, DiagnosticBag diagnostics) =>
        BindSimple<IResourceEntrySymbol, ErrorSymbols.ErrorResourceEntrySymbol>(
            node,
            diagnostics, node.TargetId,
            LookupOptions.ResourceEntryOnly | LookupOptions.SharedOnly | node.Type switch
            {
                InfoLinkKind.InfoGroup => LookupOptions.ResourceGroupEntryOnly,
                InfoLinkKind.Profile => LookupOptions.ProfileEntryOnly,
                InfoLinkKind.Rule => LookupOptions.RuleEntryOnly,
                _ => LookupOptions.Default,
            });

    internal ISelectionEntryContainerSymbol BindSharedSelectionEntrySymbol(EntryLinkNode node, DiagnosticBag diagnostics) =>
        BindSimple<ISelectionEntryContainerSymbol, ErrorSymbols.ErrorSelectionEntryContainerSymbol>(
            node,
            diagnostics, node.TargetId,
            LookupOptions.ContainerEntryOnly | LookupOptions.SharedOnly | node.Type switch
            {
                EntryLinkKind.SelectionEntry => LookupOptions.SelectionEntryOnly,
                EntryLinkKind.SelectionEntryGroup => LookupOptions.SelectionGroupEntryOnly,
                _ => LookupOptions.Default,
            });

    internal ICategoryEntrySymbol BindCategoryEntrySymbol(CategoryLinkNode node, DiagnosticBag diagnostics) =>
        BindSimple<ICategoryEntrySymbol, ErrorSymbols.ErrorCategoryEntrySymbol>(
            node, diagnostics, node.TargetId, LookupOptions.CategoryEntryOnly);

    // TODO this definitely needs work, as it looks downward, into children
    internal ISelectionEntrySymbol BindSelectionEntryGroupDefaultEntrySymbol(SelectionEntryGroupNode node, DiagnosticBag diagnostics) =>
        BindSimple<ISelectionEntrySymbol, ErrorSymbols.ErrorSelectionEntrySymbol>(
            node, diagnostics, node.DefaultSelectionEntryId, LookupOptions.SelectionEntryOnly);

    private TSymbol BindSimple<TSymbol, TErrorSymbol>(SourceNode node, DiagnosticBag diagnostics, string? symbolId, LookupOptions options)
        where TSymbol : ISymbol
        where TErrorSymbol : ErrorSymbols.ErrorSymbolBase, TSymbol, new()
    {
        Debug.Assert(symbolId is not null);
        var result = LookupResult.GetInstance();
        LookupSymbolsWithDelayedDiagnosing(result, symbolId, options);
        var bindingResult = ResultSymbol(result, symbolId, node, diagnostics, out _, options);
        result.Free();
        if (bindingResult is ErrorSymbols.ErrorSymbolBase error and not TSymbol)
        {
            return new TErrorSymbol().WithErrorDetailsFrom(error);
        }
        return (TSymbol)bindingResult;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Method is WIP")]
    internal SingleLookupResult CheckViability(
        ISymbol symbol,
        string symbolId,
        LookupOptions options,
        bool diagnose)
    {
        if (symbol.Id != symbolId)
        {
            return LookupResult.Empty();
        }
        if (options.HasFlag(LookupOptions.RootOnly) && !IsRootEntry(symbol))
        {
            return LookupResult.Empty();
        }
        if (options.HasFlag(LookupOptions.SharedOnly) && !IsSharedEntry(symbol))
        {
            return LookupResult.Empty();
        }
        if (options.HasFlag(LookupOptions.CatalogueOnly) && symbol.Kind != SymbolKind.Catalogue)
        {
            return LookupResult.Empty();
        }
        if (options.HasFlag(LookupOptions.ResoureDefinitionOnly))
        {
            if (symbol.Kind != SymbolKind.ResourceDefinition || symbol is not IResourceDefinitionSymbol rdSymbol)
            {
                return LookupResult.Empty();
            }
            var resourceKind = rdSymbol.ResourceKind;
            if (options.HasFlag(LookupOptions.PublicationOnly) && resourceKind != ResourceKind.Publication)
            {
                return LookupResult.Empty();
            }
            if (options.HasFlag(LookupOptions.CostTypeOnly) && resourceKind != ResourceKind.Cost)
            {
                return LookupResult.Empty();
            }
            if (options.HasFlag(LookupOptions.ProfileTypeOnly) && resourceKind != ResourceKind.Profile)
            {
                return LookupResult.Empty();
            }
            if (options.HasFlag(LookupOptions.CharacteristicTypeOnly) && resourceKind != ResourceKind.Characteristic)
            {
                return LookupResult.Empty();
            }
        }
        if (options.HasFlag(LookupOptions.ResourceEntryOnly) && symbol.Kind != SymbolKind.Resource)
        {
            return LookupResult.Empty();
        }
        if (options.HasFlag(LookupOptions.ContainerEntryOnly) && symbol.Kind != SymbolKind.ContainerEntry)
        {
            return LookupResult.Empty();
        }
        return LookupResult.Good(symbol);
    }

    private static bool IsRootEntry(ISymbol symbol) => symbol.ContainingCatalogue is { } catalogue && symbol.Kind switch
    {
        SymbolKind.ContainerEntry => catalogue.RootContainerEntries.Contains(symbol),
        SymbolKind.Resource => catalogue.RootResourceEntries.Contains(symbol),
        _ => false,
    };

    private static bool IsSharedEntry(ISymbol symbol) => symbol.ContainingCatalogue is { } catalogue && symbol.Kind switch
    {
        SymbolKind.ContainerEntry => catalogue.SharedSelectionEntryContainers.Contains(symbol),
        SymbolKind.Resource => catalogue.SharedResourceEntries.Contains(symbol),
        _ => false,
    };

    internal void CheckViability<TSymbol>(
        LookupResult result,
        ImmutableArray<TSymbol> symbols,
        string symbolId,
        LookupOptions options,
        bool diagnose)
        where TSymbol : ISymbol
    {
        foreach (var symbol in symbols)
        {
            result.MergeEqual(CheckViability(symbol, symbolId, options, diagnose));
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Method is WIP")]
    internal ISymbol ResultSymbol(
        LookupResult result,
        string symbolId,
        SourceNode where,
        DiagnosticBag diagnostics,
        out bool wasError,
        LookupOptions options = LookupOptions.Default)
    {
        Debug.Assert(where is not null);
        Debug.Assert(diagnostics is not null);
        var symbols = result.Symbols;
        wasError = false;
        if (result.IsMultiViable)
        {
            if (result.SingleSymbolOrDefault is { } singleSymbol)
            {
                return singleSymbol;
            }
            else
            {
                // TODO sort by "bestness"?
                // TODO multi-result analysis
                // TODO report possible warnings or errors
                // TODO candidate-containing result
                diagnostics.Add(ErrorCode.ERR_MultipleViableBindingCandidates, where.GetLocation(), result.Symbols.ToImmutableArray(), symbolId);
                return new ErrorSymbols.ErrorSymbolBase();
            }
        }
        wasError = true;
        if (result.Kind is LookupResultKind.Empty)
        {
            // TODO diagnostics, specific type?
            diagnostics.Add(ErrorCode.ERR_NoBindingCandidates, where.GetLocation(), symbolId);
            return new ErrorSymbols.ErrorSymbolBase();
        }

        Debug.Assert(symbols.Count > 0);

        if (result.Error is { } error)
        {
            diagnostics.Add(new WhamDiagnostic(error, where.GetLocation()));
        }
        else
        {
            diagnostics.Add(ErrorCode.ERR_UnviableBindingCandidates, where.GetLocation(), result.Symbols.ToImmutableArray(), symbolId);
        }

        if (result.SingleSymbolOrDefault is { } singleUnviable)
        {
            // single non-viable, error already reported - just return
            return singleUnviable;
        }
        else
        {
            // multiple - package up
            // TODO candidate-containing result
            return new ErrorSymbols.ErrorSymbolBase();
        }
    }

    internal virtual void LookupSymbolsInSingleBinder(
        LookupResult result,
        string symbolId,
        LookupOptions options,
        Binder originalBinder,
        bool diagnose)
    {
    }

    private Binder? LookupSymbolsWithDelayedDiagnosing(
        LookupResult result,
        string symbolId,
        LookupOptions options = LookupOptions.Default)
    {
        var binder = LookupSymbolsWithScopeTraversal(result, symbolId, options, diagnose: false);
        Debug.Assert(binder is not null || result.IsClear);

        if (result.Kind is not LookupResultKind.Empty and not LookupResultKind.Viable)
        {
            result.Clear();
            // retry to get diagnosis
            var otherBinder = LookupSymbolsWithScopeTraversal(result, symbolId, options, diagnose: true);
            Debug.Assert(binder == otherBinder);
        }
        Debug.Assert(result.IsMultiViable || result.IsClear || result.Error is not null);
        return binder;
    }

    private Binder? LookupSymbolsWithScopeTraversal(
        LookupResult result,
        string symbolId,
        LookupOptions options,
        bool diagnose)
    {
        Debug.Assert(result.IsClear);
        Binder? binder = null;
        // traverse scopes from this to the next and next
        // until there's no next, or a result is viable.
        for (var scope = this; scope != null && !result.IsMultiViable; scope = scope.Next)
        {
            if (binder is null)
            {
                // we didn't yet get any results
                scope.LookupSymbolsInSingleBinder(result, symbolId, options, this, diagnose);
                if (!result.IsClear)
                {
                    // first binder to set non-empty-result, save it to return
                    binder = scope;
                }
            }
            else
            {
                // we already set some non-empty result, so we create a temporary to merge with it
                var tmpResult = LookupResult.GetInstance();
                scope.LookupSymbolsInSingleBinder(tmpResult, symbolId, options, this, diagnose);
                result.MergeEqual(tmpResult);
                tmpResult.Free();
            }
        }
        return binder;
    }
}
