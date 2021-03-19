using System;
using System.Collections.Immutable;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols
{
    public enum SymbolKind
    {
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
    public interface ISymbol
    {
        SymbolKind Kind { get; }
        string Name { get; }
        ISymbol ContainingSymbol { get; }
    }
    interface ICatalogueSymbol : ISymbol
    {
        ImmutableArray<ICatalogueItemSymbol> Items { get; }
    }
    interface ICatalogueItemSymbol : ISymbol
    {
        ICatalogueSymbol ContainingCatalogue { get; }
    }
    interface IEntrySymbol : ICatalogueItemSymbol
    {
        ImmutableArray<IEffectSymbol> Effects { get; }
    }
    interface IResourceEntryOrContainerSymbol : IEntrySymbol
    {

    }
    interface IResourceEntrySymbol : IEntrySymbol, IResourceEntryOrContainerSymbol
    {
        IResourceTypeSymbol Type { get; }
    }
    interface ICharacteristicSymbol : IResourceEntrySymbol
    {
        new ICharacteristicTypeSymbol Type { get; }
    }
    interface ICostSymbol : IResourceEntrySymbol
    {
        new ICostTypeSymbol Type { get; }
    }
    interface IProfileSymbol : IResourceEntrySymbol
    {
        new IProfileTypeSymbol Type { get; }
    }
    interface IRuleSymbol : IResourceEntrySymbol
    { }
    interface IContainerEntrySymbol : IEntrySymbol
    {
        ImmutableArray<IResourceEntryOrContainerSymbol> Resources { get; }
    }
    interface IResourceContainerSymbol : IContainerEntrySymbol, IResourceEntryOrContainerSymbol
    { }
    interface ICoreEntrySymbol : IContainerEntrySymbol
    {
        ImmutableArray<IConstraintSymbol> Constraints { get; }
    }
    interface ICategoryEntrySymbol : ICoreEntrySymbol
    { }
    interface IForceEntrySymbol : ICoreEntrySymbol
    {
        ImmutableArray<IForceEntrySymbol> NestedForces { get; }
    }
    interface ISelectionEntryOrGroupSymbol : ICoreEntrySymbol
    {
        ImmutableArray<ISelectionEntryOrGroupSymbol> Children { get; }
    }
    interface ISelectionEntrySymbol : ISelectionEntryOrGroupSymbol
    { }
    interface ISelectionEntryGroupSymbol : ISelectionEntryOrGroupSymbol
    { }
    interface IResourceTypeSymbol : ICatalogueItemSymbol
    { }
    interface ICharacteristicTypeSymbol : IResourceTypeSymbol
    { }
    interface ICostTypeSymbol : IResourceTypeSymbol
    { }
    interface IProfileTypeSymbol : IResourceTypeSymbol
    { }
    interface ILogicSymbol : ICatalogueItemSymbol
    { }
    interface IConstraintSymbol : ILogicSymbol
    { }
    interface IEffectSymbol : ILogicSymbol
    {
        ImmutableArray<ITriggerSymbol> Triggers { get; }
    }
    interface IModifierSymbol : IEffectSymbol
    { }
    interface IModifierGroupSymbol : IEffectSymbol
    {
        ImmutableArray<IEffectSymbol> Children { get; }
    }
    interface ITriggerSymbol : ILogicSymbol
    {
        IEffectSymbol Effect { get; }
    }
    interface IConditionSymbol : ITriggerSymbol
    { }
    interface IConditionGroupSymbol : ITriggerSymbol
    {
        ImmutableArray<ITriggerSymbol> Children { get; }
    }
    interface IRepeatSymbol : ITriggerSymbol
    { }
    interface IRosterSymbol : ISymbol
    { }
    interface IForceOrSelectionSymbol : ISymbol
    { }
    interface IForceSymbol : IForceOrSelectionSymbol
    { }
    interface ISelectionSymbol : IForceOrSelectionSymbol
    {
        IForceOrSelectionSymbol Parent { get; }
    }
}
