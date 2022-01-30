using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel;

public static class SourceNodeExtensions
{
    public static Location GetLocation(this SourceNode node)
    {
        // TODO remove this after SourceNode has this method itself
        //return new SourceLocation(node);
        // we don't have tree ref here
        var tree = SourceTree.CreateForRoot(node.AncestorsAndSelf().Last());
        return new SourceLocation(tree, node.GetSpan());
    }

    public static TextSpan GetSpan(this SourceNode node)
    {
        // TODO remove this after SourceNode has this method itself
        var pos = node.AncestorsAndSelf().Select(item =>
        {
            if (item.Parent is null)
                return 0;
            var precedingSiblingWidth = item.Parent.Children().Take(item.IndexInParent).Sum(x => x.GetWidth());
            return precedingSiblingWidth;
        }).Sum();
        return new TextSpan(pos, node.GetWidth());
    }

    public static SourceTree GetSourceTree(this SourceNode node, Compilation compilation)
    {
        // TODO this should not require compilation (a node knows it's tree)
        return compilation.SourceTrees.Single(x => x.GetRoot() == node);
    }

    public static int GetWidth(this SourceNode node)
    {
        // TODO remove this after SourceNode has this method itself
        return node.DescendantsAndSelf().Count();
    }
}
