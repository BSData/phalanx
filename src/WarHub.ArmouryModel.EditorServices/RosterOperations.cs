using WarHub.ArmouryModel.Source;
using static WarHub.ArmouryModel.Source.NodeFactory;

namespace WarHub.ArmouryModel.EditorServices;

public static class RosterOperations
{
    public static IRosterOperation Identity => IdentityRosterOperation.Instance;

    public static CreateRosterOperation CreateRoster() => new();

    public static ChangeCostLimitOperation ChangeCostLimit(CostLimitNode costLimit, decimal newValue) =>
        new(costLimit, newValue);

    public static AddForceOperation AddForce(ForceEntryNode forceEntry) => new(forceEntry);

    public static AddSelectionOperation AddSelection(SelectionEntryNode selectionEntry, ForceNode force) =>
        new(selectionEntry, force);

    public static RemoveForceOperation RemoveForce(ForceNode force) => new(force);

    public static RemoveSelectionOperation RemoveSelection(SelectionNode selection) =>
        new(selection);

    public static ChangeSelectionCountOperation ChangeCountOf(SelectionNode selection, int newCount) =>
        new(selection, newCount);

}

public class IdentityRosterOperation : IRosterOperation
{
    private IdentityRosterOperation() { }

    public static IdentityRosterOperation Instance { get; } = new();

    public RosterOperationKind Kind => RosterOperationKind.Identity;

    public RosterState Apply(RosterState baseState) => baseState;
}

public abstract record OperationBuilderBase
{
    protected abstract RosterOperationKind Kind { get; }

    protected virtual bool ShouldRecalculateRosterCosts => true;

    protected IRosterOperation Operation(Func<RosterNode, RosterNode> rosterTransform)
    {
        return new LambdaOperation(state => Update(state, rosterTransform));
    }

    protected virtual RosterState Update(RosterState state, Func<RosterNode, RosterNode> rosterTransform)
    {
        var transformed = rosterTransform(state.RosterRequired);
        var updated = ShouldRecalculateRosterCosts ? transformed.WithUpdatedCostTotals() : transformed;
        return state.ReplaceRoster(updated);
    }
}

public record RosterOperationBase : IRosterOperation
{
    RosterOperationKind IRosterOperation.Kind => Kind;

    protected virtual RosterOperationKind Kind => RosterOperationKind.Unknown;

    protected virtual RosterNode TransformRoster(RosterState state) => state.RosterRequired;

    RosterState IRosterOperation.Apply(RosterState baseState)
    {
        return baseState.ReplaceRoster(TransformRoster(baseState).WithUpdatedCostTotals());
    }
}

public record CreateRosterOperation : IRosterOperation
{
    public string? Name { get; init; }

    RosterState IRosterOperation.Apply(RosterState baseState)
    {
        var gamesystem = baseState.Gamesystem;
        var roster =
            Roster(gamesystem, Name)
            .WithCostLimits(gamesystem.CostTypes
                .Select(costType => CostLimit(costType, costType.DefaultCostLimit))
                .ToArray())
            .WithCosts(gamesystem.CostTypes
                .Select(costType => Cost(costType, 0m))
                .ToArray());
        return new RosterState(baseState.Compilation.AddSourceTrees(SourceTree.CreateForRoot(roster)));
    }
}

public record ChangeCostLimitOperation(CostLimitNode CostLimit, decimal NewValue) : RosterOperationBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.ModifyCostLimits;

    protected override RosterNode TransformRoster(RosterState state)
    {
        var roster = state.RosterRequired;
        return roster.Replace(roster.CostLimits.First(x => x.TypeId == CostLimit.TypeId), x => x.WithValue(NewValue));
    }
}

public record AddForceOperation(ForceEntryNode ForceEntry) : RosterOperationBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.AddForce;

    public Func<RosterNode, ForceNode>? SelectForceParent { get; init; }

    protected override RosterNode TransformRoster(RosterState state)
    {
        var roster = state.RosterRequired;
        // TODO add categories, rules, profiles, catalogue cost types to cost limits
        var force = Force(ForceEntry);
        return SelectForceParent is null
            ? roster.AddForces(force)
            : roster.ReplaceFluent(SelectForceParent, x => x.AddForces(force));
    }
}

public record RemoveForceOperation(ForceNode Force) : RosterOperationBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.RemoveForce;

    protected override RosterNode TransformRoster(RosterState state)
    {
        return state.RosterRequired.Remove(Force);
    }
}

public record AddSelectionOperation(SelectionEntryNode SelectionEntry, ForceNode Force) : RosterOperationBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.AddSelection;

    protected override RosterNode TransformRoster(RosterState state)
    {
        var roster = state.RosterRequired;
        var selectionEntryId = SelectionEntry.Id;
        if (selectionEntryId is null)
            return roster; // TODO add diagnostic invalid data
        var selection =
            Selection(SelectionEntry, selectionEntryId)
            .AddCosts(SelectionEntry.Costs);
        // TODO add selection categories, rules, profiles
        // TODO add subselections
        return roster.Replace(Force, x => x.AddSelections(selection));
    }
}

public record RemoveSelectionOperation(SelectionNode Selection) : RosterOperationBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.RemoveSelection;

    protected override RosterNode TransformRoster(RosterState state)
    {
        return state.RosterRequired.Remove(Selection);
    }
}

public record ChangeSelectionCountOperation(SelectionNode Selection, int NewCount) : RosterOperationBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.ModifySelectionCount;

    protected override RosterNode TransformRoster(RosterState state)
    {
        var roster = state.RosterRequired;
        // TODO subselections (collective?)
        return roster.Replace(Selection, x => x.WithUpdatedNumberAndCosts(NewCount))!;
    }
}
