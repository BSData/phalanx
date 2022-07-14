using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class ResourceEntryBaseSymbol : EntrySymbol, IResourceEntrySymbol
{
    protected ResourceEntryBaseSymbol(
        ISymbol containingSymbol,
        SourceNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Resources = CreateResourceEntries().ToImmutableArray();

        IEnumerable<ResourceEntryBaseSymbol> CreateResourceEntries()
        {
            if (declaration is not InfoGroupNode groupNode)
                yield break;
            foreach (var item in groupNode.InfoGroups)
            {
                yield return CreateEntry(containingSymbol, item, diagnostics);
            }
            foreach (var item in groupNode.InfoLinks)
            {
                yield return CreateEntry(containingSymbol, item, diagnostics);
            }
            foreach (var item in groupNode.Profiles)
            {
                yield return CreateEntry(containingSymbol, item, diagnostics);
            }
            foreach (var item in groupNode.Rules)
            {
                yield return CreateEntry(containingSymbol, item, diagnostics);
            }
        }
    }

    public sealed override SymbolKind Kind => SymbolKind.Resource;

    public abstract ResourceKind ResourceKind { get; }

    public virtual IResourceDefinitionSymbol? Type => null;

    public override IResourceEntrySymbol? ReferencedEntry => null;

    public sealed override ImmutableArray<ResourceEntryBaseSymbol> Resources { get; }
}
