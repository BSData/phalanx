using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class SelectionEntryBaseSymbol : ContainerEntryBaseSymbol, ISelectionEntryContainerSymbol
{
    protected SelectionEntryBaseSymbol(
        ISymbol containingSymbol,
        SelectionEntryBaseNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Categories = CreateCategoryEntries().ToImmutableArray();
        PrimaryCategory = Categories.FirstOrDefault(x => x.IsPrimaryCategory);
        ChildSelectionEntries = CreateSelectionEntryContainers().ToImmutableArray();

        IEnumerable<CategoryLinkSymbol> CreateCategoryEntries()
        {
            foreach (var item in declaration.CategoryLinks)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
        }
        IEnumerable<SelectionEntryBaseSymbol> CreateSelectionEntryContainers()
        {
            foreach (var item in declaration.EntryLinks)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
            foreach (var item in declaration.SelectionEntries)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
            foreach (var item in declaration.SelectionEntryGroups)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
        }
    }

    public override ISelectionEntryContainerSymbol? ReferencedEntry => null;

    public CategoryLinkSymbol? PrimaryCategory { get; }

    public ImmutableArray<CategoryLinkSymbol> Categories { get; }

    public ImmutableArray<SelectionEntryBaseSymbol> ChildSelectionEntries { get; }

    ICategoryEntrySymbol? ISelectionEntryContainerSymbol.PrimaryCategory => PrimaryCategory;

    ImmutableArray<ICategoryEntrySymbol> ISelectionEntryContainerSymbol.Categories =>
        Categories.Cast<CategoryLinkSymbol, ICategoryEntrySymbol>();

    ImmutableArray<ISelectionEntryContainerSymbol> ISelectionEntryContainerSymbol.ChildSelectionEntries =>
        ChildSelectionEntries.Cast<SelectionEntryBaseSymbol, ISelectionEntryContainerSymbol>();

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        .AddRange(Categories)
        .AddRange(ChildSelectionEntries);
}
