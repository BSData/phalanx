using System.Diagnostics;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel;

/// <summary>
/// This is base class for a bag used to accumulate information while binding is performed.
/// Including diagnostic messages and dependencies in the form of "used" assemblies.
/// From https://sourceroslyn.io/#Microsoft.CodeAnalysis/Binding/BindingDiagnosticBag.cs
/// </summary>
internal abstract class BindingDiagnosticBag
{
    public readonly DiagnosticBag? DiagnosticBag;

    protected BindingDiagnosticBag(DiagnosticBag? diagnosticBag)
    {
        DiagnosticBag = diagnosticBag;
    }

    internal bool AccumulatesDiagnostics => DiagnosticBag is not null;

    internal void AddRange<T>(ImmutableArray<T> diagnostics) where T : Diagnostic
    {
        DiagnosticBag?.AddRange(diagnostics);
    }

    internal void AddRange(IEnumerable<Diagnostic> diagnostics)
    {
        DiagnosticBag?.AddRange(diagnostics);
    }

    internal bool HasAnyResolvedErrors()
    {
        Debug.Assert(DiagnosticBag is not null);
        return DiagnosticBag?.HasAnyResolvedErrors() == true;
    }

    internal bool HasAnyErrors()
    {
        Debug.Assert(DiagnosticBag is not null);
        return DiagnosticBag?.HasAnyErrors() == true;
    }

    internal void Add(Diagnostic diag)
    {
        DiagnosticBag?.Add(diag);
    }
}

