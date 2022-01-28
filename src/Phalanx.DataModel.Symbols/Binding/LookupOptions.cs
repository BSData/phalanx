namespace Phalanx.DataModel.Symbols.Binding;

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
    PublicationOnly = (1 << 3) | ResoureDefinitionOnly,

    /// <summary>
    /// Look only for <see cref="ICostTypeSymbol"/>s.
    /// </summary>
    CostTypeOnly = (1 << 4) | ResoureDefinitionOnly,
    ProfileTypeOnly = (1 << 5) | ResoureDefinitionOnly,
    CharacteristicTypeOnly = (1 << 6) | ResoureDefinitionOnly,
    EntryOnly = 1 << 7,
    ResourceEntryOnly = (1 << 8) | EntryOnly,
    CostOnly = (1 << 9) | ResourceEntryOnly,
    RuleEntryOnly = (1 << 10) | ResourceEntryOnly,
    ProfileEntryOnly = (1 << 11) | ResourceEntryOnly,
    CharacteristicEntryOnly = (1 << 12) | ResourceEntryOnly,
    ContainerEntryOnly = (1 << 13) | EntryOnly,
    ForceEntryOnly = (1 << 14) | ContainerEntryOnly,
    CategoryEntryOnly = (1 << 15) | ContainerEntryOnly,
    SelectionEntryOnly = (1 << 16) | ContainerEntryOnly,
    SelectionGroupEntryOnly = (1 << 17) | ContainerEntryOnly,
    SharedEntryOnly = (1 << 18) | EntryOnly,
    RootEntryOnly = (1 << 19) | EntryOnly,
    ResourceGroupEntryOnly = (1 << 20) | ResourceEntryOnly,
}

internal static class LookupOptionsExtensions
{
    internal static bool CanConsiderCatalogues(this LookupOptions options) =>
        (options & (LookupOptions.ResoureDefinitionOnly | LookupOptions.EntryOnly)) == 0;

    internal static bool CanConsiderResourceDefinitions(this LookupOptions options) =>
        (options & (LookupOptions.CatalogueOnly | LookupOptions.EntryOnly)) == 0;

    internal static bool CanConsiderResourceEntries(this LookupOptions options) =>
        (options & (LookupOptions.CatalogueOnly | LookupOptions.ResoureDefinitionOnly | LookupOptions.ContainerEntryOnly)) == 0;

    internal static bool CanConsiderContainerEntries(this LookupOptions options) =>
        (options & (LookupOptions.CatalogueOnly | LookupOptions.ResoureDefinitionOnly | LookupOptions.ResourceEntryOnly)) == 0;
}
