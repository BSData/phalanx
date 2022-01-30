namespace WarHub.ArmouryModel.EditorServices;

/// <summary>
/// Provides methods that change roster state.
/// </summary>
public class RosterEditor
{
    private readonly object lockObject = new();
    private RosterState state;
    private IRosterOperation? operation;

    public RosterEditor(RosterState state)
    {
        this.state = state;
    }

    public RosterState State => GetCurrentState();

    public void AddOperation(IRosterOperation operation)
    {
        lock (lockObject)
        {
            this.operation = this.operation is null ? operation : this.operation.With(operation);
        }
    }

    private RosterState GetCurrentState()
    {
        lock (lockObject)
        {
            if (operation is not null)
            {
                (state, operation) = (operation.Apply(state), null);
            }
            return state;
        }
    }
}
