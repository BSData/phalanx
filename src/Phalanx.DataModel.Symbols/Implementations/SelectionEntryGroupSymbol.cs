using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class SelectionEntryGroupSymbol : SelectionEntryBaseSymbol, ISelectionEntryGroupSymbol
{
    private ISelectionEntrySymbol? lazyDefaultEntry;

    internal new SelectionEntryGroupNode Declaration { get; }

    public SelectionEntryGroupSymbol(
        ISymbol containingSymbol,
        SelectionEntryGroupNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.Entry;

    public override bool IsSelectionGroup => true;

    public ISelectionEntrySymbol? DefaultSelectionEntry
    {
        get
        {
            ForceComplete();
            return lazyDefaultEntry;
        }
    }

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyDefaultEntry = binder.BindSelectionEntrySymbol(Declaration.DefaultSelectionEntryId);
    }
}
