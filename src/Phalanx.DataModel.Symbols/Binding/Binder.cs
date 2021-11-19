using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Binding;

public class Binder
{
    protected Binder(Compilation compilation)
    {
        Compilation = compilation;
        if (this is not BuckStopsHereBinder)
            throw new InvalidOperationException();
    }

    protected Binder(Binder next)
    {
        Next = next ?? throw new ArgumentNullException(nameof(next));
        Compilation = next.Compilation;
    }

    internal Compilation Compilation { get; }

    protected Binder? Next { get; }

    public virtual IPublicationSymbol? BindPublicationSymbol(IPublicationReferencingNode node)
    {
        return Next!.BindPublicationSymbol(node);
    }
}
