using System;

namespace Phalanx.Tool.Editor;

/// <summary>
/// A roster operation that wraps a lambda transformation.
/// </summary>
public record LambdaOperation(Func<RosterState, RosterState> Transformation)
    : IRosterOperation
{
    public RosterState Apply(RosterState baseState) => Transformation(baseState);
}
