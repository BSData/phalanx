using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ForceEntrySymbol : ContainerEntryBaseSymbol, IForceEntrySymbol, INodeDeclaredSymbol<ForceEntryNode>
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

        IEnumerable<ForceEntrySymbol> CreateChildEntries()
        {
            foreach (var item in declaration.ForceEntries)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
        }
        IEnumerable<CategoryLinkSymbol> CreateCategories()
        {
            foreach (var item in declaration.CategoryLinks)
            {
                yield return CreateEntry(this, item, diagnostics);
            }
        }
    }

    public override ForceEntryNode Declaration { get; }

    public override ContainerEntryKind ContainerKind => ContainerEntryKind.Force;

    public ImmutableArray<ForceEntrySymbol> ChildForces { get; }

    public ImmutableArray<CategoryLinkSymbol> Categories { get; }

    ImmutableArray<IForceEntrySymbol> IForceEntrySymbol.ChildForces =>
        ChildForces.Cast<ForceEntrySymbol, IForceEntrySymbol>();

    ImmutableArray<ICategoryEntrySymbol> IForceEntrySymbol.Categories =>
        Categories.Cast<CategoryLinkSymbol, ICategoryEntrySymbol>();

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        .AddRange(ChildForces)
        .AddRange(Categories);
}
