using WarHub.ArmouryModel.Source;
using static WarHub.ArmouryModel.Source.NodeFactory;

namespace WarHub.ArmouryModel.EditorServices;

public delegate TNode NodeSelector<TRoot, TNode>(TRoot root)
    where TRoot : IRootNode where TNode : SourceNode;

/// <summary>
/// Provides methods that change roster state.
/// </summary>
public record RosterOperationBuilder(RosterState State)
{
    public RosterNode Roster => State.Roster;
    public GamesystemNode Gamesystem => State.Dataset.Gamesystem;
    public ImmutableArray<CatalogueNode> Catalogues => State.Dataset.Catalogues;
    public static CreateRosterOperationBuilder Create(Dataset dataset) => new(dataset);
    public ChangeCostLimitOperationBuilder ChangeCostLimit(CostLimitNode costLimit) => new(costLimit);
    public AddForceOperationBuilder AddForce(ForceEntryNode forceEntry) => new(forceEntry);
    public AddSelectionOperationBuilder AddSelection(SelectionEntryNode selectionEntry) => new(selectionEntry);
    public IRosterOperation RemoveForce(ForceNode force) => new RemoveForceOperationBuilder(force).Build();
    public IRosterOperation RemoveSelection(SelectionNode selection) => new RemoveSelectionOperationBuilder(selection).Build();
    public ChangeSelectionCountOperationBuilder ChangeCountOf(SelectionNode selection) => new(selection);

    // probably an extension method
    public RosterOperationBuilder Apply(Func<RosterOperationBuilder, IRosterOperation> operationSelector) =>
        this with { State = operationSelector(this).Apply(State) };

}
