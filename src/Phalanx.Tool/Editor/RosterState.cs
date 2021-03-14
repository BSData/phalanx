using System.Collections.Immutable;
using WarHub.ArmouryModel.Source;

namespace Phalanx.Tool.Editor
{
    public record RosterState(Dataset Dataset)
    {
        public RosterNode Roster { get; init; } = RosterCore.Empty.ToNode();

        public ImmutableArray<RosterDiagnostic> Diagnostics { get; init; } =
            ImmutableArray<RosterDiagnostic>.Empty;
    }
}
