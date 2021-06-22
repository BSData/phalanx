using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public abstract class EntrySymbol : CatalogueItemSymbol, IEntrySymbol
    {
        internal EntryBaseNode Declaration { get; }

        protected EntrySymbol(
            ISymbol containingSymbol,
            EntryBaseNode declaration,
            Binder binder,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, diagnostics)
        {
            Declaration = declaration;
            // consider binding at later stage
            var publication = binder.BindPublicationSymbol(declaration);
            if (publication is not null)
            {
                PublicationReference = new EntryPublicationReferenceSymbol(this, publication);
            }
            Effects = LogicSymbol.CreateEffects(this, binder, diagnostics);
        }

        public string? Id => Declaration.Id;

        public override string Name => Declaration.Name ?? "";

        public bool IsHidden => Declaration.Hidden;

        public bool IsReference => false;

        public IEntrySymbol? ReferencedEntry => null;

        public IPublicationReferenceSymbol? PublicationReference { get; }

        public ImmutableArray<IEffectSymbol> Effects { get; }
    }
}
