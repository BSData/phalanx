namespace WarHub.ArmouryModel;

/// <summary>
/// Describes how to report a warning diagnostic.
/// </summary>
public enum ReportDiagnostic
{
    /// <summary>
    /// Report a diagnostic by default.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Report a diagnostic as an error.
    /// </summary>
    Error = 1,

    /// <summary>
    /// Report a diagnostic as a warning even though /warnaserror is specified.
    /// </summary>
    Warn = 2,

    /// <summary>
    /// Report a diagnostic as an info.
    /// </summary>
    Info = 3,

    /// <summary>
    /// Report a diagnostic as hidden.
    /// </summary>
    Hidden = 4,

    /// <summary>
    /// Suppress a diagnostic.
    /// </summary>
    Suppress = 5,
}
