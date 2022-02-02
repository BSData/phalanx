using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class RuleSymbol : EntrySymbol, IRuleSymbol, INodeDeclaredSymbol<RuleNode>
{
    public RuleSymbol(
        ISymbol containingSymbol,
        RuleNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override RuleNode Declaration { get; }

    public override SymbolKind Kind => SymbolKind.Resource;

    public ResourceKind ResourceKind => ResourceKind.Rule;

    public string DescriptionText => Declaration.Description ?? string.Empty;

    IResourceDefinitionSymbol? IResourceEntrySymbol.Type => null;

    IResourceEntrySymbol? IResourceEntrySymbol.ReferencedEntry => null;
}
