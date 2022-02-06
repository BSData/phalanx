using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal interface INodeDeclaredSymbol<out TNode> where TNode : SourceNode
{
    TNode Declaration { get; }
}
