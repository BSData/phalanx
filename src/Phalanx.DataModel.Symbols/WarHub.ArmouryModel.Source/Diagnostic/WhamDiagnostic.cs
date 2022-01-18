namespace WarHub.ArmouryModel.Source;

/// <summary>
/// A diagnostic, along with the location where it occurred.
/// </summary>
internal sealed class WhamDiagnostic : DiagnosticWithInfo
{
    internal WhamDiagnostic(DiagnosticInfo info, Location location, bool isSuppressed = false)
        : base(info, location, isSuppressed)
    {
    }

    public override string ToString() =>
        DiagnosticFormatter.Instance.Format(this);

    internal override Diagnostic WithLocation(Location location)
    {
        if (location == null)
        {
            throw new ArgumentNullException(nameof(location));
        }

        if (location != Location)
        {
            return new WhamDiagnostic(Info, location, IsSuppressed);
        }

        return this;
    }

    internal override Diagnostic WithSeverity(DiagnosticSeverity severity)
    {
        if (Severity != severity)
        {
            return new WhamDiagnostic(Info.GetInstanceWithSeverity(severity), Location, IsSuppressed);
        }

        return this;
    }

    internal override Diagnostic WithIsSuppressed(bool isSuppressed)
    {
        if (IsSuppressed != isSuppressed)
        {
            return new WhamDiagnostic(Info, Location, isSuppressed);
        }

        return this;
    }
}
