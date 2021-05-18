using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    public interface ICoreEntrySymbol : IContainerEntrySymbol
    {
        ImmutableArray<IConstraintSymbol> Constraints { get; }
    }
}
