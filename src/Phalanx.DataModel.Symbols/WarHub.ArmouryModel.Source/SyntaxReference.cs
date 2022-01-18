using WarHub.ArmouryModel.Source.Text;

namespace WarHub.ArmouryModel.Source;

/// <summary>
/// A reference to a syntax node.
/// </summary>
public abstract class SyntaxReference
{
    /// <summary>
    /// The syntax tree that this references a node within.
    /// </summary>
    public abstract SourceTree SyntaxTree { get; }

    /// <summary>
    /// The span of the node referenced.
    /// </summary>
    public abstract TextSpan Span { get; }

    /// <summary>
    /// Retrieves the original referenced syntax node.  
    /// This action may cause a parse to happen to recover the syntax node.
    /// </summary>
    /// <returns>The original referenced syntax node.</returns>
    public abstract SourceNode GetSyntax(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the original referenced syntax node.  
    /// This action may cause a parse to happen to recover the syntax node.
    /// </summary>
    /// <returns>The original referenced syntax node.</returns>
    public virtual Task<SourceNode> GetSyntaxAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(GetSyntax(cancellationToken));
    }

    /// <summary>
    /// The location of this syntax reference.
    /// </summary>
    /// <returns>The location of this syntax reference.</returns>
    /// <remarks>
    /// More performant than GetSyntax().GetLocation().
    /// </remarks>
    public Location GetLocation()
    {
        return SyntaxTree.GetLocation(Span);
    }
}
