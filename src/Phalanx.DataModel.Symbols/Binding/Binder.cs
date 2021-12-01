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
}
