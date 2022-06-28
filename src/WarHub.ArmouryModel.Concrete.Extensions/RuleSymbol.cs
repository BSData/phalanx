using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class RuleSymbol : ResourceEntryBaseSymbol, IRuleSymbol, INodeDeclaredSymbol<RuleNode>
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

    public override ResourceKind ResourceKind => ResourceKind.Rule;

    public string DescriptionText => Declaration.Description ?? string.Empty;
}
