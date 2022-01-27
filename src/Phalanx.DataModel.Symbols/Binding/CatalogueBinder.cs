using Phalanx.DataModel.Symbols.Implementation;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Binding;

internal class CatalogueBaseBinder : Binder
{
    public CatalogueBaseSymbol Catalogue { get; }

    private ImmutableArray<ICatalogueSymbol> lazyRootClosure;

    internal CatalogueBaseBinder(Binder next, CatalogueBaseSymbol catalogue) : base(next)
    {
        Catalogue = catalogue;
    }

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

    internal ImmutableArray<T> GetWithId<T>(Func<ICatalogueSymbol, IEnumerable<T>> getItems, string? id) where T : ISymbol
    {
        if (id is null)
            return ImmutableArray<T>.Empty;
        return RootClosure
            .SelectMany(x => getItems(x))
            .Where(x => x.Id == id)
            .ToImmutableArray();
    }

    internal override ICategoryEntrySymbol? BindCategoryEntrySymbol(string? targetId)
    {
        var candidates = GetWithId(x => x.RootContainerEntries.OfType<ICategoryEntrySymbol>(), targetId);
        if (candidates.Length > 0)
        {
            // TODO log diagnostic when >1
            return candidates[0];
        }
        return NextRequired.BindCategoryEntrySymbol(targetId);
    }

    internal override ICostTypeSymbol? BindCostTypeSymbol(string? typeId)
    {
        var candidates = GetWithId(x => x.ResourceDefinitions.OfType<ICostTypeSymbol>(), typeId);
        if (candidates.Length > 0)
        {
            // TODO log diagnostic when >1
            return candidates[0];
        }
        return NextRequired.BindCostTypeSymbol(typeId);
    }

    internal override IPublicationSymbol BindPublicationSymbol(string? publicationId)
    {
        var candidates = GetWithId(x => x.ResourceDefinitions.OfType<IPublicationSymbol>(), publicationId);
        if (candidates.Length > 0)
        {
            // TODO log diagnostic when >1
            return candidates[0];
        }
        return NextRequired.BindPublicationSymbol(publicationId);
    }

    internal override IProfileTypeSymbol? BindProfileTypeSymbol(string? typeId)
    {
        var candidates = GetWithId(x => x.ResourceDefinitions.OfType<IProfileTypeSymbol>(), typeId);
        if (candidates.Length > 0)
        {
            // TODO log diagnostic when >1
            return candidates[0];
        }
        return NextRequired.BindProfileTypeSymbol(typeId);
    }

    internal override ISelectionEntryContainerSymbol? BindSharedSelectionEntrySymbol(string? targetId, EntryLinkKind type)
    {
        var candidates = type switch
        {
            EntryLinkKind.SelectionEntry =>
                GetWithId(x => x.SharedSelectionEntryContainers.Where(x => x.ContainerKind == ContainerEntryKind.Selection), targetId),
            EntryLinkKind.SelectionEntryGroup =>
                GetWithId(x => x.SharedSelectionEntryContainers.Where(x => x.ContainerKind == ContainerEntryKind.SelectionGroup), targetId),
            _ => ImmutableArray<ISelectionEntryContainerSymbol>.Empty
        };
        if (candidates.Length > 0)
        {
            // TODO log diagnostic when >1
            return candidates[0];
        }
        return NextRequired.BindSharedSelectionEntrySymbol(targetId, type);
    }
}
