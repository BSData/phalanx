using System.Globalization;

namespace WarHub.ArmouryModel.Source;

/// <summary>
/// Represents a diagnostic, such as a compiler error or a warning, along with the location where it occurred.
/// </summary>
public abstract partial class Diagnostic : IEquatable<Diagnostic?>, IFormattable
{
    internal const string CompilerDiagnosticCategory = "Compiler";

    /// <summary>
    /// The default warning level, which is also used for non-error diagnostics.
    /// </summary>
    internal const int DefaultWarningLevel = 4;

    /// <summary>
    /// The warning level used for hidden and info diagnostics. Because these diagnostics interact with other editor features, we want them to always be produced unless /warn:0 is set.
    /// </summary>
    internal const int InfoAndHiddenWarningLevel = 1;

    /// <summary>
    /// The maximum warning level represented by a large value of 9999.
    /// </summary>
    internal const int MaxWarningLevel = 9999;

    /// <summary>
    /// Creates a <see cref="Diagnostic"/> instance.
    /// </summary>
    /// <param name="descriptor">A <see cref="DiagnosticDescriptor"/> describing the diagnostic</param>
    /// <param name="location">An optional primary location of the diagnostic. If null, <see cref="Location"/> will return <see cref="Location.None"/>.</param>
    /// <param name="messageArgs">Arguments to the message of the diagnostic</param>
    /// <returns>The <see cref="Diagnostic"/> instance.</returns>
    public static Diagnostic Create(
        DiagnosticDescriptor descriptor,
        Location? location,
        params object?[]? messageArgs)
    {
        return Create(descriptor, location, null, null, messageArgs);
    }

    /// <summary>
    /// Creates a <see cref="Diagnostic"/> instance.
    /// </summary>
    /// <param name="descriptor">A <see cref="DiagnosticDescriptor"/> describing the diagnostic.</param>
    /// <param name="location">An optional primary location of the diagnostic. If null, <see cref="Location"/> will return <see cref="Location.None"/>.</param>
    /// <param name="properties">
    /// An optional set of name-value pairs by means of which the analyzer that creates the diagnostic
    /// can convey more detailed information to the fixer. If null, <see cref="Properties"/> will return
    /// <see cref="ImmutableDictionary{TKey, TValue}.Empty"/>.
    /// </param>
    /// <param name="messageArgs">Arguments to the message of the diagnostic.</param>
    /// <returns>The <see cref="Diagnostic"/> instance.</returns>
    public static Diagnostic Create(
        DiagnosticDescriptor descriptor,
        Location? location,
        ImmutableDictionary<string, string?>? properties,
        params object?[]? messageArgs)
    {
        return Create(descriptor, location, null, properties, messageArgs);
    }

    /// <summary>
    /// Creates a <see cref="Diagnostic"/> instance.
    /// </summary>
    /// <param name="descriptor">A <see cref="DiagnosticDescriptor"/> describing the diagnostic.</param>
    /// <param name="location">An optional primary location of the diagnostic. If null, <see cref="Location"/> will return <see cref="Location.None"/>.</param>
    /// <param name="additionalLocations">
    /// An optional set of additional locations related to the diagnostic.
    /// Typically, these are locations of other items referenced in the message.
    /// If null, <see cref="AdditionalLocations"/> will return an empty list.
    /// </param>
    /// <param name="messageArgs">Arguments to the message of the diagnostic.</param>
    /// <returns>The <see cref="Diagnostic"/> instance.</returns>
    public static Diagnostic Create(
        DiagnosticDescriptor descriptor,
        Location? location,
        IEnumerable<Location>? additionalLocations,
        params object?[]? messageArgs)
    {
        return Create(descriptor, location, additionalLocations, properties: null, messageArgs: messageArgs);
    }

    /// <summary>
    /// Creates a <see cref="Diagnostic"/> instance.
    /// </summary>
    /// <param name="descriptor">A <see cref="DiagnosticDescriptor"/> describing the diagnostic.</param>
    /// <param name="location">An optional primary location of the diagnostic. If null, <see cref="Location"/> will return <see cref="Location.None"/>.</param>
    /// <param name="additionalLocations">
    /// An optional set of additional locations related to the diagnostic.
    /// Typically, these are locations of other items referenced in the message.
    /// If null, <see cref="AdditionalLocations"/> will return an empty list.
    /// </param>
    /// <param name="properties">
    /// An optional set of name-value pairs by means of which the analyzer that creates the diagnostic
    /// can convey more detailed information to the fixer. If null, <see cref="Properties"/> will return
    /// <see cref="ImmutableDictionary{TKey, TValue}.Empty"/>.
    /// </param>
    /// <param name="messageArgs">Arguments to the message of the diagnostic.</param>
    /// <returns>The <see cref="Diagnostic"/> instance.</returns>
    public static Diagnostic Create(
        DiagnosticDescriptor descriptor,
        Location? location,
        IEnumerable<Location>? additionalLocations,
        ImmutableDictionary<string, string?>? properties,
        params object?[]? messageArgs)
    {
        return Create(descriptor, location, effectiveSeverity: descriptor.DefaultSeverity, additionalLocations, properties, messageArgs);
    }

