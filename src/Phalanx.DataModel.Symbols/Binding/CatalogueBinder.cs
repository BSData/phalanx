using Phalanx.DataModel.Symbols.Implementation;

namespace Phalanx.DataModel.Symbols.Binding;

public class CatalogueBinder : Binder
{
    public CatalogueSymbol Catalogue { get; }

    internal CatalogueBinder(Binder next, CatalogueSymbol catalogue) : base(next)
    {
        Catalogue = catalogue;
    }

    internal override ICostTypeSymbol? BindCostTypeSymbol(string? typeId)
    {
        var candidates = Catalogue.ResourceDefinitions.OfType<ICostTypeSymbol>()
            .Where(x => x.Id == typeId).ToImmutableArray();
        if (candidates.Length > 0)
        {
            // TODO log diagnostic when >1
            return candidates[0];
        }
        return NextRequired.BindCostTypeSymbol(typeId);
    }

    internal override IPublicationSymbol BindPublicationSymbol(string? publicationId)
    {
        var candidates = Catalogue.ResourceDefinitions.OfType<IPublicationSymbol>()
            .Where(x => x.Id == publicationId).ToImmutableArray();
        if (candidates.Length > 0)
        {
            // TODO log diagnostic when >1
            return candidates[0];
        }
        return NextRequired.BindPublicationSymbol(publicationId);
    }

    internal override IProfileTypeSymbol? BindProfileTypeSymbol(string? typeId)
    {
        var candidates = Catalogue.ResourceDefinitions.OfType<IProfileTypeSymbol>()
            .Where(x => x.Id == typeId).ToImmutableArray();
        if (candidates.Length > 0)
        {
            // TODO log diagnostic when >1
            return candidates[0];
        }
        return NextRequired.BindProfileTypeSymbol(typeId);
    }
}
