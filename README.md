# phalanx

Project Phalanx

## TODO

- binding when fails return invalid/missing symbols, not null
- drop GamesystemContext and Dataset types
- symbol layer:
  - symbolinfo that might be an error
  - implementation for: constraints, logic (conditions, modifiers), roster entitites
  - binder factory
  - binders for sub-catalogue levels
- add default subselections:
  - for entries with constraints min > 0 (constraints need symbols?)
  - entry groups
- category links of two types (force entry child and selection entry child)?

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
