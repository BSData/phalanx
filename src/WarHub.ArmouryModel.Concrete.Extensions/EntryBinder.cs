namespace WarHub.ArmouryModel.Concrete;

internal class EntryBinder : Binder
{
    public EntryBinder(EntrySymbol entrySymbol, Binder next) : base(next)
    {
        EntrySymbol = entrySymbol;
    }

    public EntrySymbol EntrySymbol { get; }

    internal override Symbol? ContainingSymbol => EntrySymbol;

    internal override EntrySymbol? ContainingEntrySymbol => EntrySymbol;
}
