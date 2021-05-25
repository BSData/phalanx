namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Roster cost with value and limit.
    /// BS Roster/Cost+CostLimit.
    /// WHAM <see cref="WarHub.ArmouryModel.Source.CostNode" />
    /// and <see cref="WarHub.ArmouryModel.Source.CostLimitNode" />.
    /// </summary>
    public interface IRosterCostSymbol : IRosterItemSymbol
    {
        decimal Value { get; }
        decimal? Limit { get; }
        ICostTypeSymbol CostType { get; }
    }
}
