using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Binding;

public class BinderFactory
{
    private readonly BuckStopsHereBinder buckStopsHereBinder;

    public BinderFactory(Compilation compilation, SourceTree sourceTree)
    {
        Compilation = compilation;
        SourceTree = sourceTree;
        buckStopsHereBinder = new BuckStopsHereBinder(compilation);
    }

    public Compilation Compilation { get; }

    public SourceTree SourceTree { get; }

    public Binder GetBinder(SourceNode node, ISymbol? containingSymbol = null)
    {
        // TODO implement
        throw new NotImplementedException();
    }
}
