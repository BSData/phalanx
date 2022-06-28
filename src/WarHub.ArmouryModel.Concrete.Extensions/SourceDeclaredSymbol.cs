using System.Diagnostics;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class SourceDeclaredSymbol : Symbol, INodeDeclaredSymbol<SourceNode>
{
    protected SymbolCompletionState state;
    private ImmutableArray<Symbol> lazyMembers;

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

    public virtual SourceNode Declaration { get; }

    public sealed override ISymbol? ContainingSymbol { get; }

    public override string? Id { get; }

    public override string Name { get; }

    public override string? Comment { get; }

    internal sealed override bool RequiresCompletion => true;

    internal override WhamCompilation DeclaringCompilation
    {
        get
        {
            return base.DeclaringCompilation
                ?? throw new InvalidOperationException("Source symbols must have a declaring compilation set.");
        }
    }

    internal sealed override bool HasComplete(CompletionPart part) => state.HasComplete(part);

    internal override void ForceComplete(CancellationToken cancellationToken)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var incompletePart = state.NextIncompletePart;
            switch (incompletePart)
            {
                case CompletionPart.None:
                    return;
                case CompletionPart.StartBindingReferences:
                case CompletionPart.FinishBindingReferences:
                    BindReferences();
                    break;
                case CompletionPart.Members:
                    GetMembersCore();
                    break;
                case CompletionPart.MembersCompleted:
                    {
                        var members = GetMembersCore();
                        foreach (var member in members)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            member.ForceComplete(cancellationToken);
                        }
                        state.NotePartComplete(CompletionPart.MembersCompleted);
                        break;
                    }
                default:
                    // This assert will trigger if we forgot to handle any of the completion parts
                    Debug.Assert((incompletePart & CompletionPart.SourceDeclaredSymbolAll) == 0);
                    // any other values are completion parts intended for other kinds of symbols
                    state.NotePartComplete(CompletionPart.All & ~CompletionPart.SourceDeclaredSymbolAll);
                    break;
            }
            state.SpinWaitComplete(incompletePart, cancellationToken);
        }
        throw new InvalidOperationException("Unreachable code.");
    }

    internal sealed override ImmutableArray<ISymbol> GetMembers() => GetMembersCore().Cast<Symbol, ISymbol>();

    protected ImmutableArray<Symbol> GetMembersCore()
    {
        if (state.HasComplete(CompletionPart.Members))
        {
            return lazyMembers!;
        }
        return GetMembersCoreSlow();
    }

    protected ImmutableArray<Symbol> GetMembersCoreSlow()
    {
        if (lazyMembers.IsDefault)
        {
            var diagnostics = BindingDiagnosticBag.GetInstance();
            var members = MakeAllMembers(diagnostics);
            if (ImmutableInterlocked.InterlockedCompareExchange(ref lazyMembers, members, default).IsDefault)
            {
                AddDeclarationDiagnostics(diagnostics);
                state.NotePartComplete(CompletionPart.Members);
            }
            diagnostics.Free();
        }
        state.SpinWaitComplete(CompletionPart.Members, default);
        return lazyMembers;
    }

    protected virtual ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        ImmutableArray<Symbol>.Empty;

    protected void BindReferences()
    {
        if (state.HasComplete(CompletionPart.ReferencesCompleted))
        {
            return;
        }
        if (state.NotePartComplete(CompletionPart.StartBindingReferences))
        {
            var diagnostics = BindingDiagnosticBag.GetInstance();
            BindReferencesCore(DeclaringCompilation.GetBinder(Declaration, ContainingSymbol), diagnostics);
            AddDeclarationDiagnostics(diagnostics);
            state.NotePartComplete(CompletionPart.FinishBindingReferences);
        }
    }

    protected virtual void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
    }

    protected T GetBoundField<T>(ref T? field) where T : class
    {
        if (!state.HasComplete(CompletionPart.ReferencesCompleted))
        {
            BindReferences();
        }
        return field ?? throw new InvalidOperationException("Bound field was null after binding.");
    }


    protected T? GetOptionalBoundField<T>(ref T? field)
    {
        if (!state.HasComplete(CompletionPart.ReferencesCompleted))
        {
            BindReferences();
        }
        return field;
    }
}
