namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Rule with simple text contents.
    /// BS RuleEntry/InfoLink@type=rule.
    /// WHAM <see cref="WarHub.ArmouryModel.Source.RuleNode" />.
    /// </summary>
    public interface IRuleSymbol : IResourceEntrySymbol
    {
        new IRuleSymbol? ReferencedEntry { get; }
    }
}
