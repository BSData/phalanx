namespace WarHub.ArmouryModel;

/// <summary>
/// Force instance in a roster.
/// BS Force.
/// WHAM <see cref="Source.ForceNode" />.
/// </summary>
public interface IForceSymbol : ISelectionContainerSymbol, IForceContainerSymbol
{
    new IForceEntrySymbol SourceEntry { get; }

    ICatalogueReferenceSymbol CatalogueReference { get; }

    // TODO catalogue reference: id, name, revision?

    /// <summary>
    /// Categories declared in the <see cref="SourceEntry"/>.
    /// </summary>
    ImmutableArray<ICategorySymbol> Categories { get; }

    /// <summary>
    /// Publications provided by Catalogue.
    /// </summary>
    ImmutableArray<IPublicationSymbol> Publications { get; }
}
