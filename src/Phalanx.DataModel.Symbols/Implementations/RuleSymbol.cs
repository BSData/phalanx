using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class RuleSymbol : EntrySymbol, IRuleSymbol
{
    private readonly RuleNode declaration;

    public RuleSymbol(
        ICatalogueItemSymbol containingSymbol,
        RuleNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        this.declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.Resource;

    public string DescriptionText => declaration.Description ?? string.Empty;

    IResourceDefinitionSymbol? IResourceEntrySymbol.Type => null;

    IResourceEntrySymbol? IResourceEntrySymbol.ReferencedEntry => null;
}
