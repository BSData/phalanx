namespace WarHub.ArmouryModel;

/// <summary>
/// Extensions on <see cref="ISymbol"/>.
/// </summary>
public static class ISymbolExtensions
{
    public static bool IsKind(this ISymbol symbol, SymbolKind kind) => symbol.Kind == kind;

    public static bool IsContainerKind(this ISymbol symbol, ContainerKind kind) => symbol switch
    {
        { Kind: SymbolKind.ContainerEntry } and IContainerEntrySymbol { ContainerKind: var x } => x == kind,
        { Kind: SymbolKind.Container } and IContainerEntryInstanceSymbol { ContainerKind: var x } => x == kind,
        _ => false,
    };

    public static bool IsResourceKind(this ISymbol symbol, ResourceKind kind) => symbol switch
    {
        { Kind: SymbolKind.ResourceDefinition } and IResourceDefinitionSymbol { ResourceKind: var x } => x == kind,
        { Kind: SymbolKind.ResourceEntry } and IResourceEntrySymbol { ResourceKind: var x } => x == kind,
        { Kind: SymbolKind.Resource } and IResourceSymbol { ResourceKind: var x } => x == kind,
        _ => false,
    };
}
