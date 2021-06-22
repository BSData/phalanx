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

The symbols are the basis of semantic model object graph.

### Symbol notes

- `ISymbol` is the root type.
- Links (BS EntryLink/InfoLink/CategoryLink) are represented as the symbol type
that the link points to, its `IsReference` is `true` and `ReferencedEntry` points
to the link target.

### Symbol list

- ICatalogueSymbol (+ catalogue links?)
- ICatalogueItemSymbol // has catalogue ref
  - ICatalogueReferenceSymbol // catalogue link
  - IResourceDefinitionSymbol
    - ICharacteristicTypeSymbol
    - ICostTypeSymbol
    - IProfileTypeSymbol
    - IPublicationSymbol
  - IEntrySymbol // contains effects
    - IResourceEntrySymbol
      - ICharacteristicSymbol
      - ICostSymbol
      - IProfileSymbol (can be a link)
      - IRuleSymbol (can be a link)
      - IResourceGroupSymbol (can be a link)
    - IContainerEntrySymbol // contains constraints, resources
      - ICategoryEntry (can be a link)
      - IForceEntry
      - ISelectionEntryContainerSymbol // contains other SelectionEntry-like stuff
        - ISelectionEntrySymbol (can be a link)
        - ISelectionEntryGroupSymbol (can be a link)
  - ILogicSymbol
    - IConstraintSymbol // contains effects
    - IEffectSymbol
      - IConditionalEffectSymbol
      - ILoopEffectSymbol
      - IModifyingEffectSymbol
    - IConditionSymbol
      - IQueryConditionSymbol // condition/constaint/repeat "query" part
      - ITupleOperationConditionSymbol // condition groups
    - IQuerySymbol
- IRosterSymbol
- IRosterItemSymbol // has roster ref
  - IRosterCostSymbol
  - IRosterEntrySymbol // has SourceEntry
    - IForceOrSelectionSymbol // has selections
      - IForceSymbol
      - ISelectionSymbol
