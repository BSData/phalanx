using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel;

public abstract class SourceTree
{
    public abstract SourceNode GetRoot();

    public static SourceTree CreateForRoot(SourceNode rootNode) =>
        new InMemoryTree(rootNode);

    private sealed class InMemoryTree : SourceTree
    {
        private readonly SourceNode root;

        public InMemoryTree(SourceNode root)
        {
            this.root = root;
        }
        public override SourceNode GetRoot() => root;
    }
}
