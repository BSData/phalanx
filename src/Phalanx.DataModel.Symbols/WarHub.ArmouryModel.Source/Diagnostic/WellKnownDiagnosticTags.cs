namespace WarHub.ArmouryModel.Source;

public static class WellKnownDiagnosticTags
{
    /// <summary>
    /// Indicates that the diagnostic is related to some unnecessary source code.
    /// </summary>
    public const string Unnecessary = nameof(Unnecessary);

    /// <summary>
    /// Indicates that the diagnostic is related to build.
    /// </summary>
    public const string Build = nameof(Build);

    /// <summary>
    /// Indicates that the diagnostic is reported by the compiler.
    /// </summary>
    public const string Compiler = nameof(Compiler);

    /// <summary>
    /// Indicates that the diagnostic can be used for telemetry
    /// </summary>
    public const string Telemetry = nameof(Telemetry);

    /// <summary>
    /// Indicates that the diagnostic is not configurable, i.e. it cannot be suppressed or filtered or have its severity changed.
    /// </summary>
    public const string NotConfigurable = nameof(NotConfigurable);

    /// <summary>
    /// Indicates that the diagnostic is a compilation end diagnostic reported
    /// from a compilation end action.
    /// </summary>
    public const string CompilationEnd = nameof(CompilationEnd);
}
