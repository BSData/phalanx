using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class QuerySymbol : LogicSymbol, IQuerySymbol
{
    public QuerySymbol(
        ISymbol containingSymbol,
        QueryBaseNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol)
    {
        // TODO query
    }
}
