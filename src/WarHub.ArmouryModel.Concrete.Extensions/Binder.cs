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

    internal virtual ForceSymbol? ContainingForceSymbol =>
        ContainingSymbol as ForceSymbol ?? NextRequired.ContainingForceSymbol;

    internal virtual SelectionSymbol? ContainingSelectionSymbol =>
        ContainingSymbol as SelectionSymbol ?? NextRequired.ContainingSelectionSymbol;

    internal IResourceDefinitionSymbol BindProfileTypeSymbol(ProfileNode node, BindingDiagnosticBag diagnostics) =>
        BindSimple<IResourceDefinitionSymbol, ErrorSymbols.ErrorResourceDefinitionSymbol>(
            node, diagnostics, node.TypeId, LookupOptions.ProfileTypeOnly);

    internal IPublicationSymbol BindPublicationSymbol(IPublicationReferencingNode node, BindingDiagnosticBag diagnostics) =>
        BindSimple<IPublicationSymbol, ErrorSymbols.ErrorPublicationSymbol>(
            (SourceNode)node, diagnostics, node.PublicationId, LookupOptions.PublicationOnly);

    internal IResourceDefinitionSymbol BindCharacteristicTypeSymbol(CharacteristicNode node, BindingDiagnosticBag diagnostics) =>
        BindSimple<IResourceDefinitionSymbol, ErrorSymbols.ErrorResourceDefinitionSymbol>(
            node, diagnostics, node.TypeId, LookupOptions.CharacteristicTypeOnly);

    internal IForceEntrySymbol BindForceEntrySymbol(ForceNode node, BindingDiagnosticBag diagnostics) =>
        BindSimple<IForceEntrySymbol, ErrorSymbols.ErrorForceEntrySymbol>(
            node, diagnostics, node.EntryId, LookupOptions.ForceEntryOnly);

    internal ICatalogueSymbol BindCatalogueSymbol(CatalogueLinkNode node, BindingDiagnosticBag diagnostics) =>
        BindSimple<ICatalogueSymbol, ErrorSymbols.ErrorCatalogueSymbol>(
            node, diagnostics, node.TargetId, LookupOptions.CatalogueOnly);

    internal ICatalogueSymbol BindCatalogueSymbol(ForceNode node, BindingDiagnosticBag diagnostics) =>
        BindSimple<ICatalogueSymbol, ErrorSymbols.ErrorCatalogueSymbol>(
            node, diagnostics, node.CatalogueId, LookupOptions.CatalogueOnly);

    internal ICatalogueSymbol BindCatalogueSymbol(SourceNode node, string? symboldId, BindingDiagnosticBag diagnostics) =>
        BindSimple<ICatalogueSymbol, ErrorSymbols.ErrorCatalogueSymbol>(
            node, diagnostics, symboldId, LookupOptions.CatalogueOnly);

    internal ICatalogueSymbol BindGamesystemSymbol(CatalogueNode node, BindingDiagnosticBag diagnostics) =>
        BindSimple<ICatalogueSymbol, ErrorSymbols.ErrorGamesystemSymbol>(
            node, diagnostics, node.GamesystemId, LookupOptions.CatalogueOnly);

    internal ICatalogueSymbol BindGamesystemSymbol(RosterNode node, BindingDiagnosticBag diagnostics) =>
        BindSimple<ICatalogueSymbol, ErrorSymbols.ErrorGamesystemSymbol>(
            node, diagnostics, node.GameSystemId, LookupOptions.CatalogueOnly);

    internal IResourceDefinitionSymbol BindCostTypeSymbol(CostNode node, BindingDiagnosticBag diagnostics) =>
        BindSimple<IResourceDefinitionSymbol, ErrorSymbols.ErrorResourceDefinitionSymbol>(
            node, diagnostics, node.TypeId, LookupOptions.CostTypeOnly);

    internal IResourceDefinitionSymbol BindCostTypeSymbol(SourceNode node, string? symbolId, BindingDiagnosticBag diagnostics) =>
        BindSimple<IResourceDefinitionSymbol, ErrorSymbols.ErrorResourceDefinitionSymbol>(
            node, diagnostics, symbolId, LookupOptions.CostTypeOnly);

    internal IResourceEntrySymbol BindSharedResourceEntrySymbol(InfoLinkNode node, BindingDiagnosticBag diagnostics) =>
        BindSimple<IResourceEntrySymbol, ErrorSymbols.ErrorResourceEntrySymbol>(
            node,
            diagnostics, node.TargetId,
            LookupOptions.ResourceEntryOnly | CompilationOptionReferencedEntry() | node.Type switch
            {
                InfoLinkKind.InfoGroup => LookupOptions.ResourceGroupEntryOnly,
                InfoLinkKind.Profile => LookupOptions.ProfileEntryOnly,
                InfoLinkKind.Rule => LookupOptions.RuleEntryOnly,
                _ => LookupOptions.Default,
            });

    internal ISelectionEntryContainerSymbol BindSelectionEntrySymbol(EntryLinkNode node, BindingDiagnosticBag diagnostics) =>
        BindSimple<ISelectionEntryContainerSymbol, ErrorSymbols.ErrorSelectionEntryContainerSymbol>(
            node,
            diagnostics, node.TargetId,
            LookupOptions.ContainerEntryOnly | CompilationOptionReferencedEntry() | node.Type switch
            {
                EntryLinkKind.SelectionEntry => LookupOptions.SelectionEntryOnly,
                EntryLinkKind.SelectionEntryGroup => LookupOptions.SelectionGroupEntryOnly,
                _ => LookupOptions.Default,
            });

    internal LookupOptions CompilationOptionReferencedEntry() =>
        Compilation.Options.BindEntryReferencesToNestedEntries ? LookupOptions.Default : LookupOptions.SharedOnly;

    internal ICategoryEntrySymbol BindCategoryEntrySymbol(CategoryLinkNode node, BindingDiagnosticBag diagnostics) =>
        BindCategoryEntrySymbol(node, node.TargetId, diagnostics);

    internal ICategoryEntrySymbol BindCategoryEntrySymbol(CategoryNode node, BindingDiagnosticBag diagnostics) =>
        BindCategoryEntrySymbol(node, node.EntryId, diagnostics);

    internal ICategoryEntrySymbol BindCategoryEntrySymbol(SourceNode node, string? symbolId, BindingDiagnosticBag diagnostics) =>
        BindSimple<ICategoryEntrySymbol, ErrorSymbols.ErrorCategoryEntrySymbol>(
            node, diagnostics, symbolId, LookupOptions.CategoryEntryOnly | LookupOptions.RootOnly);

    internal ISymbol BindEffectTargetMemberSymbol(SourceNode node, string? symbolId, BindingDiagnosticBag diagnostics)
    {
        Debug.Assert(ContainingEntrySymbol is not null);
        var firstPassDiags = BindingDiagnosticBag.GetInstance();
        const LookupOptions Options = LookupOptions.SingleLevel | LookupOptions.ResourceByDefinitionId
            | LookupOptions.EntryMembersOnly | LookupOptions.LookupInReferencedEntryMembers;
        var initialResult = BindSimple<ISymbol, ErrorSymbols.ErrorSymbolBase>(
            node, firstPassDiags, symbolId, Options, ContainingEntrySymbol);
        if (!initialResult.IsKind(SymbolKind.Error))
        {
            diagnostics.AddRangeAndFree(firstPassDiags);
            return initialResult;
        }
        var costTypeDiags = BindingDiagnosticBag.GetInstance();
        var costType = BindCostTypeSymbol(node, symbolId, costTypeDiags);
        if (costType.IsKind(SymbolKind.Error))
        {
            diagnostics.AddRangeAndFree(firstPassDiags);
            return initialResult;
        }
        return new GeneratedCostSymbol(ContainingEntrySymbol, costType);
    }

    internal ISymbol BindFilterEntrySymbol(SourceNode node, string? symbolId, QueryScopeKind scopeKind, BindingDiagnosticBag diagnostics)
        => scopeKind switch
        {
            QueryScopeKind.PrimaryCatalogue => BindCatalogueSymbol(node, symbolId, diagnostics),
            QueryScopeKind.PrimaryCategory => BindCategoryEntrySymbol(node, symbolId, diagnostics),
            _ => BindSimple<IContainerEntrySymbol, ErrorSymbols.ErrorContainerEntrySymbol>(node, diagnostics, symbolId, LookupOptions.ContainerEntryOnly)
        };

    internal ISymbol BindScopeEntrySymbol(SourceNode node, string? symbolId, BindingDiagnosticBag diagnostics) =>
        BindSimple<IContainerEntrySymbol, ErrorSymbols.ErrorContainerEntrySymbol>(node, diagnostics, symbolId, LookupOptions.ContainerEntryOnly);

    internal ISelectionEntryContainerSymbol BindSelectionEntryGroupDefaultEntrySymbol(
        SelectionEntryGroupNode node,
        Symbol entryGroupSymbol,
        BindingDiagnosticBag diagnostics)
    {
        return BindSimple<ISelectionEntryContainerSymbol, ErrorSymbols.ErrorSelectionEntrySymbol>(
            node, diagnostics, node.DefaultSelectionEntryId, LookupOptions.SelectionEntryOnly | LookupOptions.EntryMembersOnly, entryGroupSymbol);
    }

    internal ImmutableArray<IEntrySymbol> BindSelectionSourcePathSymbol(SelectionNode node, BindingDiagnosticBag diagnostics) =>
        BindSourcePathSymbol<ErrorSymbols.ErrorSelectionEntrySymbol>(node, node.EntryId, diagnostics, LookupOptions.SelectionEntryOnly);

    internal ImmutableArray<IEntrySymbol> BindProfileSourcePathSymbol(ProfileNode node, BindingDiagnosticBag diagnostics) =>
        BindSourcePathSymbol<ErrorSymbols.ErrorProfileSymbol>(node, node.Id, diagnostics, LookupOptions.ProfileEntryOnly);

    internal ImmutableArray<IEntrySymbol> BindRuleSourcePathSymbol(RuleNode node, BindingDiagnosticBag diagnostics) =>
        BindSourcePathSymbol<ErrorSymbols.ErrorRuleSymbol>(node, node.Id, diagnostics, LookupOptions.RuleEntryOnly);

    internal ImmutableArray<IEntrySymbol> BindSourcePathSymbol<TError>(
        SourceNode node,
        string? entryId,
        BindingDiagnosticBag diagnostics,
        LookupOptions entryKindOption)
        where TError : ErrorSymbols.ErrorSymbolBase, IEntrySymbol, new()
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(entryId));
        var parentSelection = ContainingSelectionSymbol?.Declaration == node
            ? ContainingSelectionSymbol.ContainingSymbol as ISelectionSymbol
            : ContainingSelectionSymbol;
        Debug.Assert((parentSelection as SourceDeclaredSymbol)?.Declaration != node); // we're not causing infinite recursion
        var ids = entryId.Split("::").ToImmutableArray();
        if (ids.Length == 0)
        {
            return ImmutableArray.Create<IEntrySymbol>(new ErrorSymbols.ErrorSelectionEntrySymbol()
            {
                ErrorInfo = diagnostics.Add(
                    ErrorCode.ERR_NoBindingCandidates,
                    node.GetLocation(),
                    entryId,
                    node,
                    "Node's entry ID was empty."),
            });
        }
        if (ids.Any(string.IsNullOrWhiteSpace))
        {
            diagnostics.Add(ErrorCode.ERR_GenericError, node.GetLocation(), "Empty element(s) in split EntryId.");
        }
        var qualifier = (Symbol?)parentSelection?.SourceEntry;
        var resultBuilder = ImmutableArray.CreateBuilder<IEntrySymbol>();
        // copy over already resolved links from containing selection
        if (parentSelection is { SourceEntryPath: { SourceEntries.Length: > 1 } path })
        {
            resultBuilder.AddRange(path.SourceEntries.SkipLast(1));
            qualifier = (Symbol?)path.SourceEntries[^2].ReferencedEntry;
        }
        var idsLeft = ids.AsSpan()[resultBuilder.Count..];
        for (var i = 0; i < idsLeft.Length; i++)
        {
            var idToBind = idsLeft[i];
            var opts = LookupOptions.EntryOnly;
            // if there's no qualifier, we're only looking at root entries
            opts |= qualifier is null ? LookupOptions.RootOnly : LookupOptions.Default;
            // if it's the last ID, we're only looking for specific entry kind
            opts |= i == idsLeft.Length - 1 ? entryKindOption : LookupOptions.Default;
            var entrySymbol = BindSimple<IEntrySymbol, TError>(
                node, diagnostics, idToBind, opts, qualifier);
            resultBuilder.Add(entrySymbol);
            qualifier = (Symbol?)entrySymbol.ReferencedEntry;
        }
        return resultBuilder.ToImmutable();
    }

    private TSymbol BindSimple<TSymbol, TErrorSymbol>(
        SourceNode node,
        BindingDiagnosticBag diagnostics,
        string? symbolId,
        LookupOptions options,
        Symbol? qualifier = null)
        where TSymbol : ISymbol
        where TErrorSymbol : ErrorSymbols.ErrorSymbolBase, TSymbol, new()
    {
        if (symbolId is null)
        {
            return new TErrorSymbol()
            {
                ErrorInfo = diagnostics.Add(
                    ErrorCode.ERR_NoBindingCandidates,
                    node.GetLocation(),
                    symbolId!,
                    node,
                    "Symbol ID was null."),
            };
        }
        var result = LookupResult.GetInstance();
        LookupSymbolsWithDelayedDiagnosing(result, symbolId, options, qualifier);
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
            if (options.HasFlag(LookupOptions.ResourceByDefinitionId) && symbol.Kind == SymbolKind.ResourceEntry && MatchesResourceDefinitionId(symbol, symbolId))
            {
                // the symbolId matched resource.Type.Id
            }
            else
            {
                return LookupResult.Empty();
            }
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
        if (options.HasFlag(LookupOptions.ResourceEntryOnly) && symbol.Kind != SymbolKind.ResourceEntry)
        {
            return LookupResult.Empty();
        }
        if (options.HasFlag(LookupOptions.ContainerEntryOnly) && symbol.Kind != SymbolKind.ContainerEntry)
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
        return LookupResult.Good(symbol);

        static bool MatchesResourceDefinitionId(ISymbol symbol, string symbolId)
        {
            return ((IResourceEntrySymbol)symbol).Type?.Id == symbolId;
        }
    }

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

    internal static void LookupSymbolsInSingleCatalogue(
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
        if (options.CanConsiderRootEntries())
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
        if (options.CanConsiderSharedEntries())
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
        if (result.IsMultiViable)
            return;
        if (options.CanConsiderNestedEntries())
        {
            LookupSymbolsInDescendantEntries(result, catalogue.AllItems, symbolId, options, originalBinder, diagnose);
        }
    }

    internal static void LookupSymbolsInQualifyingEntry(
        EntrySymbol qualifier,
        LookupResult result,
        string symbolId,
        LookupOptions options,
        Binder originalBinder,
        bool diagnose,
        ImmutableArray<ICatalogueSymbol> rootClosure)
    {
        Debug.Assert(qualifier is not null);
        var unreferenced = qualifier.ContainingModule is not ICatalogueSymbol containingCatalogue || !rootClosure.Contains(containingCatalogue);
        if (unreferenced)
        {
            // TODO when diagnose=true, find candidates and mark as LookupResultKind.Unreferenced
            return;
        }
        LookupSymbolsInQualifyingEntry(qualifier, result, symbolId, options, originalBinder, diagnose);
    }

    internal static void LookupSymbolsInQualifyingEntry(
        EntrySymbol qualifier,
        LookupResult result,
        string symbolId,
        LookupOptions options,
        Binder originalBinder,
        bool diagnose)
    {
        Debug.Assert(qualifier is not null);
        result.MergeEqual(originalBinder.CheckViability(qualifier, symbolId, options, diagnose));
        LookupSymbolsInDescendantEntries(result, qualifier, symbolId, options, originalBinder, diagnose);
    }

    internal static void LookupSymbolsInDescendantEntries<T>(
        LookupResult result,
        ImmutableArray<T> symbols,
        string symbolId,
        LookupOptions options,
        Binder originalBinder,
        bool diagnose)
        where T : ISymbol
    {
        foreach (var symbol in symbols)
        {
            LookupSymbolsInDescendantEntries(result, symbol, symbolId, options, originalBinder, diagnose);
        }
    }

    internal static void LookupSymbolsInDescendantEntries(
        LookupResult result,
        ISymbol symbol,
        string symbolId,
        LookupOptions options,
        Binder originalBinder,
        bool diagnose)
    {
        Debug.Assert(!options.HasFlag(LookupOptions.CatalogueOnly | LookupOptions.ResoureDefinitionOnly | LookupOptions.RootOnly | LookupOptions.SharedOnly));
        var kind = symbol.Kind;
        if (kind is not SymbolKind.ContainerEntry and not SymbolKind.ResourceEntry)
            return;
        if (symbol is not EntrySymbol entry)
            return;
        // skip looking for a container in a resource
        if (options.HasFlag(LookupOptions.ContainerEntryOnly) && kind != SymbolKind.ContainerEntry)
            return;
        var members = entry.GetMembers();
        originalBinder.CheckViability(result, members, symbolId, options, diagnose);
        if (result.IsMultiViable)
            return;
        // we consider all descendant selection entry containers and resource entries
        if (options.CanConsiderNestedEntries())
        {
            LookupSymbolsInDescendantEntries(result, members, symbolId, options, originalBinder, diagnose);
        }
        if (options.HasFlag(LookupOptions.LookupInReferencedEntryMembers) && entry.IsReference && entry.ReferencedEntry is { } refEntry)
        {
            LookupSymbolsInDescendantEntries(result, refEntry, symbolId, options, originalBinder, diagnose);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Method is WIP")]
    internal ISymbol ResultSymbol(
        LookupResult result,
        string symbolId,
        SourceNode where,
        BindingDiagnosticBag diagnostics,
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
                var diag = diagnostics.Add(
                    ErrorCode.ERR_MultipleViableBindingCandidates,
                    where.GetLocation(),
                    symbolId,
                    where,
                    result.Symbols.ToImmutableArray());
                return new ErrorSymbols.ErrorSymbolBase()
                {
                    ErrorInfo = diag,
                };
            }
        }
        wasError = true;
        if (result.Kind is LookupResultKind.Empty)
        {
            // TODO diagnostics, specific type?
            var diag = diagnostics.Add(
                ErrorCode.ERR_NoBindingCandidates,
                where.GetLocation(),
                symbolId,
                where,
                "No node with such id was found.");
            return new ErrorSymbols.ErrorSymbolBase()
            {
                ErrorInfo = diag,
            };
        }

        Debug.Assert(symbols.Count > 0);

        DiagnosticInfo? errorDiag;
        if (result.Error is { } error)
        {
            errorDiag = error;
            diagnostics.Add(new WhamDiagnostic(error, where.GetLocation()));
        }
        else
        {
            errorDiag = diagnostics.Add(
                ErrorCode.ERR_UnviableBindingCandidates,
                where.GetLocation(),
                result.Symbols.ToImmutableArray(),
                symbolId,
                where);
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
            return new ErrorSymbols.ErrorSymbolBase()
            {
                ErrorInfo = errorDiag,
            };
        }
    }

    internal virtual void LookupSymbolsInSingleBinder(
        LookupResult result,
        string symbolId,
        LookupOptions options,
        Binder originalBinder,
        bool diagnose,
        Symbol? qualifier)
    {
    }

    private Binder? LookupSymbolsWithDelayedDiagnosing(
        LookupResult result,
        string symbolId,
        LookupOptions options = LookupOptions.Default,
        Symbol? qualifier = null)
    {
        var binder = LookupSymbolsWithScopeTraversal(result, symbolId, options, diagnose: false, qualifier);
        Debug.Assert(binder is not null || result.IsClear);

        if (result.Kind is not LookupResultKind.Empty and not LookupResultKind.Viable)
        {
            result.Clear();
            // retry to get diagnosis
            var otherBinder = LookupSymbolsWithScopeTraversal(result, symbolId, options, diagnose: true, qualifier);
            Debug.Assert(binder == otherBinder);
        }
        Debug.Assert(result.IsMultiViable || result.IsClear || result.Error is not null);
        return binder;
    }

    private Binder? LookupSymbolsWithScopeTraversal(
        LookupResult result,
        string symbolId,
        LookupOptions options,
        bool diagnose,
        Symbol? qualifier)
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
                scope.LookupSymbolsInSingleBinder(result, symbolId, options, this, diagnose, qualifier);
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
                scope.LookupSymbolsInSingleBinder(tmpResult, symbolId, options, this, diagnose, qualifier);
                result.MergeEqual(tmpResult);
                tmpResult.Free();
            }
        }
        return binder;
    }

    private static bool IsRootEntry(ISymbol symbol) =>
        symbol.ContainingModule is { Kind: SymbolKind.Catalogue } module
        && module is ICatalogueSymbol { } catalogue && symbol.Kind switch
        {
            SymbolKind.ContainerEntry => catalogue.RootContainerEntries.Contains(symbol),
            SymbolKind.ResourceEntry => catalogue.RootResourceEntries.Contains(symbol),
            _ => false,
        };

    private static bool IsSharedEntry(ISymbol symbol) =>
        symbol.ContainingModule is { Kind: SymbolKind.Catalogue } module
        && module is ICatalogueSymbol { } catalogue && symbol.Kind switch
        {
            SymbolKind.ContainerEntry => catalogue.SharedSelectionEntryContainers.Contains(symbol),
            SymbolKind.ResourceEntry => catalogue.SharedResourceEntries.Contains(symbol),
            _ => false,
        };
}
