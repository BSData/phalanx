using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class RosterRuleSymbol : RosterResourceBaseSymbol, IRosterRuleSymbol, INodeDeclaredSymbol<RuleNode>
{
    public RosterRuleSymbol(ISymbol? containingSymbol, RuleNode declaration, DiagnosticBag diagnostics) : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public new RuleNode Declaration { get; }

    public override ResourceKind ResourceKind => ResourceKind.Rule;

    public string DescriptionText => Declaration.Description ?? string.Empty;

    public override IRuleSymbol SourceEntry => (IRuleSymbol)SourceEntryPath.SourceEntries.Last();
}
