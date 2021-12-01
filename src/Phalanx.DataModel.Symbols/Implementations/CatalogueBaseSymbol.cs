using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class CatalogueBaseSymbol : SourceDeclaredSymbol, ICatalogueSymbol
{
    private readonly ImmutableArray<CostTypeSymbol> costTypes;
    private readonly ImmutableArray<ProfileTypeSymbol> profileTypes;
    private readonly ImmutableArray<PublicationSymbol> publications;

    protected CatalogueBaseSymbol(
        Compilation declaringCompilation,
        CatalogueBaseNode declaration)
        : base(declaration)
    {
        DeclaringCompilation = declaringCompilation;
        Declaration = declaration;
        var diagnostics = DiagnosticBag.GetInstance();
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
                yield return EntrySymbol.CreateEntry(this, item, diagnostics);
            }
            foreach (var item in declaration.EntryLinks)
            {
                yield return EntrySymbol.CreateEntry(this, item, diagnostics);
            }
            foreach (var item in declaration.ForceEntries)
            {
                yield return EntrySymbol.CreateEntry(this, item, diagnostics);
            }
            foreach (var item in declaration.CategoryEntries)
            {
                yield return EntrySymbol.CreateEntry(this, item, diagnostics);
            }
        }
        IEnumerable<IResourceEntrySymbol> CreateRootResourceEntries()
        {
            foreach (var item in declaration.Rules)
            {
                yield return EntrySymbol.CreateEntry(this, item, diagnostics);
            }
        }
        IEnumerable<ISelectionEntryContainerSymbol> CreateSharedContainerEntries()
        {
            foreach (var item in declaration.SharedSelectionEntries)
            {
                yield return EntrySymbol.CreateEntry(this, item, diagnostics);
            }
            foreach (var item in declaration.SharedSelectionEntryGroups)
            {
                yield return EntrySymbol.CreateEntry(this, item, diagnostics);
            }
        }
        IEnumerable<IResourceEntrySymbol> CreateSharedResourceEntries()
        {
            foreach (var item in declaration.SharedInfoGroups)
            {
                yield return EntrySymbol.CreateEntry(this, item, diagnostics);
            }
            foreach (var item in declaration.SharedProfiles)
            {
                yield return EntrySymbol.CreateEntry(this, item, diagnostics);
            }
            foreach (var item in declaration.SharedRules)
            {
                yield return EntrySymbol.CreateEntry(this, item, diagnostics);
            }
        }
    }

    internal new CatalogueBaseNode Declaration { get; }

    public override SymbolKind Kind => SymbolKind.Catalogue;

    public ICatalogueSymbol ContainingCatalogue => this;

    public override ISymbol? ContainingSymbol => null;

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

    internal override Compilation DeclaringCompilation { get; }
}
