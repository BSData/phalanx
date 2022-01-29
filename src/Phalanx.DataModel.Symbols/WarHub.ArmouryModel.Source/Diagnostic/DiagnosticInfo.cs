using System.Diagnostics;
using System.Globalization;
using Phalanx.DataModel.Symbols;

namespace WarHub.ArmouryModel.Source;

/// <summary>
/// A DiagnosticInfo object has information about a diagnostic, but without any attached location information.
/// </summary>
/// <remarks>
/// More specialized diagnostics with additional information (e.g., ambiguity errors) can derive from this class to
/// provide access to additional information about the error, such as what symbols were involved in the ambiguity.
/// </remarks>
[DebuggerDisplay("{GetDebuggerDisplay(), nq}")]
internal class DiagnosticInfo : IFormattable
{
    private static ImmutableDictionary<int, DiagnosticDescriptor> errorCodeToDescriptorMap = ImmutableDictionary<int, DiagnosticDescriptor>.Empty;

    // Mark compiler errors as non-configurable to ensure they can never be suppressed or filtered.
    private static readonly ImmutableArray<string> compilerErrorCustomTags = ImmutableArray.Create(WellKnownDiagnosticTags.Compiler, WellKnownDiagnosticTags.Telemetry, WellKnownDiagnosticTags.NotConfigurable);
    private static readonly ImmutableArray<string> compilerNonErrorCustomTags = ImmutableArray.Create(WellKnownDiagnosticTags.Compiler, WellKnownDiagnosticTags.Telemetry);

    // Only the compiler creates instances.
    internal DiagnosticInfo(CommonMessageProvider messageProvider, int errorCode)
    {
        MessageProvider = messageProvider;
        Code = errorCode;
        DefaultSeverity = messageProvider.GetSeverity(errorCode);
        Severity = DefaultSeverity;
        Arguments = Array.Empty<object>();
    }

    // Only the compiler creates instances.
    internal DiagnosticInfo(CommonMessageProvider messageProvider, int errorCode, params object[] arguments)
        : this(messageProvider, errorCode)
    {
        Arguments = arguments;
    }

    protected DiagnosticInfo(DiagnosticInfo original, DiagnosticSeverity overriddenSeverity)
    {
        MessageProvider = original.MessageProvider;
        Code = original.Code;
        DefaultSeverity = original.DefaultSeverity;
        Arguments = original.Arguments;

        Severity = overriddenSeverity;
    }

    internal static DiagnosticDescriptor GetDescriptor(int errorCode, CommonMessageProvider messageProvider)
    {
        var defaultSeverity = messageProvider.GetSeverity(errorCode);
        return GetOrCreateDescriptor(errorCode, defaultSeverity, messageProvider);
    }

    private static DiagnosticDescriptor GetOrCreateDescriptor(int errorCode, DiagnosticSeverity defaultSeverity, CommonMessageProvider messageProvider)
    {
        return ImmutableInterlocked.GetOrAdd(ref errorCodeToDescriptorMap, errorCode, code => CreateDescriptor(code, defaultSeverity, messageProvider));
    }

    private static DiagnosticDescriptor CreateDescriptor(int errorCode, DiagnosticSeverity defaultSeverity, CommonMessageProvider messageProvider)
    {
        var id = messageProvider.GetIdForErrorCode(errorCode);
        var title = messageProvider.GetTitle(errorCode);
        var description = messageProvider.GetDescription(errorCode);
        var messageFormat = messageProvider.GetMessageFormat(errorCode);
        var helpLink = messageProvider.GetHelpLink(errorCode);
        var category = messageProvider.GetCategory(errorCode);
        var customTags = GetCustomTags(defaultSeverity);
        return new DiagnosticDescriptor(id, title, messageFormat, category, defaultSeverity,
            isEnabledByDefault: true, description: description, helpLinkUri: helpLink, customTags: customTags);
    }

    // Only the compiler creates instances.
    internal DiagnosticInfo(CommonMessageProvider messageProvider, bool isWarningAsError, int errorCode, params object[] arguments)
        : this(messageProvider, errorCode, arguments)
    {
        Debug.Assert(!isWarningAsError || DefaultSeverity == DiagnosticSeverity.Warning);

        if (isWarningAsError)
        {
            Severity = DiagnosticSeverity.Error;
        }
    }

    // Create a copy of this instance with a explicit overridden severity
    internal virtual DiagnosticInfo GetInstanceWithSeverity(DiagnosticSeverity severity)
    {
        return new DiagnosticInfo(this, severity);
    }

    /// <summary>
    /// The error code, as an integer.
    /// </summary>
    public int Code { get; }

    public virtual DiagnosticDescriptor Descriptor =>
        GetOrCreateDescriptor(Code, DefaultSeverity, MessageProvider);

    /// <summary>
    /// Returns the effective severity of the diagnostic: whether this diagnostic is informational, warning, or error.
    /// If IsWarningsAsError is true, then this returns <see cref="DiagnosticSeverity.Error"/>, while <see cref="DefaultSeverity"/> returns <see cref="DiagnosticSeverity.Warning"/>.
    /// </summary>
    public DiagnosticSeverity Severity { get; }

