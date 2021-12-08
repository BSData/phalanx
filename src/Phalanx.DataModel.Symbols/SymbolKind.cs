namespace Phalanx.DataModel.Symbols;

public enum SymbolKind
{
    // grouping of the same-gamesystem-id elements
    Namespace,

    // catalogue, gamesystem (special kind of catalogue)
    Catalogue,

    // selection entry, selection entry group, force entry?, category entry?
    Entry,

    // condition, constraint, modifier, modifier group, repeat
    Logic,

    // characteristic type, cost type, profile type, category entry?
    ResourceType,

    // characteristics, costs, profiles, rules, info groups?, categories?, publications?
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
