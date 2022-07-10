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

    internal override void LookupSymbolsInSingleBinder(LookupResult result, string symbolId, LookupOptions options, Binder originalBinder, bool diagnose, ISymbol? qualifier)
    {
        if (options.HasFlag(LookupOptions.CatalogueOnly))
        {
            return;
        }
        foreach (var catalogue in GetRosterCatalogues())
        {
            LookupSymbolsInSingleCatalogue(catalogue, result, symbolId, options, originalBinder, diagnose);
            if (result.IsMultiViable)
                return;
        }
    }

    internal IEnumerable<ICatalogueSymbol> GetRosterCatalogues()
    {
        yield return Roster.Gamesystem;
        var set = new HashSet<string> { Roster.Declaration.GameSystemId! };
        var forceQueue = ImmutableQueue.Create<IForceContainerSymbol>(Roster);
        while (!forceQueue.IsEmpty)
        {
            forceQueue = forceQueue.Dequeue(out var container);
            if (container is IForceSymbol force && set.Add(force.CatalogueReference.Id!))
            {
                yield return force.CatalogueReference.Catalogue;
            }
            foreach (var item in container.Forces)
            {
                forceQueue = forceQueue.Enqueue(item);
            }
        }
    }
}
