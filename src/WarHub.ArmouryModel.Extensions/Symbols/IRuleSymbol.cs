namespace WarHub.ArmouryModel;

/// <summary>
/// Rule with simple text contents.
/// BS RuleEntry/InfoLink@type=rule.
/// WHAM <see cref="Source.RuleNode" />.
/// </summary>
public interface IRuleSymbol : IResourceEntrySymbol
{
    string DescriptionText { get; }
}
