namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Umbrella type for all statically defined resources used in other entries.
/// </summary>
public interface IResourceDefinitionSymbol : ICatalogueItemSymbol
{
    string? Id { get; }
}
