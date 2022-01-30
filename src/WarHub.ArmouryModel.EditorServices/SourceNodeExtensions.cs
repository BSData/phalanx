using System.Diagnostics.CodeAnalysis;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.EditorServices;

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
