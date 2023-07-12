using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel;

/// <summary>
/// A program location in source code.
/// </summary>
internal sealed class SourceLocation : Location, IEquatable<SourceLocation?>
{
    private readonly SourceTree syntaxTree;
    private readonly TextSpan span;

    public SourceLocation(SourceTree syntaxTree, TextSpan span)
    {
        this.syntaxTree = syntaxTree;
        this.span = span;
    }

    // TODO SourceNode doesn't have SourceTree/Span currently
    // public SourceLocation(SourceNode node)
    //     : this(node.SourceTree, node.Span)
    // {
    // }

    public SourceLocation(SyntaxReference syntaxRef)
        : this(syntaxRef.SyntaxTree, syntaxRef.Span)
    {
    }

    public override LocationKind Kind => LocationKind.SourceFile;

    public override TextSpan SourceSpan => span;

    public override SourceTree SourceTree => syntaxTree;

    public override FileLinePositionSpan GetLineSpan()
    {
        // If there's no syntax tree (e.g. because we're binding speculatively),
        // then just return an invalid span.
        if (syntaxTree == null)
        {
            return default;
        }

        return syntaxTree.GetLineSpan(span);
    }

    public bool Equals(SourceLocation? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other != null && other.syntaxTree == syntaxTree && other.span == span;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as SourceLocation);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(syntaxTree, span.GetHashCode());
    }

    protected override string GetDebuggerDisplay()
    {
        return string.Concat(base.GetDebuggerDisplay(), "\"", syntaxTree.ToString().AsSpan(span.Start, span.Length), "\"");
    }
}
