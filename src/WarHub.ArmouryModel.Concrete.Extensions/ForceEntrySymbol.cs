using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ForceEntrySymbol : ContainerEntryBaseSymbol, IForceEntrySymbol
{
    public ForceEntrySymbol(
        ISymbol containingSymbol,
        ForceEntryNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;

        ChildForces = CreateChildEntries().ToImmutableArray();
        Categories = CreateCategories().ToImmutableArray();

        IEnumerable<IForceEntrySymbol> CreateChildEntries()
        {
            foreach (var item in declaration.ForceEntries)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
        }
        IEnumerable<ICategoryEntrySymbol> CreateCategories()
        {
            foreach (var item in declaration.CategoryLinks)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
        }
    }

    public override ContainerEntryKind ContainerKind => ContainerEntryKind.Force;

    public ImmutableArray<IForceEntrySymbol> ChildForces { get; }

    public ImmutableArray<ICategoryEntrySymbol> Categories { get; }

    internal new ContainerEntryBaseNode Declaration { get; }
}
