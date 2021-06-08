using System;
using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.SourceAnalysis;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class GamesystemSymbol : CatalogueBaseSymbol
    {
        public GamesystemSymbol(GamesystemContext context, BindingDiagnosticContext diagnostics)
            : base(
                context.Gamesystem ?? throw new ArgumentNullException(nameof(context), "Needs to have game system."),
                context,
                diagnostics)
        {
        }

        public override bool IsLibrary => false;

        public override bool IsGamesystem => true;

        public override ICatalogueSymbol Gamesystem => this;

        public override ImmutableArray<ICatalogueReferenceSymbol> Imports => ImmutableArray<ICatalogueReferenceSymbol>.Empty;
    }
}
