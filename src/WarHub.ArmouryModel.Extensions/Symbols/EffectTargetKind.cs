namespace WarHub.ArmouryModel;

/// <summary>
/// Declares which field or descendant symbol of an Entry does the effect apply to.
/// </summary>
public enum EffectTargetKind
{
    /// <summary>
    /// There is no target specified.
    /// </summary>
    None,

    /// <summary>
    /// The containing effect is affected (e.g. a repeating effect on another effect).
    /// </summary>
    Effect,

    /// <summary>
    /// The <see cref="ISymbol.Name"/> text is affected.
    /// </summary>
    SymbolName,

    /// <summary>
    /// The <see cref="IEntrySymbol.IsHidden"/> value is affected.
    /// </summary>
    EntryHiddenState,

    /// <summary>
    /// The <see cref="ISelectionEntryContainerSymbol.Categories"/> list is affected.
    /// </summary>
    EntryCategory,

    /// <summary>
    /// The <see cref="IPublicationReferenceSymbol.Page"/> of <see cref="IEntrySymbol.PublicationReference"/>
    /// is affected.
    /// </summary>
    PublicationPage,

    /// <summary>
    /// Value of the <see cref="IRuleSymbol.DescriptionText"/> is changed.
    /// </summary>
    RuleDescription,

    /// <summary>
    /// A member symbol of the affected symbol has its value changed.
    /// Possibilites: Constraint boundary value, Cost value, Characteristic value.
    /// The <see cref="IQuerySymbol.ReferenceValue"/> of <see cref="IConstraintSymbol.Query"/>
    /// of one of the <see cref="IContainerEntrySymbol.Constraints"/> is affected.
    /// </summary>
    Member,
}
