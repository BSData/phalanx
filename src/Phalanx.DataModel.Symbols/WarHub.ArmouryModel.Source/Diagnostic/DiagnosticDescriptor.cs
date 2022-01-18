namespace WarHub.ArmouryModel.Source;

/// <summary>
/// Provides a description about a <see cref="Diagnostic"/>
/// </summary>
public sealed class DiagnosticDescriptor : IEquatable<DiagnosticDescriptor?>
{
    /// <summary>
    /// An unique identifier for the diagnostic.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// A short localizable title describing the diagnostic.
    /// </summary>
    public LocalizableString Title { get; }

    /// <summary>
    /// An optional longer localizable description for the diagnostic.
    /// </summary>
    public LocalizableString Description { get; }

    /// <summary>
    /// An optional hyperlink that provides more detailed information regarding the diagnostic.
    /// </summary>
    public string HelpLinkUri { get; }

    /// <summary>
    /// A localizable format message string, which can be passed as the first argument to <see cref="string.Format(string, object[])"/> when creating the diagnostic message with this descriptor.
    /// </summary>
    /// <returns></returns>
    public LocalizableString MessageFormat { get; }

    /// <summary>
    /// The category of the diagnostic (like Design, Naming etc.)
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// The default severity of the diagnostic.
    /// </summary>
    public DiagnosticSeverity DefaultSeverity { get; }

    /// <summary>
    /// Returns true if the diagnostic is enabled by default.
    /// </summary>
    public bool IsEnabledByDefault { get; }

    /// <summary>
    /// Custom tags for the diagnostic.
    /// </summary>
    public IEnumerable<string> CustomTags { get; }

    internal ImmutableArray<string> ImmutableCustomTags => (ImmutableArray<string>)CustomTags;

    /// <summary>
    /// Create a DiagnosticDescriptor, which provides description about a <see cref="Diagnostic"/>.
    /// NOTE: For localizable <paramref name="title"/>, <paramref name="description"/> and/or <paramref name="messageFormat"/>,
    /// use constructor overload <see cref="DiagnosticDescriptor(string, LocalizableString, LocalizableString, string, DiagnosticSeverity, bool, LocalizableString, string, string[])"/>.
    /// </summary>
    /// <param name="id">A unique identifier for the diagnostic. For example, code analysis diagnostic ID "CA1001".</param>
    /// <param name="title">A short title describing the diagnostic. For example, for CA1001: "Types that own disposable fields should be disposable".</param>
    /// <param name="messageFormat">A format message string, which can be passed as the first argument to <see cref="string.Format(string, object[])"/> when creating the diagnostic message with this descriptor.
    /// For example, for CA1001: "Implement IDisposable on '{0}' because it creates members of the following IDisposable types: '{1}'."</param>
    /// <param name="category">The category of the diagnostic (like Design, Naming etc.). For example, for CA1001: "Microsoft.Design".</param>
    /// <param name="defaultSeverity">Default severity of the diagnostic.</param>
    /// <param name="isEnabledByDefault">True if the diagnostic is enabled by default.</param>
    /// <param name="description">An optional longer description of the diagnostic.</param>
    /// <param name="helpLinkUri">An optional hyperlink that provides a more detailed description regarding the diagnostic.</param>
    /// <param name="customTags">Optional custom tags for the diagnostic. See <see cref="WellKnownDiagnosticTags"/> for some well known tags.</param>
    public DiagnosticDescriptor(
        string id,
        string title,
        string messageFormat,
        string category,
        DiagnosticSeverity defaultSeverity,
        bool isEnabledByDefault,
        string? description = null,
        string? helpLinkUri = null,
        params string[] customTags)
        : this(id, title, messageFormat, category, defaultSeverity, isEnabledByDefault, description, helpLinkUri, customTags.ToImmutableArray())
    {
    }

