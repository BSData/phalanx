using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols;

public class Compilation
{
    public ImmutableArray<SourceTree> SourceTrees { get; }

    internal Binder GetBinder(SourceNode node) => throw new NotImplementedException();
}
public abstract class SemanticModel
{

}
public abstract class SourceTree
{
    public abstract SourceNode GetRoot();
}
