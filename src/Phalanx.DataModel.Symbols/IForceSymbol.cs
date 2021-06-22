using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Force instance in a roster.
    /// BS Force.
    /// WHAM <see cref="WarHub.ArmouryModel.Source.ForceNode" />.
    /// </summary>
    public interface IForceSymbol : IForceOrSelectionSymbol
    {
        new IForceEntrySymbol SourceEntry { get; }

        ImmutableArray<IForceSymbol> ChildForces { get; }
    }
}
