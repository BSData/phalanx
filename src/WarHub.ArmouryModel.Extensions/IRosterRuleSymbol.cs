namespace WarHub.ArmouryModel;

/// <summary>
/// Rule with simple text contents.
/// Instance of <see cref="IRuleSymbol"/> in <see cref="SourceEntry"/>.
/// BS Rule in Roster.
/// WHAM <see cref="Source.RuleNode" />.
/// </summary>
public interface IRosterRuleSymbol : IResourceSymbol
{
    string DescriptionText { get; }

    new IRuleSymbol SourceEntry { get; }
}
