using System.Diagnostics.CodeAnalysis;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.EditorServices;

public static class SourceNodeExtensions
{
    public static TRoot Replace<TRoot, TNode>(this TRoot root, TNode node, Func<TNode, TNode?> computeReplacement)
        where TNode : SourceNode where TRoot : SourceNode
    {
        var replacer = new NodeReplacer<TNode>(new[] { node }, computeReplacement);
        return (TRoot?)replacer.Visit(root) ?? throw new InvalidOperationException();
    }

    public static TRoot Replace<TRoot, TNode>(this TRoot root, IEnumerable<TNode> nodes, Func<TNode, TNode?> computeReplacement)
        where TNode : SourceNode where TRoot : SourceNode
    {
        var replacer = new NodeReplacer<TNode>(nodes, computeReplacement);
        return (TRoot?)replacer.Visit(root) ?? throw new InvalidOperationException();
    }

    public static TRoot ReplaceFluent<TRoot, TNode>(this TRoot root, Func<TRoot, TNode> nodeSelector, Func<TNode, TNode?> computeReplacement)
        where TNode : SourceNode where TRoot : SourceNode
        => root.Replace(nodeSelector(root), computeReplacement);


    public static TRoot ReplaceFluent<TRoot, TNode>(this TRoot root, Func<TRoot, IEnumerable<TNode>> nodesSelector, Func<TNode, TNode?> computeReplacement)
        where TNode : SourceNode where TRoot : SourceNode
        => root.Replace(nodesSelector(root), computeReplacement);

    public static TRoot Remove<TRoot, TNode>(this TRoot root, TNode node)
        where TNode : SourceNode where TRoot : SourceNode
    {
        var replacer = new NodeReplacer<TNode>(new[] { node }, x => null);
        return (TRoot?)replacer.Visit(root) ?? throw new InvalidOperationException();
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
        return roster.ReplaceFluent(x => x.Costs, Update);

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
        return node.WithNumber(newCount)
            .ReplaceFluent(x => x.Costs, x => x.WithValue(x.Value / oldCount * newCount));
    }

    private class NodeReplacer<TNode> : SourceRewriter where TNode : SourceNode
    {
        public NodeReplacer(
            IEnumerable<TNode>? nodes,
            Func<TNode, TNode?> computeReplacementNode)
        {
            Nodes = nodes?.ToHashSet();
            ComputeReplacementNode = computeReplacementNode;
        }

        private HashSet<TNode>? Nodes { get; }

        private Func<TNode, TNode?> ComputeReplacementNode { get; }

        public override SourceNode? Visit(SourceNode? originalNode)
        {
            if (originalNode is null)
            {
                return null;
            }
            var visitedNode = base.Visit(originalNode);
            if (visitedNode is null)
            {
                return null;
            }
            return ComputeReplacement(originalNode, visitedNode);
        }

        private SourceNode? ComputeReplacement(SourceNode originalNode, SourceNode visitedNode)
        {
            if (visitedNode is TNode typedNode)
            {
                if (Nodes is { } set)
                {
                    return set.Contains(visitedNode) ? ComputeReplacementNode(typedNode) : visitedNode;
                }
                return ComputeReplacementNode(typedNode);
            }
            return visitedNode;
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
