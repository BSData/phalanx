using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel;

public abstract class SourceTree
{
    public virtual string? FilePath { get; }

    public abstract SourceNode GetRoot();

    public abstract SourceTree WithRoot(SourceNode newRootNode);

    public static SourceTree CreateForRoot(SourceNode rootNode) =>
        new InMemoryTree(rootNode);

    private sealed class InMemoryTree : SourceTree
    {
        private readonly SourceNode root;

        public InMemoryTree(SourceNode root)
        {
            this.root = root;
        }

        public override FileLinePositionSpan GetLineSpan(TextSpan span)
        {
            return default; // TODO implement
        }

        public override Location GetLocation(TextSpan span)
        {
            return new SourceLocation(this, span);
        }

        public override SourceNode GetRoot() => root;

        public override InMemoryTree WithRoot(SourceNode newRootNode) =>
            new(newRootNode);
    }

    public abstract FileLinePositionSpan GetLineSpan(TextSpan span);
    public abstract Location GetLocation(TextSpan span);
}
