namespace WarHub.ArmouryModel;

/// <summary>
/// A reference of a publication symbol with a page detail.
/// WHAM <see cref="Source.IPublicationReferencingNode" />.
/// </summary>
public interface IPublicationReferenceSymbol : ISymbol
{
    IPublicationSymbol Publication { get; }

    string Page { get; }
}
