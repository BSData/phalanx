namespace WarHub.ArmouryModel;

/// <summary>
/// Defines cost of an entry.
/// BS Cost on SelectionEntries.
/// WHAM <see cref="Source.CostNode" />.
/// </summary>
public interface ICostSymbol : IResourceEntrySymbol
{
    decimal Value { get; }

    string TypeId { get; }
}
