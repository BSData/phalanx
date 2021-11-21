namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class Symbol : ISymbol
{
    public abstract SymbolKind Kind { get; }

    public abstract string Name { get; }

    public abstract string? Comment { get; }

    public abstract ISymbol? ContainingSymbol { get; }

    internal abstract Compilation DeclaringCompilation { get; }
}
