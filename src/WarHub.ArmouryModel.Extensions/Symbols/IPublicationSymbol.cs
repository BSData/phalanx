namespace WarHub.ArmouryModel;

/// <summary>
/// Defines a publication.
/// BS Publication.
/// WHAM <see cref="Source.PublicationNode" />.
/// </summary>
public interface IPublicationSymbol : IResourceDefinitionSymbol
{
    /// <summary>
    /// A short name like an acronym or abbreviation for this publication, optional.
    /// </summary>
    string? ShortName { get; }

    /// <summary>
    /// Name of the publisher of the material, optional.
    /// </summary>
    string? Publisher { get; }

    /// <summary>
    /// Date at which the publication was released for the first time, optional.
    /// </summary>
    DateOnly? PublicationDate { get; }

    /// <summary>
    /// A public URL this publication can be found at.
    /// </summary>
    Uri? PublicationUrl { get; }
}
