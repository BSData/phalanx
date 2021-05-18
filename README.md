# phalanx
Project Phalanx

## TODO
- symbol layer?
  - consider phases of building the symbols
  - symbolinfo that might be an error
  - late-bound links?
  - indexes - resolving targets/ids for links, constraints, modifiers, conditions
  - references/back-references
- add default subselections:
  - for entries with constraints min > 0 (constraints need symbols?)
  - entry groups
- support links (requires symbols)
- category links of two types (force entry child and selection entry child)
- nested forces

## Symbols

The symbols are the basis of semantic model object graph. `ISymbol` is the root type.

- ICatalogueSymbol (+ catalogue links?)
- ICatalogueItemSymbol
  - IEntrySymbol //contains IEffectSymbol
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
      - IResourceContainerSymbol // contain IResourceEntrySymbols
      - ICoreEntrySymbol // contains IConstraintSymbols
        - ICategoryEntry (+ category links?)
        - IForceEntry
        - ISelectionEntryContainerSymbol // contains other SelectionEntry-like stuff
          - ISelectionEntrySymbol
          - ISelectionEntryGroupSymbol
          - ISelectionEntryLinkSymbol
  - IResourceTypeSymbol
    - ICharacteristicTypeSymbol
    - ICostTypeSymbol
    - IProfileTypeSymbol
    - IPublicationSymbol
  - ILogicSymbol
    - IConstraintSymbol
    - IEffectSymbol
      - IConditionalEffectSymbol
      - ILoopEffectSymbol
      - IModifyingEffectSymbol
    - IConditionSymbol
      - IQueryConditionSymbol // condition/constaint/repeat "query" part
      - ITupleOperationConditionSymbol // condition groups
    - IQuerySymbol
- IRosterSymbol
- IForceOrSelectionSymbol
  - IForceSymbol
  - ISelectionSymbol
- roster category ?
- Undecided:
  - Links
    - CatalogueLink
    - CategoryLink
    - EntryLink
    - InfoLink
  - Category
