using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Looped application of some effects.
    /// BS Repeat and Modifier
    /// </summary>
    public interface ILoopEffectSymbol : IEffectSymbol
    {
        ILoopCountSymbol Count { get; }

        ImmutableArray<IEffectSymbol> Effects { get; }
    }
}
