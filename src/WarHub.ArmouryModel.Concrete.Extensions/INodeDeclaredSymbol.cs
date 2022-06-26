namespace WarHub.ArmouryModel.Concrete;

internal interface INodeDeclaredSymbol<out TNode>
{
    TNode Declaration { get; }
}
