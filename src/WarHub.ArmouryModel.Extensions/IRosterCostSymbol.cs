namespace WarHub.ArmouryModel;

/// <summary>
/// Roster cost with value and limit.
/// BS Roster/Cost+CostLimit.
/// WHAM <see cref="Source.CostNode" />
/// and <see cref="Source.CostLimitNode" />.
/// </summary>
public interface IRosterCostSymbol : IRosterItemSymbol
{
    decimal Value { get; }
    decimal? Limit { get; }
    ICostTypeSymbol CostType { get; }
}
