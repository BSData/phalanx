using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel;

public abstract class Compilation
{
    internal Compilation(
        string? name,
        ImmutableArray<SourceTree> sourceTrees,
        CompilationOptions options)
    {
        Name = name;
        SourceTrees = sourceTrees;
        Options = options;
    }

    public string? Name { get; }

    public ImmutableArray<SourceTree> SourceTrees { get; }

    public CompilationOptions Options { get; }

    public abstract SemanticModel GetSemanticModel(SourceTree tree);

    internal Binder GetBinder(SourceNode node)
    {
        // TODO node has to have SourceTree property...
        // return GetBinderFactory(node.SourceTree).GetBinder(node);
        return GetBinderFactory(SourceTrees.First(x => x.GetRoot() == node)).GetBinder(node);
    }

    internal abstract BinderFactory GetBinderFactory(SourceTree tree);
}
