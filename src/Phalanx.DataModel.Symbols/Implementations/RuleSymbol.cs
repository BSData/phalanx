using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class RuleSymbol : EntrySymbol, IRuleSymbol
{
    private readonly RuleNode declaration;

    public RuleSymbol(
        ICatalogueItemSymbol containingSymbol,
        RuleNode declaration,
        Binder binder,
        BindingDiagnosticContext diagnostics)
        : base(containingSymbol, declaration, binder, diagnostics)
    {
        this.declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.Resource;

    public string DescriptionText => declaration.Description ?? string.Empty;

    IResourceDefinitionSymbol? IResourceEntrySymbol.Type => null;

    IResourceEntrySymbol? IResourceEntrySymbol.ReferencedEntry => null;
}
