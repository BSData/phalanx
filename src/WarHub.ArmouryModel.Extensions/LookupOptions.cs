namespace WarHub.ArmouryModel;

/// <summary>
/// Defines a set of options that constrain items taken into account during lookup.
/// </summary>
[Flags]
internal enum LookupOptions
{
    /// <summary>
    /// No additional constraints.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Look only for <see cref="ICatalogueSymbol"/>s.
    /// </summary>
    CatalogueOnly = 1 << 1,

    /// <summary>
    /// Look only for <see cref="IResourceDefinitionSymbol"/>s.
    /// </summary>
    ResoureDefinitionOnly = 1 << 2,

    /// <summary>
    /// Look only for <see cref="IPublicationSymbol"/>s.
    /// </summary>
    PublicationOnly = 1 << 3 | ResoureDefinitionOnly,

    /// <summary>
    /// Look only for <see cref="IResourceDefinitionSymbol"/> with <see cref="ResourceKind.Cost"/>.
    /// </summary>
    CostTypeOnly = 1 << 4 | ResoureDefinitionOnly,
    ProfileTypeOnly = 1 << 5 | ResoureDefinitionOnly,
    CharacteristicTypeOnly = 1 << 6 | ResoureDefinitionOnly,
    EntryOnly = 1 << 7,
    ResourceOnly = 1 << 8,
    ResourceEntryOnly = ResourceOnly | EntryOnly,
    CostOnly = 1 << 9 | ResourceEntryOnly,
    RuleEntryOnly = 1 << 10 | ResourceEntryOnly,
    ProfileEntryOnly = 1 << 11 | ResourceEntryOnly,
    CharacteristicEntryOnly = 1 << 12 | ResourceEntryOnly,
    ResourceGroupEntryOnly = 1 << 13 | ResourceEntryOnly,
    ContainerOnly = 1 << 14,
    ContainerEntryOnly = ContainerOnly | EntryOnly,
    ForceEntryOnly = 1 << 15 | ContainerEntryOnly,
    CategoryEntryOnly = 1 << 16 | ContainerEntryOnly,
    SelectionEntryOnly = 1 << 17 | ContainerEntryOnly,
    SelectionGroupEntryOnly = 1 << 18 | ContainerEntryOnly,
    SingleLevel = 1 << 19,
    SharedOnly = 1 << 20,
    RootOnly = 1 << 21,
    EntryMembersOnly = 1 << 22,
    /// <summary>
    /// Bind to resources when their definition's ID matches the looked-up symbol ID.
    /// </summary>
    ResourceByDefinitionId = 1 << 23,
}

internal static class LookupOptionsExtensions
{
    internal static bool CanConsiderCatalogues(this LookupOptions options) =>
        (options & (LookupOptions.ResoureDefinitionOnly | LookupOptions.EntryOnly | LookupOptions.EntryMembersOnly)) == 0;

    internal static bool CanConsiderResourceDefinitions(this LookupOptions options) =>
        (options & (LookupOptions.CatalogueOnly | LookupOptions.EntryOnly | LookupOptions.EntryMembersOnly)) == 0;

    internal static bool CanConsiderResourceEntries(this LookupOptions options) =>
        (options & (LookupOptions.CatalogueOnly | LookupOptions.ResoureDefinitionOnly | LookupOptions.ContainerOnly)) == 0;

    internal static bool CanConsiderContainerEntries(this LookupOptions options) =>
        (options & (LookupOptions.CatalogueOnly | LookupOptions.ResoureDefinitionOnly | LookupOptions.ResourceOnly)) == 0;

    internal static bool CanConsiderSharedEntries(this LookupOptions options) =>
        (options & (LookupOptions.CatalogueOnly | LookupOptions.ResoureDefinitionOnly | LookupOptions.RootOnly)) == 0;

    internal static bool CanConsiderRootEntries(this LookupOptions options) =>
        (options & (LookupOptions.CatalogueOnly | LookupOptions.ResoureDefinitionOnly | LookupOptions.SharedOnly)) == 0;

    internal static bool CanConsiderConstraints(this LookupOptions options) =>
        (options & (LookupOptions.CatalogueOnly | LookupOptions.ResoureDefinitionOnly | LookupOptions.EntryOnly)) == 0;

    internal static bool CanConsiderNestedEntries(this LookupOptions options) =>
        (options & (LookupOptions.CatalogueOnly | LookupOptions.ResoureDefinitionOnly | LookupOptions.RootOnly | LookupOptions.SharedOnly | LookupOptions.SingleLevel)) == 0;
}
