namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Describes a kind of resource for both <see cref="IResourceDefinitionSymbol"/>
/// and <see cref="IResourceEntrySymbol"/>.
/// </summary>
public enum ResourceKind
{
    /// <summary>
    /// The kind is not known, or invalid.
    /// </summary>
    Error,

    /// <summary>
    /// Profile element, <see cref="ICharacteristicTypeSymbol"/> and <see cref="ICharacteristicSymbol"/>.
    /// </summary>
    Characteristic,

    /// <summary>
    /// Numeric cost of a selection, <see cref="ICostTypeSymbol"/> and <see cref="ICostSymbol"/>.
    /// </summary>
    Cost,

    /// <summary>
    /// Set of characteristics, <see cref="IProfileTypeSymbol"/> and <see cref="IProfileSymbol"/>.
    /// </summary>
    Profile,

    /// <summary>
    /// A publication description, <see cref="IPublicationSymbol"/>.
    /// </summary>
    Publication,

    /// <summary>
    /// A textual game rule, <see cref="IRuleSymbol"/>.
    /// </summary>
    Rule,
    
    /// <summary>
    /// A set of resources, <see cref="IResourceGroupSymbol"/>.
    /// </summary>
    Group,
}
