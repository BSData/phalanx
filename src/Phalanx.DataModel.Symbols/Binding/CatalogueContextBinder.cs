using Phalanx.DataModel.Symbols.Implementation;

namespace Phalanx.DataModel.Symbols.Binding;

public class CatalogueContextBinder : Binder
{
    public CatalogueSymbol Catalogue { get; }

    internal CatalogueContextBinder(Binder next, CatalogueSymbol catalogue) : base(next)
    {
        Catalogue = catalogue;
    }

    internal IEnumerable<ICatalogueSymbol> GetImportedCatalogues()
    {
        return Catalogue.Imports.Select(x => x.Catalogue).Append(Catalogue.Gamesystem);
    }

    internal override ICostTypeSymbol? BindCostTypeSymbol(string? typeId)
    {
        foreach (var import in GetImportedCatalogues())
        {
            var candidates = Catalogue.ResourceDefinitions.OfType<ICostTypeSymbol>()
                .Where(x => x.Id == typeId).ToImmutableArray();
            if (candidates.Length > 0)
            {
                // TODO log diagnostic when >1
                return candidates[0];
            }
            // TODO what if more imports define it?
        }
        return NextRequired.BindCostTypeSymbol(typeId);
    }

    internal override IPublicationSymbol BindPublicationSymbol(string? publicationId)
    {
        foreach (var import in GetImportedCatalogues())
        {
            var candidates = Catalogue.ResourceDefinitions.OfType<IPublicationSymbol>()
                .Where(x => x.Id == publicationId).ToImmutableArray();
            if (candidates.Length > 0)
            {
                // TODO log diagnostic when >1
                return candidates[0];
            }
            // TODO what if more imports define it?
        }
        return NextRequired.BindPublicationSymbol(publicationId);
    }

    internal override IProfileTypeSymbol? BindProfileTypeSymbol(string? typeId)
    {
        foreach (var import in GetImportedCatalogues())
        {
            var candidates = Catalogue.ResourceDefinitions.OfType<IProfileTypeSymbol>()
                .Where(x => x.Id == typeId).ToImmutableArray();
            if (candidates.Length > 0)
            {
                // TODO log diagnostic when >1
                return candidates[0];
            }
            // TODO what if more imports define it?
        }
        return NextRequired.BindProfileTypeSymbol(typeId);
    }
}
