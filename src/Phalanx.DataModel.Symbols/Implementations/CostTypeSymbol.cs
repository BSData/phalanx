using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class CostTypeSymbol : CatalogueItemSymbol, ICostTypeSymbol
    {
        private readonly CostTypeNode declaration;

        public CostTypeSymbol(ICatalogueSymbol containingSymbol, CostTypeNode declaration, BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, diagnostics)
        {
            this.declaration = declaration;
        }

        public override SymbolKind Kind => SymbolKind.ResourceType;

        public override string Name => declaration.Name ?? "";

        public string? Id => declaration.Id;
    }
}
