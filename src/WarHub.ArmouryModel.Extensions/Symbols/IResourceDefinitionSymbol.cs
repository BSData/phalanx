namespace WarHub.ArmouryModel;

/// <summary>
/// Umbrella type for all statically defined resources used in other entries.
/// </summary>
public interface IResourceDefinitionSymbol : ISymbol
{
    /// <summary>
    /// Describes what kind of resource is being defined.
    /// </summary>
    ResourceKind ResourceKind { get; }

    /// <summary>
    /// Collection of nested resource definitions, that belong to this definition.
    /// This contains e.g. profile type's characteristic types.
    /// </summary>
    ImmutableArray<IResourceDefinitionSymbol> Definitions { get; }
}
