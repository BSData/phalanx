using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class SourceDeclaredSymbol : Symbol
{
    internal SourceNode Declaration { get; }

    protected SourceDeclaredSymbol(
        ISymbol? containingSymbol,
        SourceNode declaration)
    {
        Id = (declaration as IIdentifiableNode)?.Id;
        Name = (declaration as INameableNode)?.Name ?? string.Empty;
        Comment = (declaration as CommentableNode)?.Comment;
        Declaration = declaration;
        ContainingSymbol = containingSymbol;
    }

    public sealed override ISymbol? ContainingSymbol { get; }

    public override string? Id { get; }

    public sealed override string Name { get; }

    public sealed override string? Comment { get; }

    internal override bool RequiresCompletion => true;

    internal override WhamCompilation DeclaringCompilation
    {
        get
        {
            return base.DeclaringCompilation
                ?? throw new InvalidOperationException("Source symbols must have a declaring compilation set.");
        }
    }

    protected sealed override void BindReferences(WhamCompilation compilation, DiagnosticBag diagnostics)
    {
        base.BindReferences(compilation, diagnostics);

        BindReferencesCore(compilation.GetBinder(Declaration, ContainingSymbol), diagnostics);
    }

    protected virtual void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
    }
}
