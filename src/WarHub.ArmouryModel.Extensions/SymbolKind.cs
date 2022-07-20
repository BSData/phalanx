namespace WarHub.ArmouryModel;

/// <summary>
/// Discriminates key types of <see cref="ISymbol"/>.
/// </summary>
public enum SymbolKind
{
    /// <summary>
    /// The kind is not known, or invalid.
    /// </summary>
    Error,

    // grouping of the same-gamesystem-id elements
    /// <summary>
    /// Namespace - group of elements belonging to a single gamesystem, commonly the root symbol.
    /// <see cref="IGamesystemNamespaceSymbol"/>.
    /// </summary>
    Namespace,

    /// <summary>
    /// Catalogue module (including root catalogue aka gamesystem).
    /// <see cref="ICatalogueSymbol"/>.
    /// </summary>
    Catalogue,

    /// <summary>
    /// Roster module. <see cref="IRosterSymbol"/>.
    /// </summary>
    Roster,

    /// <summary>
    /// Resource definition. Further discriminated by <see cref="ResourceKind"/>.
    /// Includes characteristic type, cost type, profile type.
    /// </summary>
    ResourceDefinition,

    /// <summary>
    /// Resource entry. Further discriminated by <see cref="ResourceKind"/>.
    /// Includes resource entries: characteristics, costs, profiles, rules,
    /// info groups, publications, info links.
    /// </summary>
    ResourceEntry,

    /// <summary>
    /// Resource instance. Further discriminated by <see cref="ResourceKind"/>.
    /// Includes actual resources: rules, profiles in a roster.
    /// </summary>
    Resource,

    /// <summary>
    /// Container entry. Further discriminated by <see cref="ContainerKind"/>.
    /// Includes selection entries, selection entry groups, force entries,
    /// category entries, category/entry links.
    /// </summary>
    ContainerEntry,

    /// <summary>
    /// Container instance. Further discriminated by <see cref="ContainerKind"/>.
    /// Includes entry instances in roster: force, category, selection.
    /// </summary>
    Container,

    /// <summary>
    /// Constraint that creates boundaries of allowed counts.
    /// </summary>
    Constraint,

    /// <summary>
    /// Effect symbol conditionally applies modifications to a symbol.
    /// </summary>
    Effect,

    /// <summary>
    /// Defines a condition that can be satisfied or not.
    /// </summary>
    Condition,

    /// <summary>
    /// Defines a query that asks questions about a roster state and returns some results.
    /// </summary>
    Query,

    /// <summary>
    /// Symbol referring to some other element.
    /// Includes all kinds of links (references): catalogue reference,
    /// publication reference, entry path reference.
    /// </summary>
    Link,

    /// <summary>
    /// Special cost symbol, includes an optional limit.
    /// <see cref="IRosterCostSymbol"/>.
    /// </summary>
    RosterCost,
}
