using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class SourceDeclaredSymbol : Symbol
{
    internal SourceNode Declaration { get; }

    protected SourceDeclaredSymbol(SourceNode declaration)
    {
        Id = (declaration as IIdentifiableNode)?.Id;
        Name = (declaration as INameableNode)?.Name ?? string.Empty;
        Comment = (declaration as CommentableNode)?.Comment;
        Declaration = declaration;
    }

    public override string? Id { get; }

    public sealed override string Name { get; }

    public sealed override string? Comment { get; }

    internal override bool RequiresCompletion => true;

    private bool BindingDone { get; set; }

    internal override void ForceComplete()
    {
        if (!BindingDone)
        {
            BindReferences();
        }
    }

    protected void BindReferences()
    {
        if (BindingDone)
            throw new InvalidOperationException("Already bound!");
        BindReferencesCore(DeclaringCompilation.GetBinder(Declaration), DiagnosticBag.GetInstance());
    }

    protected virtual void BindReferencesCore(Binding.Binder binder, DiagnosticBag diagnosticBag)
    {
    }
}
