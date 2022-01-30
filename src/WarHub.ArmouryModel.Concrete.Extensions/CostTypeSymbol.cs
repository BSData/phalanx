using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CostTypeSymbol : SourceDeclaredSymbol, ICostTypeSymbol
{
    internal new CostTypeNode Declaration { get; }

    public CostTypeSymbol(
        ICatalogueSymbol containingSymbol,
        CostTypeNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.ResourceDefinition;

    public ResourceKind ResourceKind => ResourceKind.Cost;
}
