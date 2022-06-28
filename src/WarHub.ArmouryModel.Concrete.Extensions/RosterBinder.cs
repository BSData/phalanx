namespace WarHub.ArmouryModel.Concrete;

internal class RosterBinder : Binder
{
    public RosterSymbol Roster { get; }

    internal RosterBinder(Binder next, RosterSymbol roster) : base(next)
    {
        Roster = roster;
    }

    internal override Symbol? ContainingSymbol => Roster;
}
