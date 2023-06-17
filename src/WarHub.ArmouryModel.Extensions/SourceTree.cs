using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel;

public abstract class SourceTree
{
    public virtual string? FilePath { get; }

    public abstract SourceNode GetRoot();

    public abstract SourceTree WithRoot(SourceNode newRootNode);

    public static SourceTree CreateForRoot(SourceNode rootNode) =>
        new InMemoryTree(rootNode);

    public static async Task<SourceTree> CreateForDatafileAsync(IDatafileInfo datafile)
    {
        var node = await datafile.GetDataAsync();
        if (node is null)
        {
            throw new InvalidOperationException("Failed to read datafile: " + datafile.Filepath);
        }
        return new InMemoryFilebasedTree(node, datafile.Filepath);
    }

    private class InMemoryTree : SourceTree
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

    private class InMemoryFilebasedTree : InMemoryTree
    {
        private readonly string? filepath;

        public InMemoryFilebasedTree(SourceNode root, string? filepath) : base(root)
        {
            this.filepath = filepath;
        }

        public override string? FilePath => filepath;

        public override InMemoryFilebasedTree WithRoot(SourceNode newRootNode) =>
            new(newRootNode, FilePath);
    }

    public abstract FileLinePositionSpan GetLineSpan(TextSpan span);
    public abstract Location GetLocation(TextSpan span);
}
