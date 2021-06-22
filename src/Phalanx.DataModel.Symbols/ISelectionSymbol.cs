namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Selection instance in a roster.
    /// BS Selection.
    /// WHAM <see cref="WarHub.ArmouryModel.Source.SelectionNode" />.
    /// </summary>
    public interface ISelectionSymbol : IForceOrSelectionSymbol
    {
        /// <summary>
        /// Selection count, or the number of times that selection is "taken".
        /// </summary>
        int Count { get; }

        new ISelectionEntrySymbol SourceEntry { get; }

        IForceOrSelectionSymbol Parent { get; }
    }
}
