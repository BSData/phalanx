using WarHub.ArmouryModel.Source;
using static WarHub.ArmouryModel.Source.NodeFactory;

namespace WarHub.ArmouryModel.EditorServices;

public static class RosterOperations
{
    public static IRosterOperation Identity => IdentityRosterOperation.Instance;

    public static CreateRosterOperation CreateRoster() => new();

    public static ChangeCostLimitOperation ChangeCostLimit(CostLimitNode costLimit, decimal newValue) =>
        new(costLimit.TypeId!, newValue);

    public static ChangeCostLimitOperation ChangeCostLimit(string typeId, decimal newValue) =>
        new(typeId, newValue);

    public static ChangeRosterNameOperation ChangeRosterName(string name) =>
        new(name);

    public static AddForceOperation AddForce(ForceEntryNode forceEntry) => new(forceEntry);

    public static AddSelectionOperation AddSelection(SelectionEntryNode selectionEntry, ForceNode force) =>
        new(selectionEntry, force);

    public static AddSelectionFromLinkOp AddSelectionFromLink(SelectionEntryNode selectionEntry, EntryLinkNode link, string force) =>
        new(selectionEntry, link, force);

    public static RemoveForceOperation RemoveForce(ForceNode force) => new(force);

    public static RemoveSelectionOperation RemoveSelection(SelectionNode selection) =>
        new(selection);

    public static ChangeSelectionCountOperation ChangeCountOf(SelectionNode selection, int newCount) =>
        new(selection, newCount);

    public static AddRootEntryFromSymbol AddRootEntryFromSymbol(ISelectionEntryContainerSymbol link, string force, int count = 1) =>
        new(link, force, count);
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

public record ChangeCostLimitOperation(string TypeId, decimal NewValue) : RosterOperationBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.ModifyCostLimits;

    protected override RosterNode TransformRoster(RosterState state)
    {
        var roster = state.RosterRequired;

        if (roster.CostLimits.FirstOrDefault(x => x.TypeId == TypeId) is { } costLimit)
        {
            return roster.Replace(costLimit, x => x.WithValue(NewValue));
        }
        var costType = state.Gamesystem.CostTypes.First(type => type.Id == TypeId);
        return roster.AddCostLimits(CostLimit(costType));
    }
}

public record ChangeRosterNameOperation(string Name) : RosterOperationBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.RenameRoster;

    protected override RosterNode TransformRoster(RosterState state)
    {
        var roster = state.RosterRequired;
        return roster.WithName(Name);
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

public record AddSelectionFromLinkOp(SelectionEntryNode SelectionEntry, EntryLinkNode EntryLink, string ForceId) : RosterOperationBase
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
            .AddCosts(EntryLink.Costs);
        // .WithCategories(CategoryList( catNodes));
        // TODO add selection categories, rules, profiles
        // TODO add subselections

        // Use ID because the ForceNode object becomes invalid after other operations,
        // such as a prior selectionAdd
        var force = roster.Forces.FirstOrDefault(f => f.Id == ForceId);

        if (force != null)
            return roster.Replace(force, x => x.AddSelections(selection));
        else
            return roster;
    }
}

public record AddRootEntryFromSymbol(ISelectionEntryContainerSymbol Entry, string ForceId, int Count = 1) : RosterOperationBase
{
    protected override RosterOperationKind Kind => RosterOperationKind.AddSelection;

    protected override RosterNode TransformRoster(RosterState state)
    {
        var roster = state.RosterRequired;
        // hack for referencing symbols from previous compilations, until SymbolReference is done
        var entryLocal = state.Compilation.GlobalNamespace.Catalogues
            .First(x => x.Id == Entry.ContainingModule!.Id)
            .RootContainerEntries
            .Where(x => x.IsContainerKind(ContainerKind.Selection))
            .OfType<ISelectionEntryContainerSymbol>()
            .First(x => x.Id == Entry.Id);
        var entries = !entryLocal.IsReference
            ? new[] { entryLocal }
            : new[] { entryLocal, entryLocal.ReferencedEntry! };
        // TODO how does BS work when Link declares costs as well as the Target entry?
        var costNodes = entries
            .SelectMany(x => x.Costs)
            .Where(x => x.Value > 0 && x.Type?.Id is not null)
            .Select(x => Cost(x.Name, x.Type!.Id, x.Value))
            .ToList();
        // TODO handle primary set in both link and target entry, deduplicate categories
        var catList = entries
            .SelectMany(x => x.Categories)
            .Select(x => Category(x.ReferencedEntry!.GetEntryDeclaration()!, x.ReferencedEntry!.Id).WithPrimary(x.IsPrimaryCategory))
            .ToList();
        var selectionEntryNode = (entryLocal.IsReference ? entryLocal.ReferencedEntry! : entryLocal).GetEntryDeclaration()!;
        for (var i = 0; i < Count; i++)
        {
            // Use ID because the ForceNode object becomes invalid after other operations,
            // such as a prior selectionAdd
            var force = roster.Forces.First(f => f.Id == ForceId);
            var selection =
                Selection(selectionEntryNode, selectionEntryNode.Id!)
                .AddCosts(costNodes)
                .AddCategories(catList);

            roster = roster.Replace(force, x => x.AddSelections(selection));
        }
        return roster;
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
