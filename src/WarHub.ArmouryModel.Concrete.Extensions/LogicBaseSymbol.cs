using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class LogicBaseSymbol : SourceDeclaredSymbol, ILogicSymbol
{
    protected LogicBaseSymbol(ISymbol? containingSymbol, SourceNode declaration) : base(containingSymbol, declaration)
    {
    }
}
