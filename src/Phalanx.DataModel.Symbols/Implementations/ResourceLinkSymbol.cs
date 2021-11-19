using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class ResourceLinkSymbol : EntrySymbol, IResourceEntrySymbol
{
    public ResourceLinkSymbol(
        ICatalogueItemSymbol containingSymbol,
        InfoLinkNode declaration,
        Binder binder,
        BindingDiagnosticContext diagnostics)
        : base(containingSymbol, declaration, binder, diagnostics)
    {
        ReferencedEntry = null; // TODO bind
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public IResourceDefinitionSymbol? Type => null;

    public IResourceEntrySymbol? ReferencedEntry { get; }
}