    /// <summary>
    /// Create a DiagnosticDescriptor, which provides description about a <see cref="Diagnostic"/>.
    /// </summary>
    /// <param name="id">A unique identifier for the diagnostic. For example, code analysis diagnostic ID "CA1001".</param>
    /// <param name="title">A short localizable title describing the diagnostic. For example, for CA1001: "Types that own disposable fields should be disposable".</param>
    /// <param name="messageFormat">A localizable format message string, which can be passed as the first argument to <see cref="string.Format(string, object[])"/> when creating the diagnostic message with this descriptor.
    /// For example, for CA1001: "Implement IDisposable on '{0}' because it creates members of the following IDisposable types: '{1}'."</param>
    /// <param name="category">The category of the diagnostic (like Design, Naming etc.). For example, for CA1001: "Microsoft.Design".</param>
    /// <param name="defaultSeverity">Default severity of the diagnostic.</param>
    /// <param name="isEnabledByDefault">True if the diagnostic is enabled by default.</param>
    /// <param name="description">An optional longer localizable description of the diagnostic.</param>
    /// <param name="helpLinkUri">An optional hyperlink that provides a more detailed description regarding the diagnostic.</param>
    /// <param name="customTags">Optional custom tags for the diagnostic. See <see cref="WellKnownDiagnosticTags"/> for some well known tags.</param>
    /// <remarks>Example descriptor for rule CA1001:
    ///     internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(RuleId,
    ///         new LocalizableResourceString(nameof(FxCopRulesResources.TypesThatOwnDisposableFieldsShouldBeDisposable), FxCopRulesResources.ResourceManager, typeof(FxCopRulesResources)),
    ///         new LocalizableResourceString(nameof(FxCopRulesResources.TypeOwnsDisposableFieldButIsNotDisposable), FxCopRulesResources.ResourceManager, typeof(FxCopRulesResources)),
    ///         FxCopDiagnosticCategory.Design,
    ///         DiagnosticSeverity.Warning,
    ///         isEnabledByDefault: true,
    ///         helpLinkUri: "http://msdn.microsoft.com/library/ms182172.aspx",
    ///         customTags: DiagnosticCustomTags.Microsoft);
    /// </remarks>
    public DiagnosticDescriptor(
        string id,
        LocalizableString title,
        LocalizableString messageFormat,
        string category,
        DiagnosticSeverity defaultSeverity,
        bool isEnabledByDefault,
        LocalizableString? description = null,
        string? helpLinkUri = null,
        params string[] customTags)
        : this(id, title, messageFormat, category, defaultSeverity, isEnabledByDefault, description, helpLinkUri, customTags.ToImmutableArray())
    {
    }

    internal DiagnosticDescriptor(
        string id,
        LocalizableString title,
        LocalizableString messageFormat,
        string category,
        DiagnosticSeverity defaultSeverity,
        bool isEnabledByDefault,
        LocalizableString? description,
        string? helpLinkUri,
        ImmutableArray<string> customTags)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("DiagnosticId cannot be null or whitespace", nameof(id));
        }

        Id = id;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Category = category ?? throw new ArgumentNullException(nameof(category));
        MessageFormat = messageFormat ?? throw new ArgumentNullException(nameof(messageFormat));
        DefaultSeverity = defaultSeverity;
        IsEnabledByDefault = isEnabledByDefault;
        Description = description ?? string.Empty;
        HelpLinkUri = helpLinkUri ?? string.Empty;
        CustomTags = customTags;
    }

    public bool Equals(DiagnosticDescriptor? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return
            other != null &&
            Category == other.Category &&
            DefaultSeverity == other.DefaultSeverity &&
            Description.Equals(other.Description) &&
            HelpLinkUri == other.HelpLinkUri &&
            Id == other.Id &&
            IsEnabledByDefault == other.IsEnabledByDefault &&
            MessageFormat.Equals(other.MessageFormat) &&
            Title.Equals(other.Title);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as DiagnosticDescriptor);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Category,
            DefaultSeverity,
            Description,
            HelpLinkUri,
            Id,
            IsEnabledByDefault,
            MessageFormat,
            Title);
    }
}
