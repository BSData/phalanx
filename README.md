# phalanx
Project Phalanx

## TODO
- symbol layer?
- add default subselections:
  - for entries with constraints min > 0 (constraints need symbols?)
  - entry groups
- support links (requires symbols)
- category links of two types (force entry child and selection entry child)

## Symbols

The symbols are the basis of semantic model object graph. `ISymbol` is the root type.

- ICatalogueSymbol (+ catalogue links?)
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
      - info links ?
    - IContainerEntrySymbol
      - IResourceContainerSymbol
      - ICoreEntrySymbol
        - ICategoryEntry (+ category links?)
        - IForceEntry
        - ISelectionEntryOrGroupSymbol
          - ISelectionEntrySymbol
          - ISelectionEntryGroupSymbol
          - ? IEntryLink
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
- roster category ?
- Undecided:
  - Publication (IResourceTypeSymbol?)
  - Links
    - CatalogueLink
    - CategoryLink
    - EntryLink
    - InfoLink
  - Category
