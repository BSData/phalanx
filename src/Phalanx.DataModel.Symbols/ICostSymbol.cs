namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Defines cost of an entry.
/// BS Cost on SelectionEntries.
/// WHAM <see cref="WarHub.ArmouryModel.Source.CostNode" />.
/// </summary>
public interface ICostSymbol : IResourceEntrySymbol
{
    decimal Value { get; }

    new ICostTypeSymbol Type { get; }
}
