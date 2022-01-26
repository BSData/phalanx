using System.Diagnostics;
using Phalanx.DataModel.Symbols.Implementation;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Binding;

public class Binder
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

    internal virtual IPublicationSymbol BindPublicationSymbol(string? publicationId) =>
        NextRequired.BindPublicationSymbol(publicationId);

    internal virtual IResourceEntrySymbol? BindResourceEntrySymbol(string? targetId, InfoLinkKind type) =>
        NextRequired.BindResourceEntrySymbol(targetId, type);

    internal virtual ICostTypeSymbol? BindCostTypeSymbol(string? typeId) =>
        NextRequired.BindCostTypeSymbol(typeId);

    internal virtual ISelectionEntryContainerSymbol? BindSharedSelectionEntrySymbol(string? targetId, EntryLinkKind type) =>
        NextRequired.BindSharedSelectionEntrySymbol(targetId, type);

    internal virtual ICategoryEntrySymbol? BindCategoryEntrySymbol(string? targetId) =>
        NextRequired.BindCategoryEntrySymbol(targetId);

    internal virtual ISelectionEntrySymbol? BindSelectionEntrySymbol(string? targetId) =>
        NextRequired.BindSelectionEntrySymbol(targetId);

    internal virtual ICatalogueSymbol? BindCatalogueSymbol(string? targetId, CatalogueLinkKind type) =>
        NextRequired.BindCatalogueSymbol(targetId, type);

    internal virtual ICatalogueSymbol? BindGamesystemSymbol(string? gamesystemId) =>
        NextRequired.BindGamesystemSymbol(gamesystemId);

    internal virtual ICharacteristicTypeSymbol? BindCharacteristicTypeSymbol(IProfileTypeSymbol type, CharacteristicNode declaration)
    {
        // TODO should this be done at the higher binder level (e.g. profile type binder?)
        return type.CharacteristicTypes.Where(x => x.Id == declaration.TypeId).SingleOrDefault();
    }

    internal virtual IProfileTypeSymbol? BindProfileTypeSymbol(string? typeId) =>
        NextRequired.BindProfileTypeSymbol(typeId);

    internal IProfileTypeSymbol BindProfileTypeSymbol(ProfileNode node, DiagnosticBag diagnostics)
    {
        // TODO use this when ResultSymbol is ready (returns appropriate Symbol implementation for Error)
        var symbolId = node.TypeId;
        Debug.Assert(symbolId is not null);
        var result = LookupResult.GetInstance();
        var options = LookupOptions.Default;
        LookupSymbolsWithDelayedDiagnosing(result, symbolId, options);
        var bindingResult = ResultSymbol(result, symbolId, node, diagnostics, out _, options);
        result.Free();
        return (IProfileTypeSymbol)bindingResult;
    }

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
                return new ErrorSymbols.ErrorSymbolBase();
            }
        }
        wasError = true;
        if (result.Kind is LookupResultKind.Empty)
        {
            // TODO diagnostics, specific type?
            return new ErrorSymbols.ErrorSymbolBase();
        }

        Debug.Assert(symbols.Count > 0);

        if (result.Error is { } error)
        {
            diagnostics.Add(new WhamDiagnostic(error, where.GetLocation()));
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
