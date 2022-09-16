using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class SelectionBinder : Binder
{
    internal SelectionBinder(Binder next, SelectionSymbol selection) : base(next)
    {
        Selection = selection;
    }

    public SelectionSymbol Selection { get; }

    internal override Symbol? ContainingSymbol => Selection;

    internal override SelectionSymbol? ContainingSelectionSymbol => Selection;

    internal override ContainerEntryBaseSymbol? ContainingContainerSymbol => null;

    internal override EntrySymbol? ContainingEntrySymbol => null;
}
