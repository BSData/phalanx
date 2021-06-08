namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Rule with simple text contents.
    /// BS RuleEntry/InfoLink@type=rule.
    /// WHAM <see cref="WarHub.ArmouryModel.Source.RuleNode" />.
    /// </summary>
    public interface IRuleSymbol : IResourceEntrySymbol
    {
        string DescriptionText { get; }
        new IRuleSymbol? ReferencedEntry { get; }
    }
}
