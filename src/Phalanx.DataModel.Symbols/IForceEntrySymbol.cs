using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    public interface IForceEntrySymbol : ICoreEntrySymbol
    {
        ImmutableArray<IForceEntrySymbol> NestedForces { get; }
    }
}
