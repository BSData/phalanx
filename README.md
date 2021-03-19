# phalanx
Project Phalanx


## Symbols

The symbols are the basis of semantic model object graph. `ISymbol` is the root type.

- ICatalogueSymbol
- ICatalogueItemSymbol
  - IEntrySymbol
    - IResourceEntryOrContainerSymbol
      - IResourceEntrySymbol (below)
      - IResourceContainerSymbol (below)
    - IResourceEntrySymbol
      - ICharacteristicSymbol
      - ICostSymbol
      - IProfileSymbol
      - IRuleSymbol
    - IContainerEntrySymbol
      - IResourceContainerSymbol
      - ICoreEntrySymbol
        - ICategoryEntry
        - IForceEntry
        - ISelectionEntryOrGroupSymbol
          - ISelectionEntrySymbol
          - ISelectionEntryGroupSymbol
  - IResourceTypeSymbol
    - ICharacteristicTypeSymbol
    - ICostTypeSymbol
    - IProfileTypeSymbol
  - ILogicSymbol
    - IConstraintSymbol (???)
    - IEffectSymbol
      - IModifierSymbol
      - IModifierGroupSymbol
    - ITriggerSymbol
      - IConditionSymbol
      - IConditionGroupSymbol
      - IRepeatSymbol
- IRosterSymbol
- IForceOrSelectionSymbol
  - IForceSymbol
  - ISelectionSymbol
- Undecided:
  - Publication (IResourceTypeSymbol?)
  - Links
    - CatalogueLink
    - CategoryLink
    - EntryLink
    - InfoLink
  - Category