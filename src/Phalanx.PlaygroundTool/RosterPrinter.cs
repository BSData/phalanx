using WarHub.ArmouryModel.Source;

namespace Phalanx.PlaygroundTool;

class RosterPrinter : SourceWalker
{
    private int Depth { get; set; }

    public override void VisitRoster(RosterNode node)
    {
        Depth = 0;

        static string FormatCost((CostNode cost, CostLimitNode limit) x) =>
            $"{x.cost.Value}/{x.limit.Value}{x.cost.Name}";
        var limitedCosts = node.Costs.Zip(node.CostLimits).Where(x => x.Second.Value >= 0);
        var costs = $"[{string.Join(", ", limitedCosts.Select(FormatCost))}]";
        Print($"Roster '{node.Name}' ({node.GameSystemName} v{node.GameSystemRevision}) {costs}");
        Depth++;
        base.VisitRoster(node);
        Depth--;
    }

    public override void VisitForce(ForceNode node)
    {
        Print($"- {node.Name} ({node.CatalogueName})");
        Depth++;
        base.VisitForce(node);
        Depth--;
    }

    public override void VisitSelection(SelectionNode node)
    {
        Print($"- {node.Number}x {node.Name} [{string.Join(", ", node.Costs.Select(x => $"{x.Value}{x.Name}"))}]");
        Depth++;
        base.VisitSelection(node);
        Depth--;
    }

    private void Print(string line)
    {
        for (var i = 0; i < Depth; i++)
            Console.Write("  ");
        Console.WriteLine(line);
    }
}
