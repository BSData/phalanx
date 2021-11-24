using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class SourceCatalogueItemSymbol : CatalogueItemSymbol
{
    internal SourceNode Declaration { get; }

    protected SourceCatalogueItemSymbol(
        ICatalogueItemSymbol containingSymbol,
        SourceNode declaration)
        : base(containingSymbol)
    {
        Id = (declaration as IIdentifiableNode)?.Id;
        Name = (declaration as INameableNode)?.Name ?? string.Empty;
        Comment = (declaration as CommentableNode)?.Comment;
        this.Declaration = declaration;
    }

    public string? Id { get; }

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
