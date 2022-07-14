using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class RosterResourceBaseSymbol : EntryInstanceSymbol, IResourceSymbol
{
    protected RosterResourceBaseSymbol(
        ISymbol? containingSymbol,
        SourceNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
    }

    public sealed override SymbolKind Kind => SymbolKind.Resource;

    public abstract ResourceKind ResourceKind { get; }

    IResourceEntrySymbol IResourceSymbol.SourceEntry => (IResourceEntrySymbol)SourceEntry;
}
