using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ResourceGroupSymbol : ResourceEntryBaseSymbol, IResourceGroupSymbol, INodeDeclaredSymbol<InfoGroupNode>
{
    public ResourceGroupSymbol(
        ISymbol containingSymbol,
        InfoGroupNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        Resources = CreateResourceEntries().ToImmutableArray();

        IEnumerable<ResourceEntryBaseSymbol> CreateResourceEntries()
        {
            foreach (var item in declaration.InfoGroups)
            {
                yield return CreateEntry(containingSymbol, item, diagnostics);
            }
            foreach (var item in declaration.InfoLinks)
            {
                yield return CreateEntry(containingSymbol, item, diagnostics);
            }
            foreach (var item in declaration.Profiles)
            {
                yield return CreateEntry(containingSymbol, item, diagnostics);
            }
            foreach (var item in declaration.Rules)
            {
                yield return CreateEntry(containingSymbol, item, diagnostics);
            }
        }
    }

    public override InfoGroupNode Declaration { get; }

    public override ResourceKind ResourceKind => ResourceKind.Group;

    public ImmutableArray<ResourceEntryBaseSymbol> Resources { get; }

    ImmutableArray<IResourceEntrySymbol> IResourceGroupSymbol.Resources =>
        Resources.Cast<ResourceEntryBaseSymbol, IResourceEntrySymbol>();

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics).AddRange(Resources);
}
