using System.Diagnostics;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class SourceGlobalNamespaceSymbol : Symbol, IGamesystemNamespaceSymbol
{
    protected SymbolCompletionState state;

    public SourceGlobalNamespaceSymbol(
        ImmutableArray<SourceNode> rootDataNodes,
        WhamCompilation declaringCompilation)
    {
        DeclaringCompilation = declaringCompilation;
        DeclarationDiagnostics = DiagnosticBag.GetInstance();
        AllRootSymbols = rootDataNodes.Select(CreateSymbol).Where(x => x is not null).ToImmutableArray()!;
        Rosters = AllRootSymbols.OfType<RosterSymbol>().ToImmutableArray();
        Catalogues = AllRootSymbols.OfType<CatalogueBaseSymbol>().ToImmutableArray();
        RootCatalogue = GetOrCreateGamesystemSymbol();
        // TODO more diagnostics, e.g. all catalogues are from the same game system?
        state.NotePartComplete(CompletionPart.Members);

        ICatalogueSymbol GetOrCreateGamesystemSymbol()
        {
            var rootCandidates = Catalogues.Where(x => x.IsGamesystem).ToImmutableArray();
            if (rootCandidates.Length > 1)
            {
                foreach (var candidate in rootCandidates)
                    DeclarationDiagnostics.Add(ErrorCode.ERR_MultipleGamesystems, candidate.Declaration);
            }
            return rootCandidates.FirstOrDefault()
                ?? DeclaringCompilation.CreateMissingGamesystemSymbol(DeclarationDiagnostics);
        }

        Symbol? CreateSymbol(SourceNode node)
        {
            if (node is CatalogueNode catalogueNode)
            {
                return new CatalogueSymbol(this, catalogueNode, DeclarationDiagnostics);
            }
            else if (node is GamesystemNode gamesystemNode)
            {
                return new GamesystemSymbol(this, gamesystemNode, DeclarationDiagnostics);
            }
            else if (node is RosterNode rosterNode)
            {
                return new RosterSymbol(this, rosterNode, DeclarationDiagnostics);
            }
            else
            {
                DeclarationDiagnostics.Add(ErrorCode.ERR_UnknownCatalogueType, node);
                return null;
            }
        }
    }

    public override SymbolKind Kind => SymbolKind.Namespace;

    public override string? Id => RootCatalogue.Id;

    public override string Name => RootCatalogue.Name;

    public override string? Comment => null;

    public override Symbol? ContainingSymbol => null;

    public override IGamesystemNamespaceSymbol? ContainingNamespace => null;

    public override IModuleSymbol? ContainingModule => null;

    public ICatalogueSymbol RootCatalogue { get; }

    public ImmutableArray<Symbol> AllRootSymbols { get; }

    public ImmutableArray<CatalogueBaseSymbol> Catalogues { get; }

    public ImmutableArray<RosterSymbol> Rosters { get; }

    internal override WhamCompilation DeclaringCompilation { get; }

    internal DiagnosticBag DeclarationDiagnostics { get; }

    internal override bool RequiresCompletion => true;

    ImmutableArray<ICatalogueSymbol> IGamesystemNamespaceSymbol.Catalogues =>
        Catalogues.Cast<CatalogueBaseSymbol, ICatalogueSymbol>();

    ImmutableArray<IRosterSymbol> IGamesystemNamespaceSymbol.Rosters =>
        Rosters.Cast<RosterSymbol, IRosterSymbol>();

    ImmutableArray<ISymbol> IGamesystemNamespaceSymbol.AllRootSymbols =>
        AllRootSymbols.Cast<Symbol, ISymbol>();

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
                case CompletionPart.MembersCompleted:
                    {
                        foreach (var member in AllRootSymbols)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            member.ForceComplete(cancellationToken);
                        }
                        state.NotePartComplete(CompletionPart.MembersCompleted);
                        break;
                    }
                default:
                    // This assert will trigger if we forgot to handle any of the completion parts
                    Debug.Assert((incompletePart & CompletionPart.NamespaceAll) == 0);
                    // any other values are completion parts intended for other kinds of symbols
                    state.NotePartComplete(CompletionPart.All & ~CompletionPart.NamespaceAll);
                    break;
            }
            state.SpinWaitComplete(incompletePart, cancellationToken);
        }
        throw new InvalidOperationException("Unreachable code.");
    }

    public override void Accept(SymbolVisitor visitor) =>
        visitor.VisitGamesystemNamespace(this);

    public override TResult Accept<TResult>(SymbolVisitor<TResult> visitor) =>
        visitor.VisitGamesystemNamespace(this);

    public override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument) =>
        visitor.VisitGamesystemNamespace(this, argument);
}
