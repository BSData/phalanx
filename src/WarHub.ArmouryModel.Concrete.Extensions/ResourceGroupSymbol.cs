using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ResourceGroupSymbol : EntrySymbol, IResourceGroupSymbol, INodeDeclaredSymbol<InfoGroupNode>
{
    public ResourceGroupSymbol(
        ISymbol containingSymbol,
        InfoGroupNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        Resources = CreateResourceEntries().ToImmutableArray();

        IEnumerable<IResourceEntrySymbol> CreateResourceEntries()
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

    public override SymbolKind Kind => SymbolKind.Resource;

    public ResourceKind ResourceKind => ResourceKind.Group;

    public ImmutableArray<IResourceEntrySymbol> Resources { get; }

    public IResourceDefinitionSymbol? Type => null;

    public IResourceEntrySymbol? ReferencedEntry => null;
}
