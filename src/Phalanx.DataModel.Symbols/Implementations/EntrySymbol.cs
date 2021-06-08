using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public abstract class EntrySymbol : CatalogueItemSymbol, IEntrySymbol
    {
        protected EntryBaseNode entryDeclaration;

        protected EntrySymbol(ISymbol containingSymbol, EntryBaseNode declaration, BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, diagnostics)
        {
            entryDeclaration = declaration;
        }

        public string? Id => entryDeclaration.Id;

        public override string Name => entryDeclaration.Name ?? "";

        public bool IsHidden => entryDeclaration.Hidden;

        public bool IsReference => false;

        public IEntrySymbol? ReferencedEntry => null;

        // TODO bind publication
        public IPublicationSymbol? Publication => throw new System.NotImplementedException();

        // TODO bind modifiers
        public ImmutableArray<IEffectSymbol> Effects => throw new System.NotImplementedException();
    }
}
