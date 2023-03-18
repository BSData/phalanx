namespace WarHub.ArmouryModel;

/// <summary>
/// Gamesystem Namespace is a container for all root entities
/// that belong to a single GameSystem ID (gamesystem, catalogues, rosters).
/// </summary>
public interface IGamesystemNamespaceSymbol : ISymbol
{
    /// <summary>
    /// Gets the root catalogue AKA gamesystem.
    /// </summary>
    ICatalogueSymbol RootCatalogue { get; }

    /// <summary>
    /// Gets catalogues in this namespace. This includes <see cref="RootCatalogue"/>.
    /// </summary>
    ImmutableArray<ICatalogueSymbol> Catalogues { get; }

    /// <summary>
    /// Gets rosters in this namespace.
    /// </summary>
    ImmutableArray<IRosterSymbol> Rosters { get; }

    /// <summary>
    /// Get all root symbols in this namespace: <see cref="Catalogues"/> and <see cref="Rosters"/>.
    /// </summary>
    ImmutableArray<ISymbol> AllRootSymbols { get; }
}
