using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal class ResourceLinkSymbol : EntrySymbol, IResourceEntrySymbol
{
    private IResourceEntrySymbol? lazyReferencedEntry;

    public ResourceLinkSymbol(
        ISymbol containingSymbol,
        InfoLinkNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public ResourceKind ResourceKind => Declaration.Type switch
    {
        InfoLinkKind.InfoGroup => ResourceKind.Group,
        InfoLinkKind.Profile => ResourceKind.Profile,
        InfoLinkKind.Rule => ResourceKind.Rule,
        _ => throw new NotSupportedException($"Unknown value '{Declaration.Type}'"),
    };

    public IResourceDefinitionSymbol? Type => null;

    internal new InfoLinkNode Declaration { get; }

    public IResourceEntrySymbol? ReferencedEntry => lazyReferencedEntry;

    protected override IEntrySymbol? BaseReferencedEntry => ReferencedEntry;

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyReferencedEntry = binder.BindResourceEntrySymbol(Declaration.TargetId, Declaration.Type);
    }
}
