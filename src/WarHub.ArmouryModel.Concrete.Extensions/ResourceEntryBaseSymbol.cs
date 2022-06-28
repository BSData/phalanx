using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class ResourceEntryBaseSymbol : EntrySymbol, IResourceEntrySymbol
{
    protected ResourceEntryBaseSymbol(
        ISymbol? containingSymbol,
        SourceNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
    }

    public sealed override SymbolKind Kind => SymbolKind.Resource;

    public abstract ResourceKind ResourceKind { get; }

    public virtual IResourceDefinitionSymbol? Type => null;

    public override IResourceEntrySymbol? ReferencedEntry => null;
}
