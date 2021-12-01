using Phalanx.DataModel.Symbols.Implementation;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Binding;

public class CatalogueBinder : Binder
{
    public CatalogueSymbol Catalogue { get; }

    internal CatalogueBinder(Binder next, CatalogueSymbol catalogue) : base(next)
    {
        Catalogue = catalogue;
    }

    internal IEnumerable<ICatalogueSymbol> GetImportedCatalogues()
    {
        // TODO imports of imports?
        return Catalogue.Imports.Select(x => x.Catalogue).Append(Catalogue.Gamesystem);
    }

    internal ImmutableArray<T> GetWithId<T>(Func<ICatalogueSymbol, IEnumerable<T>> getItems, string? id) where T : ISymbol
    {
        if (id is null)
            return ImmutableArray<T>.Empty;
        return GetImportedCatalogues()
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
                GetWithId(x => x.SharedSelectionEntryContainers.Where(x => x.IsSelectionEntry), targetId),
            EntryLinkKind.SelectionEntryGroup =>
                GetWithId(x => x.SharedSelectionEntryContainers.Where(x => x.IsSelectionGroup), targetId),
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
