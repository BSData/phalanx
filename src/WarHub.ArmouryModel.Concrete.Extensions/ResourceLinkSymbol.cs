using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ResourceLinkSymbol : EntrySymbol, IResourceEntrySymbol, INodeDeclaredSymbol<InfoLinkNode>
{
    private IResourceEntrySymbol? lazyReferencedEntry;

    public ResourceLinkSymbol(
        ISymbol containingSymbol,
        InfoLinkNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        ResourceKind = Declaration.Type switch
        {
            InfoLinkKind.InfoGroup => ResourceKind.Group,
            InfoLinkKind.Profile => ResourceKind.Profile,
            InfoLinkKind.Rule => ResourceKind.Rule,
            _ => ResourceKind.Error,
        };
        if (ResourceKind is ResourceKind.Error)
        {
            diagnostics.Add(ErrorCode.ERR_UnknownEnumerationValue, Declaration);
        }
    }

    public override InfoLinkNode Declaration { get; }

    public override SymbolKind Kind => SymbolKind.Resource;

    public ResourceKind ResourceKind { get; }

    public IResourceDefinitionSymbol? Type => null;

    public IResourceEntrySymbol ReferencedEntry => GetBoundField(ref lazyReferencedEntry);

    protected override IEntrySymbol? BaseReferencedEntry => ReferencedEntry;

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);

        lazyReferencedEntry = binder.BindSharedResourceEntrySymbol(Declaration, diagnostics);
    }
}
