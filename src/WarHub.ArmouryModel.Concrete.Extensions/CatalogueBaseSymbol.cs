using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class CatalogueBaseSymbol : SourceDeclaredSymbol, ICatalogueSymbol
{
    protected CatalogueBaseSymbol(
        SourceGlobalNamespaceSymbol containingSymbol,
        CatalogueBaseNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        ContainingNamespace = containingSymbol;
        Declaration = declaration;
        ResourceDefinitions = CreateResourceDefinitions().ToImmutableArray();
        RootContainerEntries = CreateRootContainerEntries().ToImmutableArray();
        RootResourceEntries = CreateRootResourceEntries().ToImmutableArray();
        SharedSelectionEntryContainers = CreateSharedContainerEntries().ToImmutableArray();
        SharedResourceEntries = CreateSharedResourceEntries().ToImmutableArray();

        IEnumerable<ResourceDefinitionBaseSymbol> CreateResourceDefinitions()
        {
            foreach (var item in declaration.CostTypes)
            {
                yield return new CostTypeSymbol(this, item, diagnostics);
            }
            foreach (var item in declaration.ProfileTypes)
            {
                yield return new ProfileTypeSymbol(this, item, diagnostics);
            }
            foreach (var item in declaration.Publications)
            {
                yield return new PublicationSymbol(this, item, diagnostics);
            }
        }

        IEnumerable<ContainerEntryBaseSymbol> CreateRootContainerEntries()
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
        IEnumerable<ResourceEntryBaseSymbol> CreateRootResourceEntries()
        {
            foreach (var item in declaration.Rules)
            {
                yield return EntrySymbol.CreateEntry(this, item, diagnostics);
            }
        }
        IEnumerable<SelectionEntryBaseSymbol> CreateSharedContainerEntries()
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
        IEnumerable<ResourceEntryBaseSymbol> CreateSharedResourceEntries()
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

    public override CatalogueBaseNode Declaration { get; }

    public override SymbolKind Kind => SymbolKind.Catalogue;

    public override SourceGlobalNamespaceSymbol ContainingNamespace { get; }

    public override IModuleSymbol? ContainingModule => null;

    public abstract bool IsLibrary { get; }

    public abstract bool IsGamesystem { get; }

    public abstract ICatalogueSymbol Gamesystem { get; }

    public abstract ImmutableArray<CatalogueReferenceSymbol> CatalogueReferences { get; }

    public ImmutableArray<Symbol> AllItems => GetMembersCore();

    ImmutableArray<ISymbol> ICatalogueSymbol.AllItems => GetMembers();

    public ImmutableArray<ResourceDefinitionBaseSymbol> ResourceDefinitions { get; }

    public ImmutableArray<ContainerEntryBaseSymbol> RootContainerEntries { get; }

    public ImmutableArray<ResourceEntryBaseSymbol> RootResourceEntries { get; }

    public ImmutableArray<SelectionEntryBaseSymbol> SharedSelectionEntryContainers { get; }

    public ImmutableArray<ResourceEntryBaseSymbol> SharedResourceEntries { get; }

    ImmutableArray<IContainerEntrySymbol> ICatalogueSymbol.RootContainerEntries =>
        RootContainerEntries.Cast<ContainerEntryBaseSymbol, IContainerEntrySymbol>();

    ImmutableArray<IResourceEntrySymbol> ICatalogueSymbol.RootResourceEntries =>
        RootResourceEntries.Cast<ResourceEntryBaseSymbol, IResourceEntrySymbol>();

    ImmutableArray<ISelectionEntryContainerSymbol> ICatalogueSymbol.SharedSelectionEntryContainers =>
        SharedSelectionEntryContainers.Cast<SelectionEntryBaseSymbol, ISelectionEntryContainerSymbol>();

    ImmutableArray<IResourceEntrySymbol> ICatalogueSymbol.SharedResourceEntries =>
        SharedResourceEntries.Cast<ResourceEntryBaseSymbol, IResourceEntrySymbol>();

    ImmutableArray<ICatalogueReferenceSymbol> ICatalogueSymbol.CatalogueReferences =>
        CatalogueReferences.Cast<CatalogueReferenceSymbol, ICatalogueReferenceSymbol>();

    ImmutableArray<IResourceDefinitionSymbol> ICatalogueSymbol.ResourceDefinitions =>
        ResourceDefinitions.Cast<ResourceDefinitionBaseSymbol, IResourceDefinitionSymbol>();

    protected sealed override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        .AddRange(CatalogueReferences)
        .AddRange(ResourceDefinitions)
        .AddRange(RootContainerEntries)
        .AddRange(RootResourceEntries)
        .AddRange(SharedSelectionEntryContainers)
        .AddRange(SharedResourceEntries);
}
