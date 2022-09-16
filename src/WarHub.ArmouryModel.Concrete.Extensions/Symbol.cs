using System.Diagnostics;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class Symbol : ISymbol
{
    public abstract SymbolKind Kind { get; }

    public abstract string? Id { get; }

    public abstract string Name { get; }

    public abstract string? Comment { get; }

    public abstract ISymbol? ContainingSymbol { get; }

    public virtual IGamesystemNamespaceSymbol? ContainingNamespace => ContainingSymbol switch
    {
        { Kind: SymbolKind.Namespace } => throw new InvalidOperationException("Namespace child must override ContainingNamespace."),
        _ => ContainingSymbol?.ContainingNamespace,
    };

    public virtual IModuleSymbol? ContainingModule => ContainingSymbol switch
    {
        { Kind: SymbolKind.Catalogue } x => (IModuleSymbol)x,
        { Kind: SymbolKind.Roster } x => (IModuleSymbol)x,
        _ => ContainingSymbol?.ContainingModule
    };

    internal virtual WhamCompilation? DeclaringCompilation => Kind switch
    {
        SymbolKind.Namespace => throw new InvalidOperationException("Namespace must override DeclaringCompilation."),
        _ => (ContainingNamespace as SourceGlobalNamespaceSymbol)?.DeclaringCompilation
    };

    internal virtual void AddDeclarationDiagnostics(BindingDiagnosticBag diagnostics)
    {
        if (diagnostics.DiagnosticBag is { IsEmptyWithoutResolution: false } || diagnostics.DependenciesBag?.Count > 0)
        {
            var compilation = DeclaringCompilation;
            Debug.Assert(compilation != null);

            // TODO implement AddUsedAssemblies first
            // compilation.AddUsedAssemblies(diagnostics.DependenciesBag);

            if (diagnostics.DiagnosticBag is { IsEmptyWithoutResolution: false })
            {
                compilation.DeclarationDiagnostics.AddRange(diagnostics.DiagnosticBag);
            }
        }
    }

    /// <summary>
    /// Reports specified use-site diagnostic to given diagnostic bag. 
    /// </summary>
    /// <remarks>
    /// This method should be the only method adding use-site diagnostics to a diagnostic bag. 
    /// It may perform additional adjustments of the location for unification related diagnostics and 
    /// may be the place where to add more use-site location post-processing.
    /// </remarks>
    /// <returns>True if the diagnostic has error severity.</returns>
    internal static bool ReportUseSiteDiagnostic(DiagnosticInfo info, DiagnosticBag diagnostics, Location location)
    {
        diagnostics.Add(info, location);
        return info.Severity == DiagnosticSeverity.Error;
    }

    /// <summary>
    /// True if the symbol has a use-site diagnostic with error severity.
    /// </summary>
    internal bool HasUseSiteError
    {
        get
        {
            var info = GetUseSiteInfo();
            return info.DiagnosticInfo?.Severity == DiagnosticSeverity.Error;
        }
    }

    /// <summary>
    /// Returns diagnostic info that should be reported at the use site of the symbol, or default if there is none.
    /// </summary>
    internal virtual UseSiteInfo<IModuleSymbol> GetUseSiteInfo() => default;

    /// <summary>
    /// True if this Symbol should be completed by calling ForceComplete.
    /// Intuitively, true for source entities (from any compilation).
    /// </summary>
    internal virtual bool RequiresCompletion => false;

    internal virtual void ForceComplete(CancellationToken cancellationToken)
    {
        // must be overridden by source symbols, no-op for other symbols
        Debug.Assert(!RequiresCompletion);
    }

    internal virtual bool HasComplete(CompletionPart part)
    {
        // must be overridden by source symbols, no-op for other symbols
        Debug.Assert(!RequiresCompletion);
        return true;
    }

    internal virtual ImmutableArray<ISymbol> GetMembers() => ImmutableArray<ISymbol>.Empty;

    public abstract void Accept(SymbolVisitor visitor);

    public abstract TResult Accept<TResult>(SymbolVisitor<TResult> visitor);

    public abstract TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument);
}
