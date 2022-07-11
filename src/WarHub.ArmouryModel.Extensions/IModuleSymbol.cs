namespace WarHub.ArmouryModel;

/// <summary>
/// A module is a standalone data blob, such as roster or catalogue, that can be saved, loaded,
/// distributed, versioned, and referenced by other modules. It also belongs to a gamesystem.
/// </summary>
public interface IModuleSymbol : ISymbol
{
    /// <summary>
    /// Game system catalogue (root), that this module is associated with.
    /// </summary>
    ICatalogueSymbol Gamesystem { get; }
}