/// <summary>
/// From https://sourceroslyn.io/#Microsoft.CodeAnalysis/Binding/BindingDiagnosticBag.cs
/// </summary>
internal abstract class BindingDiagnosticBag<TModuleSymbol> : BindingDiagnosticBag
    where TModuleSymbol : class, IModuleSymbol
{
    public readonly ICollection<TModuleSymbol>? DependenciesBag;

    protected BindingDiagnosticBag(DiagnosticBag? diagnosticBag, ICollection<TModuleSymbol>? dependenciesBag)
        : base(diagnosticBag)
    {
        Debug.Assert(diagnosticBag?.GetType().IsValueType != true);
        DependenciesBag = dependenciesBag;
    }

    protected BindingDiagnosticBag(bool usePool)
        : this(usePool ? DiagnosticBag.GetInstance() : new DiagnosticBag(), usePool ? PooledHashSet<TModuleSymbol>.GetInstance() : new HashSet<TModuleSymbol>())
    { }

    internal bool AccumulatesDependencies => DependenciesBag is not null;

    internal void Free()
    {
        DiagnosticBag?.Free();
        ((PooledHashSet<TModuleSymbol>?)DependenciesBag)?.Free();
    }

    internal ImmutableBindingDiagnostic<TModuleSymbol> ToReadOnly()
    {
        return new ImmutableBindingDiagnostic<TModuleSymbol>(DiagnosticBag?.ToReadOnly() ?? default, DependenciesBag?.ToImmutableArray() ?? default);
    }

    internal ImmutableBindingDiagnostic<TModuleSymbol> ToReadOnlyAndFree()
    {
        var result = ToReadOnly();
        Free();
        return result;
    }

    internal void AddRangeAndFree(BindingDiagnosticBag<TModuleSymbol> other)
    {
        AddRange(other);
        other.Free();
    }

    internal void Clear()
    {
        DiagnosticBag?.Clear();
        DependenciesBag?.Clear();
    }

    internal void AddRange(ImmutableBindingDiagnostic<TModuleSymbol> other, bool allowMismatchInDependencyAccumulation = false)
    {
        AddRange(other.Diagnostics);
        Debug.Assert(allowMismatchInDependencyAccumulation || other.Dependencies.IsDefaultOrEmpty || this.AccumulatesDependencies || !this.AccumulatesDiagnostics);
        AddDependencies(other.Dependencies);
    }

    internal void AddRange(BindingDiagnosticBag<TModuleSymbol>? other, bool allowMismatchInDependencyAccumulation = false)
    {
        if (other is not null)
        {
            AddRange(other.DiagnosticBag);
            Debug.Assert(allowMismatchInDependencyAccumulation || !other.AccumulatesDependencies || this.AccumulatesDependencies);
            AddDependencies(other.DependenciesBag);
        }
    }

    internal void AddRange(DiagnosticBag? bag)
    {
        if (bag is not null)
        {
            DiagnosticBag?.AddRange(bag);
        }
    }

    internal void AddDependency(TModuleSymbol? dependency)
    {
        if (dependency is object && DependenciesBag is object)
        {
            DependenciesBag.Add(dependency);
        }
    }

    internal void AddDependencies(ICollection<TModuleSymbol>? dependencies)
    {
        if (!dependencies.IsNullOrEmpty() && DependenciesBag is object)
        {
            foreach (var candidate in dependencies)
            {
                DependenciesBag.Add(candidate);
            }
        }
    }

    internal void AddDependencies(IReadOnlyCollection<TModuleSymbol>? dependencies)
    {
        if (!dependencies.IsNullOrEmpty() && DependenciesBag is object)
        {
            foreach (var candidate in dependencies)
            {
                DependenciesBag.Add(candidate);
            }
        }
    }

    internal void AddDependencies(ImmutableHashSet<TModuleSymbol>? dependencies)
    {
        if (!dependencies.IsNullOrEmpty() && DependenciesBag is object)
        {
            foreach (var candidate in dependencies)
            {
                DependenciesBag.Add(candidate);
            }
        }
    }

    internal void AddDependencies(ImmutableArray<TModuleSymbol> dependencies)
    {
        if (!dependencies.IsDefaultOrEmpty && DependenciesBag is object)
        {
            foreach (var candidate in dependencies)
            {
                DependenciesBag.Add(candidate);
            }
        }
    }

    internal void AddDependencies(BindingDiagnosticBag<TModuleSymbol> dependencies, bool allowMismatchInDependencyAccumulation = false)
    {
        Debug.Assert(allowMismatchInDependencyAccumulation || !dependencies.AccumulatesDependencies || this.AccumulatesDependencies);
        AddDependencies(dependencies.DependenciesBag);
    }

    internal void AddDependencies(UseSiteInfo<TModuleSymbol> useSiteInfo)
    {
        if (DependenciesBag is object)
        {
            AddDependency(useSiteInfo.PrimaryDependency);
            AddDependencies(useSiteInfo.SecondaryDependencies);
        }
    }

    internal void AddDependencies(CompoundUseSiteInfo<TModuleSymbol> useSiteInfo)
    {
        Debug.Assert(!useSiteInfo.AccumulatesDependencies || this.AccumulatesDependencies);
        if (DependenciesBag is object)
        {
            AddDependencies(useSiteInfo.Dependencies);
        }
    }

    internal bool Add(SourceNode node, CompoundUseSiteInfo<TModuleSymbol> useSiteInfo)
    {
        return Add(node.GetLocation(), useSiteInfo);
    }

    internal bool AddDiagnostics(SourceNode node, CompoundUseSiteInfo<TModuleSymbol> useSiteInfo)
    {
        return AddDiagnostics(node.GetLocation(), useSiteInfo);
    }

    internal bool AddDiagnostics(Location location, CompoundUseSiteInfo<TModuleSymbol> useSiteInfo)
    {
        if (DiagnosticBag is DiagnosticBag diagnosticBag)
        {
            if (!useSiteInfo.Diagnostics.IsNullOrEmpty())
            {
                var haveError = false;
                foreach (var diagnosticInfo in useSiteInfo.Diagnostics)
                {
                    if (ReportUseSiteDiagnostic(diagnosticInfo, diagnosticBag, location))
                    {
                        haveError = true;
                    }
                }

                if (haveError)
                {
                    return true;
                }
            }
        }
        else if (useSiteInfo.AccumulatesDiagnostics && !useSiteInfo.Diagnostics.IsNullOrEmpty())
        {
            foreach (var info in useSiteInfo.Diagnostics)
            {
                if (info.Severity == DiagnosticSeverity.Error)
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal bool Add(Location location, CompoundUseSiteInfo<TModuleSymbol> useSiteInfo)
    {
        Debug.Assert(!useSiteInfo.AccumulatesDependencies || this.AccumulatesDependencies);
        if (AddDiagnostics(location, useSiteInfo))
        {
            return true;
        }

        AddDependencies(useSiteInfo);
        return false;
    }

    protected abstract bool ReportUseSiteDiagnostic(DiagnosticInfo diagnosticInfo, DiagnosticBag diagnosticBag, Location location);

    internal bool Add(UseSiteInfo<TModuleSymbol> useSiteInfo, SourceNode node)
    {
        return Add(useSiteInfo, node.GetLocation());
    }

    internal bool Add(UseSiteInfo<TModuleSymbol> info, Location location)
    {
        if (ReportUseSiteDiagnostic(info.DiagnosticInfo, location))
        {
            return true;
        }

        AddDependencies(info);
        return false;
    }

    internal bool ReportUseSiteDiagnostic(DiagnosticInfo? info, Location location)
    {
        if (info is null)
        {
            return false;
        }

        if (DiagnosticBag is not null)
        {
            return ReportUseSiteDiagnostic(info, DiagnosticBag, location);
        }

        return info.Severity == DiagnosticSeverity.Error;
    }
}

internal readonly struct ImmutableBindingDiagnostic<TModuleSymbol> where TModuleSymbol : class, IModuleSymbol
{
    private readonly ImmutableArray<Diagnostic> diagnostics;
    private readonly ImmutableArray<TModuleSymbol> dependencies;

    public ImmutableArray<Diagnostic> Diagnostics => diagnostics.NullToEmpty();
    public ImmutableArray<TModuleSymbol> Dependencies => dependencies.NullToEmpty();

    public static ImmutableBindingDiagnostic<TModuleSymbol> Empty => new(default, default);

    public ImmutableBindingDiagnostic(ImmutableArray<Diagnostic> diagnostics, ImmutableArray<TModuleSymbol> dependencies)
    {
        this.diagnostics = diagnostics.NullToEmpty();
        this.dependencies = dependencies.NullToEmpty();
    }

    public ImmutableBindingDiagnostic<TModuleSymbol> NullToEmpty() => new(Diagnostics, Dependencies);

    public static bool operator ==(ImmutableBindingDiagnostic<TModuleSymbol> first, ImmutableBindingDiagnostic<TModuleSymbol> second)
    {
        return first.Diagnostics == second.Diagnostics && first.Dependencies == second.Dependencies;
    }

    public static bool operator !=(ImmutableBindingDiagnostic<TModuleSymbol> first, ImmutableBindingDiagnostic<TModuleSymbol> second)
    {
        return !(first == second);
    }

    public override bool Equals(object? obj)
    {
        return (obj as ImmutableBindingDiagnostic<TModuleSymbol>?)?.Equals(this) ?? false;
    }

    public bool Equals(ImmutableBindingDiagnostic<TModuleSymbol> other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return Diagnostics.GetHashCode();
    }
}
