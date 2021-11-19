namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Calculates the number of times a <see cref="ILoopEffectSymbol"/> should be applied.
/// BS Repeat.
/// WHAM <see cref="WarHub.ArmouryModel.Source.RepeatNode" />.
/// </summary>
public interface ILoopCountSymbol : ILogicSymbol
{

    int LoopBaseCount { get; }
    int QueryDivisionCount { get; }
    IQuerySymbol Query { get; }
}