    /// <summary>
    /// Creates a <see cref="Diagnostic"/> instance.
    /// </summary>
    /// <param name="descriptor">A <see cref="DiagnosticDescriptor"/> describing the diagnostic.</param>
    /// <param name="location">An optional primary location of the diagnostic. If null, <see cref="Location"/> will return <see cref="Location.None"/>.</param>
    /// <param name="effectiveSeverity">Effective severity of the diagnostic.</param>
    /// <param name="additionalLocations">
    /// An optional set of additional locations related to the diagnostic.
    /// Typically, these are locations of other items referenced in the message.
    /// If null, <see cref="AdditionalLocations"/> will return an empty list.
    /// </param>
    /// <param name="properties">
    /// An optional set of name-value pairs by means of which the analyzer that creates the diagnostic
    /// can convey more detailed information to the fixer. If null, <see cref="Properties"/> will return
    /// <see cref="ImmutableDictionary{TKey, TValue}.Empty"/>.
    /// </param>
    /// <param name="messageArgs">Arguments to the message of the diagnostic.</param>
    /// <returns>The <see cref="Diagnostic"/> instance.</returns>
    public static Diagnostic Create(
        DiagnosticDescriptor descriptor,
        Location? location,
        DiagnosticSeverity effectiveSeverity,
        IEnumerable<Location>? additionalLocations,
        ImmutableDictionary<string, string?>? properties,
        params object?[]? messageArgs)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        var warningLevel = GetDefaultWarningLevel(effectiveSeverity);
        return SimpleDiagnostic.Create(
            descriptor,
            severity: effectiveSeverity,
            warningLevel: warningLevel,
            location: location ?? Location.None,
            additionalLocations: additionalLocations,
            messageArgs: messageArgs,
            properties: properties);
    }

    /// <summary>
    /// Creates a <see cref="Diagnostic"/> instance which is localizable.
    /// </summary>
    /// <param name="id">An identifier for the diagnostic. For diagnostics generated by the compiler, this will be a numeric code with a prefix such as "CS1001".</param>
    /// <param name="category">The category of the diagnostic. For diagnostics generated by the compiler, the category will be "Compiler".</param>
    /// <param name="message">The diagnostic message text.</param>
    /// <param name="severity">The diagnostic's effective severity.</param>
    /// <param name="defaultSeverity">The diagnostic's default severity.</param>
    /// <param name="isEnabledByDefault">True if the diagnostic is enabled by default</param>
    /// <param name="warningLevel">The warning level, greater than 0 if severity is <see cref="DiagnosticSeverity.Warning"/>; otherwise 0.</param>
    /// <param name="title">An optional short localizable title describing the diagnostic.</param>
    /// <param name="description">An optional longer localizable description for the diagnostic.</param>
    /// <param name="helpLink">An optional hyperlink that provides more detailed information regarding the diagnostic.</param>
    /// <param name="location">An optional primary location of the diagnostic. If null, <see cref="Location"/> will return <see cref="Location.None"/>.</param>
    /// <param name="additionalLocations">
    /// An optional set of additional locations related to the diagnostic.
    /// Typically, these are locations of other items referenced in the message.
    /// If null, <see cref="AdditionalLocations"/> will return an empty list.
    /// </param>
    /// <param name="customTags">
    /// An optional set of custom tags for the diagnostic. See <see cref="WellKnownDiagnosticTags"/> for some well known tags.
    /// If null, <see cref="CustomTags"/> will return an empty list.
    /// </param>
    /// <param name="properties">
    /// An optional set of name-value pairs by means of which the analyzer that creates the diagnostic
    /// can convey more detailed information to the fixer. If null, <see cref="Properties"/> will return
    /// <see cref="ImmutableDictionary{TKey, TValue}.Empty"/>.
    /// </param>
    /// <returns>The <see cref="Diagnostic"/> instance.</returns>
    public static Diagnostic Create(
        string id,
        string category,
        LocalizableString message,
        DiagnosticSeverity severity,
        DiagnosticSeverity defaultSeverity,
        bool isEnabledByDefault,
        int warningLevel,
        LocalizableString? title = null,
        LocalizableString? description = null,
        string? helpLink = null,
        Location? location = null,
        IEnumerable<Location>? additionalLocations = null,
        IEnumerable<string>? customTags = null,
        ImmutableDictionary<string, string?>? properties = null)
    {
        return Create(id, category, message, severity, defaultSeverity, isEnabledByDefault, warningLevel, false,
            title, description, helpLink, location, additionalLocations, customTags, properties);
    }

    /// <summary>
    /// Creates a <see cref="Diagnostic"/> instance which is localizable.
    /// </summary>
    /// <param name="id">An identifier for the diagnostic. For diagnostics generated by the compiler, this will be a numeric code with a prefix such as "CS1001".</param>
    /// <param name="category">The category of the diagnostic. For diagnostics generated by the compiler, the category will be "Compiler".</param>
    /// <param name="message">The diagnostic message text.</param>
    /// <param name="severity">The diagnostic's effective severity.</param>
    /// <param name="defaultSeverity">The diagnostic's default severity.</param>
    /// <param name="isEnabledByDefault">True if the diagnostic is enabled by default</param>
    /// <param name="warningLevel">The warning level, greater than 0 if severity is <see cref="DiagnosticSeverity.Warning"/>; otherwise 0.</param>
    /// <param name="isSuppressed">Flag indicating whether the diagnostic is suppressed by a source suppression.</param>
    /// <param name="title">An optional short localizable title describing the diagnostic.</param>
    /// <param name="description">An optional longer localizable description for the diagnostic.</param>
    /// <param name="helpLink">An optional hyperlink that provides more detailed information regarding the diagnostic.</param>
    /// <param name="location">An optional primary location of the diagnostic. If null, <see cref="Location"/> will return <see cref="Location.None"/>.</param>
    /// <param name="additionalLocations">
    /// An optional set of additional locations related to the diagnostic.
    /// Typically, these are locations of other items referenced in the message.
    /// If null, <see cref="AdditionalLocations"/> will return an empty list.
    /// </param>
    /// <param name="customTags">
    /// An optional set of custom tags for the diagnostic. See <see cref="WellKnownDiagnosticTags"/> for some well known tags.
    /// If null, <see cref="CustomTags"/> will return an empty list.
    /// </param>
    /// <param name="properties">
    /// An optional set of name-value pairs by means of which the analyzer that creates the diagnostic
    /// can convey more detailed information to the fixer. If null, <see cref="Properties"/> will return
    /// <see cref="ImmutableDictionary{TKey, TValue}.Empty"/>.
    /// </param>
    /// <returns>The <see cref="Diagnostic"/> instance.</returns>
    public static Diagnostic Create(
        string id,
        string category,
        LocalizableString message,
        DiagnosticSeverity severity,
        DiagnosticSeverity defaultSeverity,
        bool isEnabledByDefault,
        int warningLevel,
        bool isSuppressed,
        LocalizableString? title = null,
        LocalizableString? description = null,
        string? helpLink = null,
        Location? location = null,
        IEnumerable<Location>? additionalLocations = null,
        IEnumerable<string>? customTags = null,
        ImmutableDictionary<string, string?>? properties = null)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(category);
        ArgumentNullException.ThrowIfNull(message);
        return SimpleDiagnostic.Create(id, title ?? string.Empty, category, message, description ?? string.Empty, helpLink ?? string.Empty,
            severity, defaultSeverity, isEnabledByDefault, warningLevel, location ?? Location.None, additionalLocations, customTags, properties, isSuppressed);
    }

    /// <summary>
    /// Gets the diagnostic descriptor, which provides a description about a <see cref="Diagnostic"/>.
    /// </summary>
    public abstract DiagnosticDescriptor Descriptor { get; }

    /// <summary>
    /// Gets the diagnostic identifier. For diagnostics generated by the compiler, this will be a numeric code with a prefix such as "CS1001".
    /// </summary>
    public abstract string Id { get; }

    /// <summary>
    /// Gets the category of diagnostic. For diagnostics generated by the compiler, the category will be "Compiler".
    /// </summary>
    internal virtual string Category => Descriptor.Category;

    /// <summary>
    /// Get the culture specific text of the message.
    /// </summary>
    public abstract string GetMessage(IFormatProvider? formatProvider = null);

    /// <summary>
    /// Gets the default <see cref="DiagnosticSeverity"/> of the diagnostic's <see cref="DiagnosticDescriptor"/>.
    /// </summary>
    /// <remarks>
    /// To get the effective severity of the diagnostic, use <see cref="Severity"/>.
    /// </remarks>
    public virtual DiagnosticSeverity DefaultSeverity => Descriptor.DefaultSeverity;

    /// <summary>
    /// Gets the effective <see cref="DiagnosticSeverity"/> of the diagnostic.
    /// </summary>
    /// <remarks>
    /// To get the default severity of diagnostic's <see cref="DiagnosticDescriptor"/>, use <see cref="DefaultSeverity"/>.
    /// To determine if this is a warning treated as an error, use <see cref="IsWarningAsError"/>.
    /// </remarks>
    public abstract DiagnosticSeverity Severity { get; }

    /// <summary>
    /// Gets the warning level. This is 0 for diagnostics with severity <see cref="DiagnosticSeverity.Error"/>,
    /// otherwise an integer greater than zero.
    /// </summary>
    public abstract int WarningLevel { get; }

    /// <summary>
    /// Returns true if the diagnostic has a source suppression, i.e. an attribute or a pragma suppression.
    /// </summary>
    public abstract bool IsSuppressed { get; }

    /// <summary>
    /// Returns true if this diagnostic is enabled by default by the author of the diagnostic.
    /// </summary>
    internal virtual bool IsEnabledByDefault => Descriptor.IsEnabledByDefault;

    /// <summary>
    /// Returns true if this is a warning treated as an error; otherwise false.
    /// </summary>
    /// <remarks>
    /// True implies <see cref="DefaultSeverity"/> = <see cref="DiagnosticSeverity.Warning"/>
    /// and <see cref="Severity"/> = <see cref="DiagnosticSeverity.Error"/>.
    /// </remarks>
    public bool IsWarningAsError =>
        DefaultSeverity == DiagnosticSeverity.Warning &&
        Severity == DiagnosticSeverity.Error;

    /// <summary>
    /// Gets the primary location of the diagnostic, or <see cref="Location.None"/> if no primary location.
    /// </summary>
    public abstract Location Location { get; }

    /// <summary>
    /// Gets an array of additional locations related to the diagnostic.
    /// Typically these are the locations of other items referenced in the message.
    /// </summary>
    public abstract IReadOnlyList<Location> AdditionalLocations { get; }

    /// <summary>
    /// Gets custom tags for the diagnostic.
    /// </summary>
    internal virtual ImmutableArray<string> CustomTags => Descriptor.ImmutableCustomTags;

    /// <summary>
    /// Gets property bag for the diagnostic. it will return <see cref="ImmutableDictionary{TKey, TValue}.Empty"/> 
    /// if there is no entry. This can be used to put diagnostic specific information you want 
    /// to pass around. for example, to corresponding fixer.
    /// </summary>
    public virtual ImmutableDictionary<string, string?> Properties
        => ImmutableDictionary<string, string?>.Empty;

    string IFormattable.ToString(string? ignored, IFormatProvider? formatProvider)
    {
        return DiagnosticFormatter.Instance.Format(this, formatProvider);
    }

    public override string ToString()
    {
#pragma warning disable CA1305 // Specify IFormatProvider // Warning: Method passes 'CultureInfo.CurrentUICulture' as the 'IFormatProvider' parameter. This property returns a culture that is inappropriate for formatting methods.
        return DiagnosticFormatter.Instance.Format(this, CultureInfo.CurrentUICulture);
#pragma warning restore CA1305 // Specify IFormatProvider
    }

    public abstract override bool Equals(object? obj);

    public abstract override int GetHashCode();

    public abstract bool Equals(Diagnostic? other);

    /// <summary>
    /// Create a new instance of this diagnostic with the Location property changed.
    /// </summary>
    internal abstract Diagnostic WithLocation(Location location);

    /// <summary>
    /// Create a new instance of this diagnostic with the Severity property changed.
    /// </summary>
    internal abstract Diagnostic WithSeverity(DiagnosticSeverity severity);

    /// <summary>
    /// Create a new instance of this diagnostic with the suppression info changed.
    /// </summary>
    internal abstract Diagnostic WithIsSuppressed(bool isSuppressed);

    /// <summary>
    /// Gets the default warning level for a diagnostic severity. Warning levels are used with the <c>/warn:N</c>
    /// command line option to suppress diagnostics over a severity of interest. When N is 0, only error severity
    /// messages are produced by the compiler. Values greater than 0 indicated that warnings up to and including
    /// level N should also be included.
    /// </summary>
    /// <remarks>
    /// <see cref="DiagnosticSeverity.Info"/> and <see cref="DiagnosticSeverity.Hidden"/> are treated as warning
    /// level 1. In other words, these diagnostics which typically interact with editor features are enabled unless
    /// the special <c>/warn:0</c> option is set.
    /// </remarks>
    /// <param name="severity">A <see cref="DiagnosticSeverity"/> value.</param>
    /// <returns>The default compiler warning level for <paramref name="severity"/>.</returns>
    internal static int GetDefaultWarningLevel(DiagnosticSeverity severity)
    {
        return severity switch
        {
            DiagnosticSeverity.Error => 0,
            _ => 1,
        };
    }
}
