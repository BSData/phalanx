namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Defines a publication.
/// BS Publication.
/// WHAM <see cref="WarHub.ArmouryModel.Source.PublicationNode" />.
/// </summary>
public interface IPublicationSymbol : IResourceDefinitionSymbol
{
    string? ShortName { get; }
    string? Publisher { get; }
    DateOnly? PublicationDate { get; }
    Uri? PublicationUrl { get; }
}
