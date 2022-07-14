namespace WarHub.ArmouryModel;

public enum SymbolKind
{
    /// <summary>
    /// The kind is not known, or invalid.
    /// </summary>
    Error,

    // grouping of the same-gamesystem-id elements
    Namespace,

    // catalogue, gamesystem ("root" catalogue)
    Catalogue,

    // selection entry, selection entry group, force entry, category entry, entry links
    ContainerEntry,

    // condition, constraint, modifier, modifier group, repeat
    Logic,

    // characteristic type, cost type, profile type
    ResourceDefinition,

    // characteristics, costs, profiles, rules, info groups, publications, info links
    Resource,

    // all kinds of links (references): catalogue reference, publication reference, entry path reference
    Link,

    // roster
    Roster,

    // force in roster
    Force,

    // category in force, category of selection
    Category,

    // roster selection
    Selection,
}
