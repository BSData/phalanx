namespace WarHub.ArmouryModel;

/// <summary>
/// A symbol that's an instance of a resource entry (<see cref="IResourceEntrySymbol"/>),
/// identified by <see cref="SourceEntry"/> which was instantiated by traversing
/// links contained in <see cref="SourceEntryPath"/>.
/// </summary>
public interface IResourceSymbol : IEntryInstanceSymbol
{
    ResourceKind ResourceKind { get; }

    new IResourceEntrySymbol SourceEntry { get; }
}
