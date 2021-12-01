using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class SelectionEntryLinkSymbol : SelectionEntryBaseSymbol
{
    private ISelectionEntryContainerSymbol? lazyReference;

    internal new EntryLinkNode Declaration { get; }

    public SelectionEntryLinkSymbol(
        ICatalogueItemSymbol containingSymbol,
        EntryLinkNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public override ISelectionEntryContainerSymbol? ReferencedEntry
    {
        get
        {
            ForceComplete();
            return lazyReference ?? throw new InvalidOperationException("Binding failed");
        }
    }

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyReference = binder.BindSharedSelectionEntrySymbol(Declaration.TargetId, Declaration.Type);
    }
}
