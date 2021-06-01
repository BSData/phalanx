using System;
using System.Collections.Immutable;
using System.Linq;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.SourceAnalysis;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class GamesystemSymbol : Symbol, ICatalogueSymbol
    {
        private readonly GamesystemNode node;
        private readonly ImmutableArray<CostTypeSymbol> costTypes;
        private readonly ImmutableArray<ProfileTypeSymbol> profileTypes;
        private readonly ImmutableArray<PublicationSymbol> publications;

        public GamesystemSymbol(GamesystemContext context, BindingDiagnosticContext diagnostics)
        {
            node = context.Gamesystem ?? throw new ArgumentNullException(nameof(context), "Needs to have game system.");
            costTypes = node.CostTypes.Select(x => new CostTypeSymbol(this, x, context, diagnostics)).ToImmutableArray();
            profileTypes = node.ProfileTypes.Select(x => new ProfileTypeSymbol(this, x, context, diagnostics)).ToImmutableArray();
            publications = node.Publications.Select(x => new PublicationSymbol(this, x, context, diagnostics)).ToImmutableArray();
            // TODO shared entries
            // TODO root entries
        }

        public bool IsLibrary => false;

        public bool IsGamesystem => true;

        public ICatalogueSymbol Gamesystem => this;

        public ImmutableArray<ICatalogueReferenceSymbol> Imports => ImmutableArray<ICatalogueReferenceSymbol>.Empty;

        public ImmutableArray<ICatalogueItemSymbol> AllItems =>
            ResourceDefinitions
            .CastArray<ICatalogueItemSymbol>()
            .AddRange(RootEntries)
            .AddRange(SharedEntries);

        public ImmutableArray<IResourceDefinitionSymbol> ResourceDefinitions =>
            costTypes
            .CastArray<IResourceDefinitionSymbol>()
            .AddRange(profileTypes)
            .AddRange(publications);

        public ImmutableArray<ISelectionEntrySymbol> RootEntries => throw new System.NotImplementedException();

        public ImmutableArray<IEntrySymbol> SharedEntries => throw new System.NotImplementedException();

        public override SymbolKind Kind => SymbolKind.Catalogue;

        public override string Name => node.Name ?? "";

        public override string? Comment => node.Comment;

        public override ISymbol ContainingSymbol => this;
    }
}
