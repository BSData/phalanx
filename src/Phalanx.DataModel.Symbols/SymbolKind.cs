namespace Phalanx.DataModel.Symbols;

public enum SymbolKind
{
    Error,

    // grouping of the same-gamesystem-id elements
    Namespace,

    // catalogue, gamesystem ("root" catalogue)
    Catalogue,

    // selection entry, selection entry group, force entry, category entry
    ContainerEntry,

    // condition, constraint, modifier, modifier group, repeat
    Logic,

    // characteristic type, cost type, profile type
    ResourceType,

    // characteristics, costs, profiles, rules, info groups, publications
    Resource,

    // all kinds of links
    Link,

    // roster
    Roster,

    // force in roster
    Force,

    // roster selection
    Selection,
}
