using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class SelectionReferencePathSymbol : SourceDeclaredSymbol, ISelectionReferencePathSymbol
{
    private ImmutableArray<ISelectionEntryContainerSymbol> lazySourceEntries;

    public SelectionReferencePathSymbol(
        ISymbol? containingSymbol,
        SelectionNode declaration)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
    }

    public override SelectionNode Declaration { get; }

    public override SymbolKind Kind => SymbolKind.Link;

    public ImmutableArray<ISelectionEntryContainerSymbol> SourceEntries => GetBoundField(ref lazySourceEntries);

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);
        lazySourceEntries = binder.BindSelectionSourcePathSymbol(Declaration, diagnostics);
    }
}
