using System.Collections.Immutable;
using System.Linq;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.SourceAnalysis;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public abstract class CatalogueBaseSymbol : Symbol, ICatalogueSymbol
    {
        private readonly CatalogueBaseNode node;
        private readonly ImmutableArray<CostTypeSymbol> costTypes;
        private readonly ImmutableArray<ProfileTypeSymbol> profileTypes;
        private readonly ImmutableArray<PublicationSymbol> publications;

        protected CatalogueBaseSymbol(CatalogueBaseNode node, GamesystemContext context, BindingDiagnosticContext diagnostics)
        {
            this.node = node;
            costTypes = node.CostTypes.Select(x => new CostTypeSymbol(this, x, diagnostics)).ToImmutableArray();
            profileTypes = node.ProfileTypes.Select(x => new ProfileTypeSymbol(this, x, diagnostics)).ToImmutableArray();
            publications = node.Publications.Select(x => new PublicationSymbol(this, x, diagnostics)).ToImmutableArray();
            // TODO shared entries
            // TODO root entries
        }

        public abstract bool IsLibrary { get; }

        public abstract bool IsGamesystem { get; }

        public abstract ICatalogueSymbol Gamesystem { get; }

        public abstract ImmutableArray<ICatalogueReferenceSymbol> Imports { get; }

        public ImmutableArray<ICatalogueItemSymbol> AllItems =>
            Imports
            .CastArray<ICatalogueItemSymbol>()
            .AddRange(ResourceDefinitions)
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

        // TODO consider compilation/root symbol?
        public override ISymbol ContainingSymbol => this;
    }
}
