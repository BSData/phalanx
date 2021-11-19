using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WarHub.ArmouryModel.Source;
using static WarHub.ArmouryModel.Source.NodeFactory;

namespace Phalanx.Tool.Editor;

public delegate TNode NodeSelector<TRoot, TNode>(TRoot root)
    where TRoot : IRootNode where TNode : SourceNode;

/// <summary>
/// Provides methods that change roster state.
/// </summary>
public record RosterEditor(RosterState State)
{
    public RosterNode Roster => State.Roster;
    public GamesystemNode Gamesystem => State.Dataset.Gamesystem;
    public ImmutableArray<CatalogueNode> Catalogues => State.Dataset.Catalogues;
    public ImmutableArray<RosterDiagnostic> Diagnostics => State.Diagnostics;
    public static CreateRosterOperationBuilder Create(Dataset dataset) => new(dataset);
    public ChangeCostLimitOperationBuilder ChangeCostLimit(CostLimitNode costLimit) => new(costLimit);
    public AddForceOperationBuilder AddForce(ForceEntryNode forceEntry) => new(forceEntry);
    public AddSelectionOperationBuilder AddSelection(SelectionEntryNode selectionEntry) => new(selectionEntry);
    public IRosterOperation RemoveForce(ForceNode force) => new RemoveForceOperationBuilder(force).Build();
    public IRosterOperation RemoveSelection(SelectionNode selection) => new RemoveSelectionOperationBuilder(selection).Build();
    public ChangeSelectionCountOperationBuilder ChangeCountOf(SelectionNode selection) => new(selection);

    // probably an extension method
    public RosterEditor Apply(Func<RosterEditor, IRosterOperation> operationSelector) =>
        this with { State = operationSelector(this).Apply(State) };

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
        public RosterEditor WithName(string rosterName)
        {
            // TODO add game system cost types to cost limits
            var roster = NodeFactory.Roster(Dataset.Gamesystem, rosterName)
                .WithCostLimits(Dataset.Gamesystem.CostTypes
                    .Select(costType => NodeFactory.CostLimit(costType, costType.DefaultCostLimit))
                    .ToArray())
                .WithCosts(Dataset.Gamesystem.CostTypes
                    .Select(costType => NodeFactory.Cost(costType, 0m))
                    .ToArray());
            return new RosterEditor(new RosterState(Dataset) { Roster = roster });
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
}

public static class SourceNodeExtensions
{
    public static NodeList<TNode> Replace<TNode>(this NodeList<TNode> nodes, TNode replaced, TNode inserted)
        where TNode : SourceNode
    {
        var array = nodes.ToList();
        array[array.IndexOf(replaced)] = inserted;
        return array.ToNodeList();
    }

    public static NodeList<TNode> Replace<TNode>(this ListNode<TNode> nodes, Func<TNode, bool> match, Func<TNode, TNode> transform)
        where TNode : SourceNode
    {
        return nodes.NodeList.Replace(match, transform);
    }

    public static NodeList<TNode> Replace<TNode>(this NodeList<TNode> nodes, Func<TNode, bool> match, Func<TNode, TNode> transform)
        where TNode : SourceNode
    {
        return nodes.Select(x => match(x) ? transform(x) : x).ToNodeList();
    }

    public static TNode? Replace<TNode>(this TNode root, Func<SourceNode, bool> match, Func<SourceNode, SourceNode?> transform)
        where TNode : SourceNode
    {
        var replacer = new NodeReplacer(match, transform);
        return (TNode?)replacer.Visit(root);
    }

    public static TNode? Remove<TNode>(this TNode root, Func<SourceNode, bool> match) where TNode : SourceNode
    {
        var remover = new NodeReplacer(match, x => null);
        return (TNode?)remover.Visit(root);
    }

    /// <summary>
    /// Creates a roster with updated cost totals.
    /// </summary>
    /// <param name="roster">A roster used to calculate the cost totals.</param>
    /// <returns>The created instance with recalculated cost totals.</returns>
    public static RosterNode WithUpdatedCostTotals(this RosterNode roster)
    {
        var aggregator = new RosterCostAggregator();
        aggregator.Visit(roster);
        return roster.WithCosts(roster.Costs.Select(Update).ToNodeList());

        CostNode Update(CostNode cost)
        {
            return cost.WithValue(aggregator.Costs.TryGetValue(cost.TypeId!, out var value) ? value : 0);
        }
    }

    public static SelectionNode WithUpdatedNumberAndCosts(this SelectionNode node, int newCount)
    {
        if (newCount < 1)
            throw new ArgumentException("Count must be greater than 0", nameof(newCount));
        if (node.Number == newCount)
            // we assume same instance is OK if count doesn't change
            return node;
        var oldCount = node.Number;
        var newCosts = node.Costs.Select(x => x.WithValue(UpdatedValue(x.Value))).ToNodeList();
        return node.WithNumber(newCount).WithCosts(newCosts);

        decimal UpdatedValue(decimal oldCost) => oldCost / oldCount * newCount;
    }

    private class NodeReplacer : SourceRewriter
    {
        public NodeReplacer(Func<SourceNode, bool> match, Func<SourceNode, SourceNode?> transform)
        {
            Match = match;
            Transform = transform;
        }

        public Func<SourceNode, bool> Match { get; }
        public Func<SourceNode, SourceNode?> Transform { get; }

        [return: MaybeNull]
        public override SourceNode? Visit(SourceNode? node)
        {
            return node is null ? null : Match(node) ? Transform(node) : base.Visit(node);
        }
    }

    private class RosterCostAggregator : SourceWalker
    {
        public Dictionary<string, decimal> Costs { get; } = new();

        public override void VisitRoster(RosterNode node)
        {
            Costs.Clear();
            Visit(node.Forces);
        }

        public override void VisitForce(ForceNode node)
        {
            Visit(node.Selections);
        }

        public override void VisitSelection(SelectionNode node)
        {
            Visit(node.Selections);
            foreach (var cost in node.Costs)
            {
                var value = Costs.TryGetValue(cost.TypeId!, out var retrieved) ? retrieved : 0;
                Costs[cost.TypeId!] = value + cost.Value;
            }
        }
    }
}
