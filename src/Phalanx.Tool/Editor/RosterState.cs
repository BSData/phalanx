using System.Collections.Immutable;
using WarHub.ArmouryModel.Source;

namespace Phalanx.Tool.Editor;

/// <summary>
/// Represents a point-in-time roster together with dataset used to create the roster and roster diagnostics.
/// Diagnostics will contain warnings and errors to show to the user.
/// </summary>
public record RosterState(Dataset Dataset)
{
    public RosterNode Roster { get; init; } = RosterCore.Empty.ToNode();

    public ImmutableArray<RosterDiagnostic> Diagnostics { get; init; } =
        ImmutableArray<RosterDiagnostic>.Empty;
}
