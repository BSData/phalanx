namespace WarHub.ArmouryModel;

/// <summary>
/// Looped application of some effects.
/// BS Repeat.
/// WHAM <see cref="Source.RepeatNode" />.
/// </summary>
public interface ILoopEffectSymbol : IEffectSymbol
{
    ILoopCountSymbol Count { get; }

    ImmutableArray<IEffectSymbol> Effects { get; }
}
