using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class QuerySymbol : LogicSymbol, IQuerySymbol
{
    public QuerySymbol(
        ICatalogueItemSymbol containingSymbol,
        QueryBaseNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol)
    {
        // TODO query
    }
}
