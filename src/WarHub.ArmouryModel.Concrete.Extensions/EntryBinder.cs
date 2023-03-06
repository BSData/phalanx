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

    internal override void LookupSymbolsInSingleBinder(LookupResult result, string symbolId, LookupOptions options, Binder originalBinder, bool diagnose, Symbol? qualifier)
    {
        if (qualifier is EntrySymbol entrySymbol && ReferenceEquals(entrySymbol, EntrySymbol))
        {
            LookupSymbolInQualifyingEntry(entrySymbol, result, symbolId, options, originalBinder, diagnose);
        }
    }
}
