namespace WarHub.ArmouryModel;

/// <summary>
/// Declares what operation the effect causes.
/// </summary>
public enum EffectOperation
{
    /// <summary>
    /// The effect causes no changes. This might be used e.g. for effects who only have child effects,
    /// as a means of grouping effects under a single condition or the same repetition.
    /// </summary>
    None,

    /// <summary>
    /// This effect applies to another effect, repeating its application a number of times
    /// resulting from <see cref="IEffectSymbol.RepetitionQuery"/> result multiplied by <see cref="IEffectSymbol.Repetitions"/>.
    /// </summary>
    RepeatEffect,

    /// <summary>
    /// Increments target field's value (characteristic, constraint or Publication Page, other numeric).
    /// </summary>
    IncrementValue,

    /// <summary>
    /// Decrements target field's value (characteristic, constraint or Publication Page, other numeric).
    /// </summary>
    DecrementValue,

    /// <summary>
    /// Changes target field's value (characteristic, constraint, entry Name, publication Page, entry Hidden state, other text)
    /// to a new value specified by <see cref="IEffectSymbol.OperandValue"/>.
    /// </summary>
    SetValue,

    /// <summary>
    /// Changes target field's text value (characteristic, entry Name, publication Page, other text)
    /// to a new value by appending <see cref="IEffectSymbol.OperandValue"/> at the end of original value.
    /// </summary>
    AppendText,

    /// <summary>
    /// Adds a <see cref="ICategoryEntrySymbol"/> specified by <see cref="IEffectSymbol.OperandSymbol"/>
    /// to the symbol containing the effect.
    /// </summary>
    AddCategory,

    /// <summary>
    /// Removes a <see cref="ICategoryEntrySymbol"/> specified by <see cref="IEffectSymbol.OperandSymbol"/>
    /// from the symbol containing the effect.
    /// </summary>
    RemoveCategory,

    /// <summary>
    /// Sets a <see cref="ICategoryEntrySymbol"/> specified by <see cref="IEffectSymbol.OperandSymbol"/>
    /// as the primary for the symbol containing the effect.
    /// </summary>
    SetCategoryPrimary,

    /// <summary>
    /// Unsets a <see cref="ICategoryEntrySymbol"/> specified by <see cref="IEffectSymbol.OperandSymbol"/>
    /// as the primary for the symbol containing the effect.
    /// </summary>
    UnsetCategoryPrimary,
}
