using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class CostTypeSymbol : SourceCatalogueItemSymbol, ICostTypeSymbol
{
    private readonly CostTypeNode declaration;

    public CostTypeSymbol(
        ICatalogueSymbol containingSymbol,
        CostTypeNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        this.declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.ResourceType;
}