    /// <summary>
    /// Returns whether this diagnostic is informational, warning, or error by default, based on the error code.
    /// To get diagnostic's effective severity, use <see cref="Severity"/>.
    /// </summary>
    public DiagnosticSeverity DefaultSeverity { get; }

    /// <summary>
    /// Gets the warning level. This is 0 for diagnostics with severity <see cref="DiagnosticSeverity.Error"/>,
    /// otherwise an integer greater than zero.
    /// </summary>
    public int WarningLevel =>
        Severity != DefaultSeverity
            ? Diagnostic.GetDefaultWarningLevel(Severity)
            : MessageProvider.GetWarningLevel(Code);

    /// <summary>
    /// Returns true if this is a warning treated as an error.
    /// </summary>
    /// <remarks>
    /// True implies <see cref="Severity"/> = <see cref="DiagnosticSeverity.Error"/> and
    /// <see cref="DefaultSeverity"/> = <see cref="DiagnosticSeverity.Warning"/>.
    /// </remarks>
    public bool IsWarningAsError =>
        DefaultSeverity == DiagnosticSeverity.Warning &&
        Severity == DiagnosticSeverity.Error;

    /// <summary>
    /// Get the diagnostic category for the given diagnostic code.
    /// Default category is <see cref="Diagnostic.CompilerDiagnosticCategory"/>.
    /// </summary>
    public string Category => MessageProvider.GetCategory(Code);

    internal ImmutableArray<string> CustomTags => GetCustomTags(DefaultSeverity);

    private static ImmutableArray<string> GetCustomTags(DiagnosticSeverity defaultSeverity)
    {
        return defaultSeverity == DiagnosticSeverity.Error ?
            compilerErrorCustomTags :
            compilerNonErrorCustomTags;
    }

    internal bool IsNotConfigurable()
    {
        // Only compiler errors are non-configurable.
        return DefaultSeverity == DiagnosticSeverity.Error;
    }

    /// <summary>
    /// If a derived class has additional information about other referenced symbols, it can
    /// expose the locations of those symbols in a general way, so they can be reported along
    /// with the error.
    /// </summary>
    public virtual IReadOnlyList<Location> AdditionalLocations => Array.Empty<Location>();

    /// <summary>
    /// Get the message id (for example "CS1001") for the message. This includes both the error number
    /// and a prefix identifying the source.
    /// </summary>
    public virtual string MessageIdentifier => MessageProvider.GetIdForErrorCode(Code);

    /// <summary>
    /// Get the text of the message in the given language.
    /// </summary>
    public virtual string GetMessage(IFormatProvider? formatProvider = null)
    {
        // Get the message and fill in arguments.
        var message = MessageProvider.LoadMessage(Code, formatProvider as CultureInfo);
        if (string.IsNullOrEmpty(message))
        {
            return string.Empty;
        }

        if (Arguments.Length == 0)
        {
            return message;
        }

        return string.Format(formatProvider, message, GetArgumentsToUse(formatProvider));
    }

    protected object[] GetArgumentsToUse(IFormatProvider? formatProvider)
    {
        object[]? argumentsToUse = null;
        for (var i = 0; i < Arguments.Length; i++)
        {
            if (Arguments[i] is DiagnosticInfo embedded)
            {
                argumentsToUse = InitializeArgumentListIfNeeded(argumentsToUse);
                argumentsToUse[i] = embedded.GetMessage(formatProvider);
                continue;
            }

            if (Arguments[i] is ISymbol symbol)
            {
                argumentsToUse = InitializeArgumentListIfNeeded(argumentsToUse);
                argumentsToUse[i] = MessageProvider.GetErrorDisplayString(symbol);
            }
        }

        return argumentsToUse ?? Arguments;
    }

    private object[] InitializeArgumentListIfNeeded(object[]? argumentsToUse)
    {
        if (argumentsToUse != null)
        {
            return argumentsToUse;
        }

        var newArguments = new object[Arguments.Length];
        Array.Copy(Arguments, newArguments, newArguments.Length);

        return newArguments;
    }

    internal object[] Arguments { get; }

    internal CommonMessageProvider MessageProvider { get; }

    public override string? ToString() => ToString(null);

    public string ToString(IFormatProvider? formatProvider)
    {
        return ((IFormattable)this).ToString(null, formatProvider);
    }

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
    {
        return string.Format(formatProvider, "{0}: {1}",
            MessageProvider.GetMessagePrefix(MessageIdentifier, Severity, IsWarningAsError, formatProvider as CultureInfo),
            GetMessage(formatProvider));
    }

    public sealed override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Code);
        for (var i = 0; i < Arguments.Length; i++)
        {
            hashCode.Add(Arguments[i]);
        }

        return hashCode.ToHashCode();
    }

    public sealed override bool Equals(object? obj)
    {
        var result = false;

        if (obj is DiagnosticInfo other &&
            other.Code == Code &&
            other.GetType() == GetType())
        {
            if (Arguments.Length == other.Arguments.Length)
            {
                result = true;
                for (var i = 0; i < Arguments.Length; i++)
                {
                    if (!object.Equals(Arguments[i], other.Arguments[i]))
                    {
                        result = false;
                        break;
                    }
                }
            }
        }

        return result;
    }

    private string? GetDebuggerDisplay()
    {
        return ToString();
    }
}
