using WarHub.ArmouryModel.Source;
using static WarHub.ArmouryModel.Source.NodeFactory;

namespace WarHub.ArmouryModel.EditorServices;

public static class RosterOperations
{
    public static IRosterOperation Identity { get; } = new LambdaOperation(x => x);


}

public abstract record OperationBuilderBase
{
    protected abstract RosterOperationKind Kind { get; }

    protected virtual bool ShouldRecalculateRosterCosts => true;

    protected IRosterOperation Operation(Func<RosterNode, RosterNode> rosterTransform)
    {
        // TODO add diagnostics for the new roster state
        return new LambdaOperation(state => Update(state, rosterTransform));
    }

    protected virtual RosterState Update(RosterState state, Func<RosterNode, RosterNode> rosterTransform)
    {
        var transformed = rosterTransform(state.Roster);
        var updated = ShouldRecalculateRosterCosts ? transformed.WithUpdatedCostTotals() : transformed;
        return state with { Roster = updated };
    }
}

public record CreateRosterOperationBuilder(Dataset Dataset)
{
    public RosterOperationBuilder WithName(string rosterName)
    {
        // TODO add game system cost types to cost limits
        var roster = NodeFactory.Roster(Dataset.Gamesystem, rosterName)
            .WithCostLimits(Dataset.Gamesystem.CostTypes
                .Select(costType => CostLimit(costType, costType.DefaultCostLimit))
                .ToArray())
            .WithCosts(Dataset.Gamesystem.CostTypes
                .Select(costType => Cost(costType, 0m))
                .ToArray());
        return new RosterOperationBuilder(new RosterState(Dataset) { Roster = roster });
    }
}

public record ChangeCostLimitOperationBuilder(CostLimitNode CostLimit) : OperationBuilderBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.ModifyCostLimits;

    public IRosterOperation To(decimal newLimit) => Operation(roster =>
    {
        return roster.WithCostLimits(roster.CostLimits.Replace(x => x == CostLimit, x => x.WithValue(newLimit)));
    });
}

public record AddForceOperationBuilder(ForceEntryNode ForceEntry) : OperationBuilderBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.AddForce;

    public IRosterOperation ToRoot() => Operation(roster =>
    {
            // TODO add categories, rules, profiles, catalogue cost types to cost limits
            return roster.AddForces(Force(ForceEntry));
    });
}

public record RemoveForceOperationBuilder(ForceNode Force) : OperationBuilderBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.RemoveForce;

    public IRosterOperation Build() => Operation(roster =>
    {
        return roster.Remove(x => x == Force)!;
    });
}

public record AddSelectionOperationBuilder(SelectionEntryNode SelectionEntry) : OperationBuilderBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.AddSelection;

    public IRosterOperation To(ForceNode force) => Operation(roster =>
    {
        var selectionEntryId = SelectionEntry.Id;
        if (selectionEntryId is null)
            return roster; // TODO add diagnostic invalid data
            var selection =
        Selection(SelectionEntry, selectionEntryId)
        .AddCosts(SelectionEntry.Costs);
            // TODO add selection categories, rules, profiles
            // TODO add subselections
            return roster.WithForces(roster.Forces.Replace(x => x == force, x => x.AddSelections(selection)));
    });
}

public record RemoveSelectionOperationBuilder(SelectionNode Selection) : OperationBuilderBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.RemoveSelection;

    public IRosterOperation Build() => Operation(roster =>
    {
        return roster.Remove(x => x == Selection)!;
    });
}

public record ChangeSelectionCountOperationBuilder(SelectionNode Selection) : OperationBuilderBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.ModifySelectionCount;

    public IRosterOperation To(int newCount) => Operation(roster =>
    {
            // TODO subselections (collective?)
            return roster.Replace(x => x == Selection, x => ((SelectionNode)x).WithUpdatedNumberAndCosts(newCount))!;
    });
}
