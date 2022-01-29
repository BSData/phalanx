# phalanx

Project Phalanx

This is a project to create an alternative roster editor that uses BattleScribe-format datafiles.

Our main communication channel beside Issues, PRs and Discussions on GitHub is the `#project-phalanx` channel on BSData Discord server.
[![BSData Discord link](https://img.shields.io/discord/558412685981777922?style=popout-square)](https://www.bsdata.net/discord)

Please see [Wiki](https://github.com/BSData/phalanx/wiki) for more information.

## TODO

- test default group entry binding - how it works in BattleScribe (allowed selections), does it work same in Phalanx
- consider removing "deeper" types than IResourceDefinition/IResourceEntry/IContainerEntry
  - simpler usage
  - requires moving their "custom" properties somewhere: "ContentFields" collection of custom name-value wrappers, maybe strongly typed?
- drop Dataset type
- symbol layer:
  - symbolinfo that might be an error
  - implementation for: constraints, logic (conditions, modifiers), roster entitites
  - binder factory
  - binders for sub-catalogue levels
- add default subselections:
  - for entries with constraints min > 0 (constraints need symbols?)
  - entry groups
  - should that be in a separate "RosterEditor" module?
- category links of two types (force entry child and selection entry child)?
- implement Logic symbols
- implement Roster symbols

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
