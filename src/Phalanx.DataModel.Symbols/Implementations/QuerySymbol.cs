using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

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
