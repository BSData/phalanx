namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Looped application of some effects.
/// BS Repeat.
/// WHAM <see cref="WarHub.ArmouryModel.Source.RepeatNode" />.
/// </summary>
public interface ILoopEffectSymbol : IEffectSymbol
{
    ILoopCountSymbol Count { get; }

    ImmutableArray<IEffectSymbol> Effects { get; }
}
