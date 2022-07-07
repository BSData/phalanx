namespace WarHub.ArmouryModel.Concrete;

internal class RosterBinder : Binder
{
    internal RosterBinder(Binder next, RosterSymbol roster) : base(next)
    {
        Roster = roster;
    }

    public RosterSymbol Roster { get; }

    internal override Symbol? ContainingSymbol => Roster;

    internal override ContainerEntryBaseSymbol? ContainingContainerSymbol => null;

    internal override EntrySymbol? ContainingEntrySymbol => null;

    internal override ForceSymbol? ContainingForceSymbol => null;

    internal override SelectionSymbol? ContainingSelectionSymbol => null;
}
