using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class RuleSymbol : EntrySymbol, IRuleSymbol
    {
        private readonly RuleNode declaration;

        public RuleSymbol(ISymbol containingSymbol, RuleNode declaration, Binder binder, BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, binder, diagnostics)
        {
            this.declaration = declaration;
        }

        public IResourceDefinitionSymbol? Type => null;

        public override SymbolKind Kind => SymbolKind.Resource;

        public string DescriptionText => declaration.Description ?? "";

        IRuleSymbol? IRuleSymbol.ReferencedEntry => null;
    }
}
