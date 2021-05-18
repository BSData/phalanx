using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    public interface IContainerEntrySymbol : IEntrySymbol
    {
        ImmutableArray<IResourceEntryOrContainerSymbol> Resources { get; }
    }
}
