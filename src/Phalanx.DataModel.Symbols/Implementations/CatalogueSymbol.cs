using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.SourceAnalysis;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class CatalogueSymbol : CatalogueBaseSymbol
    {
        private readonly CatalogueNode node;

        public CatalogueSymbol(CatalogueNode node, GamesystemContext context, BindingDiagnosticContext diagnostics)
            : base(node, context, diagnostics)
        {
            this.node = node;
            // TODO bind GamesystemSymbol
            // TODO bind Imports
        }

        public override bool IsLibrary => node.IsLibrary;

        public override bool IsGamesystem => false;

        public override ICatalogueSymbol Gamesystem => throw new System.NotImplementedException();

        public override ImmutableArray<ICatalogueReferenceSymbol> Imports { get; }
    }
}
