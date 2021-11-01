using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public abstract class CatalogueBaseSymbol : Symbol, ICatalogueSymbol
    {
        private readonly CatalogueBaseNode node;
        private readonly ImmutableArray<CostTypeSymbol> costTypes;
        private readonly ImmutableArray<ProfileTypeSymbol> profileTypes;
        private readonly ImmutableArray<PublicationSymbol> publications;

        protected CatalogueBaseSymbol(
            IDatasetSymbol containingSymbol,
            CatalogueBaseNode declaration,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            ContainingDataset = containingSymbol;
            this.node = declaration;
            costTypes = declaration.CostTypes.Select(x => new CostTypeSymbol(this, x, diagnostics)).ToImmutableArray();
            profileTypes = declaration.ProfileTypes.Select(x => new ProfileTypeSymbol(this, x, diagnostics)).ToImmutableArray();
            publications = declaration.Publications.Select(x => new PublicationSymbol(this, x, diagnostics)).ToImmutableArray();
            RootContainerEntries = CreateRootContainerEntries().ToImmutableArray();
            RootResourceEntries = CreateRootResourceEntries().ToImmutableArray();
            SharedSelectionEntryContainers = CreateSharedContainerEntries().ToImmutableArray();
            SharedResourceEntries = CreateSharedResourceEntries().ToImmutableArray();

            IEnumerable<IContainerEntrySymbol> CreateRootContainerEntries()
            {
                foreach (var item in declaration.SelectionEntries)
                {
                    yield return EntrySymbol.CreateEntry(this, item, binder, diagnostics);
                }
                foreach (var item in declaration.EntryLinks)
                {
                    yield return EntrySymbol.CreateEntry(this, item, binder, diagnostics);
                }
                foreach (var item in declaration.ForceEntries)
                {
                    yield return EntrySymbol.CreateEntry(this, item, binder, diagnostics);
                }
                foreach (var item in declaration.CategoryEntries)
                {
                    yield return EntrySymbol.CreateEntry(this, item, binder, diagnostics);
                }
            }
            IEnumerable<IResourceEntrySymbol> CreateRootResourceEntries()
            {
                foreach (var item in declaration.Rules)
                {
                    yield return EntrySymbol.CreateEntry(this, item, binder, diagnostics);
                }
            }
            IEnumerable<ISelectionEntryContainerSymbol> CreateSharedContainerEntries()
            {
                foreach (var item in declaration.SharedSelectionEntries)
                {
                    yield return EntrySymbol.CreateEntry(this, item, binder, diagnostics);
                }
                foreach (var item in declaration.SharedSelectionEntryGroups)
                {
                    yield return EntrySymbol.CreateEntry(this, item, binder, diagnostics);
                }
            }
            IEnumerable<IResourceEntrySymbol> CreateSharedResourceEntries()
            {
                foreach (var item in declaration.SharedInfoGroups)
                {
                    yield return EntrySymbol.CreateEntry(this, item, binder, diagnostics);
                }
                foreach (var item in declaration.SharedProfiles)
                {
                    yield return EntrySymbol.CreateEntry(this, item, binder, diagnostics);
                }
                foreach (var item in declaration.SharedRules)
                {
                    yield return EntrySymbol.CreateEntry(this, item, binder, diagnostics);
                }
            }
        }

        internal IDatasetSymbol ContainingDataset { get; }

        public override SymbolKind Kind => SymbolKind.Catalogue;

        public ICatalogueSymbol ContainingCatalogue => this;

        // TODO consider compilation/root symbol?
        public override ISymbol ContainingSymbol => ContainingDataset;

        public override string Name => node.Name ?? "";

        public override string? Comment => node.Comment;

        public abstract bool IsLibrary { get; }

        public abstract bool IsGamesystem { get; }

        public abstract ICatalogueSymbol Gamesystem { get; }

        public abstract ImmutableArray<ICatalogueReferenceSymbol> Imports { get; }

        public ImmutableArray<ICatalogueItemSymbol> AllItems =>
            ImmutableArray<ICatalogueItemSymbol>.Empty
            .AddRange(Imports)
            .AddRange(ResourceDefinitions)
            .AddRange(RootContainerEntries)
            .AddRange(RootResourceEntries)
            .AddRange(SharedSelectionEntryContainers)
            .AddRange(SharedResourceEntries);

        public ImmutableArray<IResourceDefinitionSymbol> ResourceDefinitions =>
            ImmutableArray<IResourceDefinitionSymbol>.Empty
            .AddRange(costTypes)
            .AddRange(profileTypes)
            .AddRange(publications);

        public ImmutableArray<IContainerEntrySymbol> RootContainerEntries { get; }

        public ImmutableArray<IResourceEntrySymbol> RootResourceEntries { get; }

        public ImmutableArray<ISelectionEntryContainerSymbol> SharedSelectionEntryContainers { get; }

        public ImmutableArray<IResourceEntrySymbol> SharedResourceEntries { get; }
    }
}
