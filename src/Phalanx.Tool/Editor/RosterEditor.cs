using System;
using System.Collections.Immutable;
using System.Linq;
using WarHub.ArmouryModel.Source;
using static WarHub.ArmouryModel.Source.NodeFactory;

namespace Phalanx.Tool.Editor
{
    public delegate TNode NodeSelector<TRoot, TNode>(TRoot root)
        where TRoot : IRootNode where TNode : SourceNode;

    public record RosterEditor(RosterState State)
    {
        public RosterNode Roster => State.Roster;
        public GamesystemNode Gamesystem => State.Dataset.Gamesystem;
        public ImmutableArray<CatalogueNode> Catalogues => State.Dataset.Catalogues;
        public ImmutableArray<RosterDiagnostic> Diagnostics => State.Diagnostics;
        public static CreateRosterOperationBuilder Create(Dataset dataset) => new(dataset);
        public ChangeCostLimitOperationBuilder ChangeCostLimit(CostLimitNode costLimit) => new(costLimit);
        public ForceAddEditor AddForce(ForceEntryNode forceEntry) => new(forceEntry);
        public SelectionAddEditor AddSelection(SelectionEntryNode selectionEntry) => new(selectionEntry);

        // probably an extension method
        public RosterEditor Apply(Func<RosterEditor, IRosterOperation> operationSelector) =>
            this with { State = operationSelector(this).Apply(State) };

        public abstract record OperationBuilderBase
        {
            protected abstract RosterOperationKind Kind { get; }

            protected static IRosterOperation Operation(Func<RosterNode, RosterNode> rosterTransform)
            {
                // TODO add diagnostics for the new roster state
                return new LambdaOperation(state => state with { Roster = rosterTransform(state.Roster) });
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

        public record ForceAddEditor(ForceEntryNode ForceEntry) : OperationBuilderBase
        {
            protected override RosterOperationKind Kind => RosterOperationKind.AddForce;

            public IRosterOperation ToRoot() => Operation(roster =>
            {
                // TODO add categories, rules, profiles, catalogue cost types to cost limits
                return roster.AddForces(Force(ForceEntry));
            });
        }

        public record SelectionAddEditor(SelectionEntryNode SelectionEntry) : OperationBuilderBase
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
                // TODO aggregate costs in roster
                // TODO add selection categories, rules, profiles
                return roster.WithForces(roster.Forces.Replace(x => x == force, x => x.AddSelections(selection)));
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
    }
}
