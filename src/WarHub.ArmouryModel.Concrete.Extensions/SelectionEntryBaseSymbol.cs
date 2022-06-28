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

        IEnumerable<ICategoryEntrySymbol> CreateCategoryEntries()
        {
            foreach (var item in declaration.CategoryLinks)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
        }
        IEnumerable<ISelectionEntryContainerSymbol> CreateSelectionEntryContainers()
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

    public ICategoryEntrySymbol? PrimaryCategory { get; }

    public ImmutableArray<ICategoryEntrySymbol> Categories { get; }

    public ImmutableArray<ISelectionEntryContainerSymbol> ChildSelectionEntries { get; }
}
