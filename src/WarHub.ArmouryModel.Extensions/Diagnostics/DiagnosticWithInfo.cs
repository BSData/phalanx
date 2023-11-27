using System.Diagnostics;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel;

/// <summary>
/// A diagnostic (such as a compiler error or a warning), along with the location where it occurred.
/// </summary>
[DebuggerDisplay("{GetDebuggerDisplay(), nq}")]
internal class DiagnosticWithInfo : Diagnostic
{
    private readonly Location location;
    private readonly bool isSuppressed;

    internal DiagnosticWithInfo(DiagnosticInfo info, Location location, bool isSuppressed = false)
    {
        Info = info;
        this.location = location;
        this.isSuppressed = isSuppressed;
    }

    public override Location Location => location;

    public override IReadOnlyList<Location> AdditionalLocations =>
        Info.AdditionalLocations;

    internal override ImmutableArray<string> CustomTags => Info.CustomTags;

    public override DiagnosticDescriptor Descriptor => Info.Descriptor;

    public override string Id => Info.MessageIdentifier;

    internal override string Category => Info.Category;

    public sealed override DiagnosticSeverity Severity => Info.Severity;

    public sealed override DiagnosticSeverity DefaultSeverity => Info.DefaultSeverity;

    internal sealed override bool IsEnabledByDefault => true;

    public override bool IsSuppressed => isSuppressed;

    public sealed override int WarningLevel => Info.WarningLevel;

    public override string GetMessage(IFormatProvider? formatProvider = null) =>
        Info.GetMessage(formatProvider);

    /// <summary>
    /// Get the information about the diagnostic: the code, severity, message, etc.
    /// </summary>
    public DiagnosticInfo Info { get; }

    public override int GetHashCode()
    {
        return HashCode.Combine(Location.GetHashCode(), Info.GetHashCode());
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Diagnostic);
    }

    public override bool Equals(Diagnostic? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not DiagnosticWithInfo other || GetType() != other.GetType())
        {
            return false;
        }

        return
            location.Equals(other.location) &&
            Info.Equals(other.Info) &&
            AdditionalLocations.SequenceEqual(other.AdditionalLocations);
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }

    internal override Diagnostic WithLocation(Location location)
    {
        ArgumentNullException.ThrowIfNull(location);

        if (location != this.location)
        {
            return new DiagnosticWithInfo(Info, location, isSuppressed);
        }

        return this;
    }

    internal override Diagnostic WithSeverity(DiagnosticSeverity severity)
    {
        if (Severity != severity)
        {
            return new DiagnosticWithInfo(Info.GetInstanceWithSeverity(severity), location, isSuppressed);
        }

        return this;
    }

    internal override Diagnostic WithIsSuppressed(bool isSuppressed)
    {
        if (IsSuppressed != isSuppressed)
        {
            return new DiagnosticWithInfo(Info, location, isSuppressed);
        }

        return this;
    }
}
