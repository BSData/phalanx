using System;

namespace Phalanx.Tool.Editor
{
    public delegate RosterState RosterStateTransformation(RosterState initialState);
    public record LambdaOperation(
        RosterOperationKind Kind,
        RosterState InitialState,
        Func<RosterState, RosterState> Transformation)
        : IRosterOperation
    {
        private RosterState? transformed;
        public RosterState Apply() => transformed ??= Transformation(InitialState);
    }
}
