using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class ResourceLinkSymbol : EntrySymbol, IResourceEntrySymbol
{
    private IResourceEntrySymbol? lazyReferencedEntry;

    public ResourceLinkSymbol(
        ICatalogueItemSymbol containingSymbol,
        InfoLinkNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public IResourceDefinitionSymbol? Type => null;

    internal new InfoLinkNode Declaration { get; }

    public IResourceEntrySymbol? ReferencedEntry => lazyReferencedEntry;

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyReferencedEntry = binder.BindResourceEntrySymbol(Declaration.TargetId, Declaration.Type);
    }
}
