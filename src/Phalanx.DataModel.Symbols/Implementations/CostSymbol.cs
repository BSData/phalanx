using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class CostSymbol : SimpleResourceEntrySymbol, ICostSymbol
    {
        private readonly CostNode declaration;

        public CostSymbol(
            ISymbol containingSymbol,
            CostNode declaration,
            Binder binder,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, diagnostics)
        {
            this.declaration = declaration;
            Type = null!; // TODO bind
        }

        public ICostTypeSymbol Type { get; }

        public decimal Value => declaration.Value;

        protected override IResourceDefinitionSymbol? BaseType => Type;
    }
}
