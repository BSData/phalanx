using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class SelectionEntryBaseSymbol : ContainerEntryBaseSymbol, ISelectionEntryContainerSymbol
{
    protected SelectionEntryBaseSymbol(
        ICatalogueItemSymbol containingSymbol,
        SelectionEntryBaseNode declaration,
        Binder binder,
        BindingDiagnosticContext diagnostics)
        : base(containingSymbol, declaration, binder, diagnostics)
    {
        Categories = CreateCategoryEntries().ToImmutableArray();
        PrimaryCategory = Categories.FirstOrDefault(x => x.IsPrimaryCategory);
        ChildSelectionEntries = CreateSelectionEntryContainers().ToImmutableArray();

        IEnumerable<ICategoryEntrySymbol> CreateCategoryEntries()
        {
            foreach (var item in declaration.CategoryLinks)
            {
                yield return CreateEntry(this, item, binder, diagnostics);
            }
        }
        IEnumerable<ISelectionEntryContainerSymbol> CreateSelectionEntryContainers()
        {
            foreach (var item in declaration.EntryLinks)
            {
                yield return CreateEntry(this, item, binder, diagnostics);
            }
            foreach (var item in declaration.SelectionEntries)
            {
                yield return CreateEntry(this, item, binder, diagnostics);
            }
            foreach (var item in declaration.SelectionEntryGroups)
            {
                yield return CreateEntry(this, item, binder, diagnostics);
            }
        }
    }

    public virtual ISelectionEntryContainerSymbol? ReferencedEntry => null;

    public ICategoryEntrySymbol? PrimaryCategory { get; }

    public ImmutableArray<ICategoryEntrySymbol> Categories { get; }

    public ImmutableArray<ISelectionEntryContainerSymbol> ChildSelectionEntries { get; }

    protected sealed override IEntrySymbol? BaseReferencedEntry => ReferencedEntry;
}
