namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Selection instance in a roster.
    /// BS Selection.
    /// WHAM <see cref="WarHub.ArmouryModel.Source.SelectionNode" />.
    /// </summary>
    public interface ISelectionSymbol : IForceOrSelectionSymbol
    {
        new ISelectionEntrySymbol SourceEntry { get; }

        IForceOrSelectionSymbol Parent { get; }
    }
}
