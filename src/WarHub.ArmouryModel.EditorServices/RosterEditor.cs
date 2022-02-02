namespace WarHub.ArmouryModel.EditorServices;

/// <summary>
/// Provides methods that change roster state. Thread-safe, allows editing roster.
/// Supports undo-redo stack of edits beginning with the initial roster state.
/// </summary>
public class RosterEditor
{
    private readonly object lockObject = new();
    /// <summary>
    /// This stack
    /// </summary>
    private ImmutableStack<(RosterState state, IRosterOperation operation)> stateStack
        = ImmutableStack<(RosterState state, IRosterOperation operation)>.Empty;
    private ImmutableStack<(RosterState state, IRosterOperation operation)> redoStack
        = ImmutableStack<(RosterState state, IRosterOperation operation)>.Empty;

    public RosterEditor(RosterState state)
    {
        stateStack = stateStack.Push((state, RosterOperations.Identity));
    }

    public RosterState State => stateStack.Peek().state;

    public bool CanUndo => !stateStack.Pop().IsEmpty;

    public bool CanRedo => !redoStack.IsEmpty;

    public void ApplyOperation(IRosterOperation operation)
    {
        lock (lockObject)
        {
            var newState = operation.Apply(State);
            stateStack = stateStack.Push((newState, operation));
            redoStack = redoStack.Clear();
        }
    }

    public bool Undo()
    {
        lock (lockObject)
        {
            var previousStack = stateStack.Pop(out var current);
            if (previousStack.IsEmpty)
            {
                return false;
            }
            stateStack = previousStack;
            redoStack = redoStack.Push(current);
            return true;
        }
    }

    public bool Redo()
    {
        lock (lockObject)
        {
            if (redoStack.IsEmpty)
            {
                return false;
            }
            redoStack = redoStack.Pop(out var redo);
            stateStack = stateStack.Push(redo);
            return true;
        }
    }
}
