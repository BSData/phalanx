using System.Diagnostics;

namespace WarHub.ArmouryModel;

/// <summary>
/// An information that should be reported at a call site of a symbol.
/// From: https://sourceroslyn.io/#Microsoft.CodeAnalysis/Binding/UseSiteInfo.cs
/// </summary>
internal readonly struct UseSiteInfo<TModuleSymbol> where TModuleSymbol : class, IModuleSymbol
{
    /// <summary>
    /// Diagnostic info that should be reported at the use site of the symbol, or null if there is none.
    /// </summary>
    public readonly DiagnosticInfo? DiagnosticInfo;

    /// <summary>
    /// When not-null, this is primary dependency of the use-site, usually the assembly defining the used symbol.
    /// Never a core library. Usually it is not included into the <see cref="SecondaryDependencies"/>.
    /// Null if <see cref="DiagnosticInfo"/> is an error.
    /// </summary>
    public readonly TModuleSymbol? PrimaryDependency;

    /// <summary>
    /// The set of other assemblies the use site will depend upon, excluding a core library.
    /// Empty if <see cref="DiagnosticInfo"/> is an error.
    /// </summary>
    public readonly ImmutableHashSet<TModuleSymbol>? SecondaryDependencies;

    public UseSiteInfo(TModuleSymbol? primaryDependency) :
        this(diagnosticInfo: null, primaryDependency, secondaryDependencies: null)
    {
    }

    public UseSiteInfo(ImmutableHashSet<TModuleSymbol>? secondaryDependencies) :
        this(diagnosticInfo: null, primaryDependency: null, secondaryDependencies)
    {
    }

    public UseSiteInfo(DiagnosticInfo? diagnosticInfo) :
        this(diagnosticInfo, primaryDependency: null, secondaryDependencies: null)
    {
    }

    public UseSiteInfo(DiagnosticInfo? diagnosticInfo, TModuleSymbol? primaryDependency) :
        this(diagnosticInfo, primaryDependency, secondaryDependencies: null)
    {
    }

    public UseSiteInfo(DiagnosticInfo? diagnosticInfo, TModuleSymbol? primaryDependency, ImmutableHashSet<TModuleSymbol>? secondaryDependencies)
    {
        Debug.Assert(diagnosticInfo?.Severity != DiagnosticSeverity.Error || (primaryDependency is null && secondaryDependencies?.IsEmpty != false));
        // Debug.Assert(primaryDependency is null || primaryDependency != primaryDependency.CorLibrary);
        // Debug.Assert(secondaryDependencies?.IsEmpty != false || !secondaryDependencies.Any(dependency => dependency == dependency.CorLibrary));

        DiagnosticInfo = diagnosticInfo;
        PrimaryDependency = primaryDependency;
        SecondaryDependencies = secondaryDependencies ?? ImmutableHashSet<TModuleSymbol>.Empty;
    }

    public bool IsEmpty => DiagnosticInfo is null && PrimaryDependency is null && SecondaryDependencies?.IsEmpty != false;

    public UseSiteInfo<TModuleSymbol> AdjustDiagnosticInfo(DiagnosticInfo? diagnosticInfo)
    {
        if ((object?)DiagnosticInfo != diagnosticInfo)
        {
            if (diagnosticInfo?.Severity == DiagnosticSeverity.Error)
            {
                return new UseSiteInfo<TModuleSymbol>(diagnosticInfo);
            }
            else
            {
                Debug.Assert(DiagnosticInfo?.Severity != DiagnosticSeverity.Error);
            }

            return new UseSiteInfo<TModuleSymbol>(diagnosticInfo, PrimaryDependency, SecondaryDependencies);
        }

        return this;
    }

    public void MergeDependencies(ref TModuleSymbol? primaryDependency, ref ImmutableHashSet<TModuleSymbol>? secondaryDependencies)
    {
        secondaryDependencies = (secondaryDependencies ?? ImmutableHashSet<TModuleSymbol>.Empty).Union(SecondaryDependencies ?? ImmutableHashSet<TModuleSymbol>.Empty);
        primaryDependency ??= PrimaryDependency;

        if (!object.Equals(primaryDependency, PrimaryDependency) && PrimaryDependency is object)
        {
            secondaryDependencies = secondaryDependencies.Add(PrimaryDependency);
        }
    }
}

/// <summary>
/// A helper used to combine information from multiple <see cref="UseSiteInfo{TModuleSymbol}"/>s related to the same
/// use site.
/// From: https://sourceroslyn.io/#Microsoft.CodeAnalysis/Binding/UseSiteInfo.cs
/// </summary>
internal struct CompoundUseSiteInfo<TModuleSymbol> where TModuleSymbol : class, IModuleSymbol
{
    private bool _hasErrors;
    private readonly DiscardLevel _discardLevel;
    private HashSet<DiagnosticInfo>? _diagnostics;
    private HashSet<TModuleSymbol>? _dependencies;
    private readonly TModuleSymbol? _assemblyBeingBuilt;

    public static CompoundUseSiteInfo<TModuleSymbol> Discarded => new(DiscardLevel.DiagnosticsAndDependencies);
    public static CompoundUseSiteInfo<TModuleSymbol> DiscardedDependencies => new(DiscardLevel.Dependencies);

    private enum DiscardLevel : byte
    {
        None,
        Dependencies,
        DiagnosticsAndDependencies
    }

    public CompoundUseSiteInfo(TModuleSymbol assemblyBeingBuilt)
    {
        Debug.Assert(assemblyBeingBuilt is object);
        // Debug.Assert(assemblyBeingBuilt is SourceSymbol);

        this = default;
        _assemblyBeingBuilt = assemblyBeingBuilt;
    }

    public CompoundUseSiteInfo(BindingDiagnosticBag<TModuleSymbol>? futureDestination, TModuleSymbol assemblyBeingBuilt)
    {
        Debug.Assert(assemblyBeingBuilt is object);
        // Debug.Assert(assemblyBeingBuilt is SourceSymbol);

        this = default;

        if (futureDestination is null)
        {
            _discardLevel = DiscardLevel.DiagnosticsAndDependencies;
        }
        else if (!futureDestination.AccumulatesDependencies)
        {
            _discardLevel = DiscardLevel.Dependencies;
        }
        else
        {
            _discardLevel = DiscardLevel.None;
            _assemblyBeingBuilt = assemblyBeingBuilt;
        }
    }

    public CompoundUseSiteInfo(CompoundUseSiteInfo<TModuleSymbol> template)
    {
        this = default;
        _discardLevel = template._discardLevel;
        _assemblyBeingBuilt = template._assemblyBeingBuilt;
    }

    private CompoundUseSiteInfo(DiscardLevel discardLevel)
    {
        Debug.Assert(discardLevel != DiscardLevel.None);
        this = default;
        _discardLevel = discardLevel;
    }

    public TModuleSymbol? AssemblyBeingBuilt => _assemblyBeingBuilt;

    private DiscardLevel DiscardLevelWithValidation
    {
        get
        {
#if DEBUG
            switch (_discardLevel)
            {
                case DiscardLevel.DiagnosticsAndDependencies:
                    Debug.Assert(_diagnostics is null);
                    Debug.Assert(_dependencies is null);
                    Debug.Assert(!_hasErrors);
                    break;

                case DiscardLevel.Dependencies:
                    Debug.Assert(_dependencies is null);
                    break;
            }

            AssertInternalConsistency();
#endif
            return _discardLevel;
        }
    }

    public bool AccumulatesDiagnostics => DiscardLevelWithValidation != DiscardLevel.DiagnosticsAndDependencies;

    public IReadOnlyCollection<DiagnosticInfo>? Diagnostics
    {
        get
        {
            Debug.Assert(AccumulatesDiagnostics);
            AssertInternalConsistency();
            return _diagnostics;
        }
    }

    [Conditional("DEBUG")]
    private readonly void AssertInternalConsistency()
    {
        Debug.Assert(_discardLevel switch { DiscardLevel.None => true, DiscardLevel.Dependencies => true, DiscardLevel.DiagnosticsAndDependencies => true, _ => false });
        Debug.Assert(_hasErrors == (_diagnostics?.Any(d => d.Severity == DiagnosticSeverity.Error) ?? false));
        Debug.Assert(!_hasErrors || (_dependencies is null));
    }

    public bool AccumulatesDependencies => DiscardLevelWithValidation == DiscardLevel.None;

    public IReadOnlyCollection<TModuleSymbol>? Dependencies
    {
        get
        {
            Debug.Assert(AccumulatesDependencies);
            AssertInternalConsistency();
            return _dependencies;
        }
    }

    public bool HasErrors
    {
        get
        {
            Debug.Assert(AccumulatesDiagnostics);
            AssertInternalConsistency();
            return _hasErrors;
        }
    }

    public void AddDiagnostics(UseSiteInfo<TModuleSymbol> info)
    {
        if (!AccumulatesDiagnostics)
        {
            return;
        }

        if (HashSetExtensions.InitializeAndAdd(ref _diagnostics, info.DiagnosticInfo) &&
            info.DiagnosticInfo?.Severity == DiagnosticSeverity.Error)
        {
            RecordPresenceOfAnError();
        }
    }

    private void RecordPresenceOfAnError()
    {
        if (!_hasErrors)
        {
            _hasErrors = true;
            _dependencies = null;
        }
    }

    public void AddDiagnostics(ICollection<DiagnosticInfo>? diagnostics)
    {
        if (!AccumulatesDiagnostics)
        {
            return;
        }

        if (!diagnostics.IsNullOrEmpty())
        {
            _diagnostics ??= new HashSet<DiagnosticInfo>();

            foreach (var diagnosticInfo in diagnostics)
            {
                if (_diagnostics.Add(diagnosticInfo) && diagnosticInfo?.Severity == DiagnosticSeverity.Error)
                {
                    RecordPresenceOfAnError();
                }
            }
        }
    }

    public void AddDiagnostics(IReadOnlyCollection<DiagnosticInfo>? diagnostics)
    {
        if (!AccumulatesDiagnostics)
        {
            return;
        }

        if (!diagnostics.IsNullOrEmpty())
        {
            _diagnostics ??= new HashSet<DiagnosticInfo>();

            foreach (var diagnosticInfo in diagnostics)
            {
                if (_diagnostics.Add(diagnosticInfo) && diagnosticInfo?.Severity == DiagnosticSeverity.Error)
                {
                    RecordPresenceOfAnError();
                }
            }
        }
    }

    public void AddDiagnostics(ImmutableArray<DiagnosticInfo> diagnostics)
    {
        if (!AccumulatesDiagnostics)
        {
            return;
        }

        if (!diagnostics.IsDefaultOrEmpty)
        {
            _diagnostics ??= new HashSet<DiagnosticInfo>();

            foreach (var diagnosticInfo in diagnostics)
            {
                if (_diagnostics.Add(diagnosticInfo) && diagnosticInfo?.Severity == DiagnosticSeverity.Error)
                {
                    RecordPresenceOfAnError();
                }
            }
        }
    }

    public void AddDependencies(UseSiteInfo<TModuleSymbol> info)
    {
        if (!_hasErrors && AccumulatesDependencies)
        {
            if (info.PrimaryDependency != _assemblyBeingBuilt)
            {
                HashSetExtensions.InitializeAndAdd(ref _dependencies, info.PrimaryDependency);
            }

            if (info.SecondaryDependencies?.IsEmpty == false && (_assemblyBeingBuilt is null || info.SecondaryDependencies.AsSingleton() != _assemblyBeingBuilt))
            {
                (_dependencies ??= new HashSet<TModuleSymbol>()).AddAll(info.SecondaryDependencies);
            }
        }
    }

    public void AddDependencies(CompoundUseSiteInfo<TModuleSymbol> info)
    {
        Debug.Assert(!info.AccumulatesDependencies || this.AccumulatesDependencies);
        if (!_hasErrors && AccumulatesDependencies)
        {
            AddDependencies(info.Dependencies);
        }
    }

    public void AddDependencies(ICollection<TModuleSymbol>? dependencies)
    {
        if (!_hasErrors && AccumulatesDependencies && !dependencies.IsNullOrEmpty() &&
            (_assemblyBeingBuilt is null || dependencies.AsSingleton() != _assemblyBeingBuilt))
        {
            (_dependencies ??= new HashSet<TModuleSymbol>()).AddAll(dependencies);
        }
    }

    public void AddDependencies(IReadOnlyCollection<TModuleSymbol>? dependencies)
    {
        if (!_hasErrors && AccumulatesDependencies && !dependencies.IsNullOrEmpty() &&
            (_assemblyBeingBuilt is null || dependencies.AsSingleton() != _assemblyBeingBuilt))
        {
            (_dependencies ??= new HashSet<TModuleSymbol>()).AddAll(dependencies);
        }
    }

    public void AddDependencies(ImmutableArray<TModuleSymbol> dependencies)
    {
        if (!_hasErrors && AccumulatesDependencies && !dependencies.IsDefaultOrEmpty &&
            (_assemblyBeingBuilt is null || dependencies.Length != 1 || dependencies[0] != _assemblyBeingBuilt))
        {
            (_dependencies ??= new HashSet<TModuleSymbol>()).AddAll(dependencies);
        }
    }

    public void MergeAndClear(ref CompoundUseSiteInfo<TModuleSymbol> other)
    {
        Debug.Assert(other.AccumulatesDiagnostics);
        Debug.Assert(other._discardLevel != DiscardLevel.None || _discardLevel == DiscardLevel.None);

        if (!AccumulatesDiagnostics)
        {
            Debug.Assert(!AccumulatesDependencies);
            other._diagnostics = null;
            other._dependencies = null;
            other._hasErrors = false;
            return;
        }

        mergeAndClear(ref _diagnostics, ref other._diagnostics);

        if (other._hasErrors)
        {
            RecordPresenceOfAnError();
            other._hasErrors = false;
        }

        if (!_hasErrors && AccumulatesDependencies)
        {
            mergeAndClear(ref _dependencies, ref other._dependencies);
        }
        else
        {
            other._dependencies = null;
        }

        static void mergeAndClear<T>(ref HashSet<T>? self, ref HashSet<T>? other)
        {
            if (self is null)
            {
                self = other;
            }
            else if (other is not null)
            {
                self.AddAll(other);
            }

            other = null;
        }
    }

    public void Add(UseSiteInfo<TModuleSymbol> other)
    {
        if (!AccumulatesDiagnostics)
        {
            Debug.Assert(!AccumulatesDependencies);
            return;
        }

        AddDiagnostics(other);
        AddDependencies(other);
    }
}

/// <summary>
/// A helper used to efficiently cache <see cref="UseSiteInfo{TModuleSymbol}"/> in the symbol.
/// From: https://sourceroslyn.io/#Microsoft.CodeAnalysis/Binding/UseSiteInfo.cs
/// </summary>
internal struct CachedUseSiteInfo<TModuleSymbol> where TModuleSymbol : class, IModuleSymbol
{
    /// <summary>
    /// Either 
    /// - null (meaning no diagnostic info and dependencies), or
    /// - a <see cref="DiagnosticInfo"/>, or
    /// - dependencies as a <see cref="ImmutableHashSet{TModuleSymbol}"/>, or
    /// - a <see cref="Boxed"/> tuple of a <see cref="DiagnosticInfo"/> and a <see cref="ImmutableHashSet{TModuleSymbol}"/>. 
    /// </summary>
    private object? _info;

    private static readonly object Sentinel = new(); // Indicates unknown state.

    public static readonly CachedUseSiteInfo<TModuleSymbol> Uninitialized = new(Sentinel); // Indicates unknown state.

    private CachedUseSiteInfo(object info)
    {
        _info = info;
    }

    public bool IsInitialized => (object?)_info != Sentinel;

    public void Initialize(DiagnosticInfo? diagnosticInfo)
    {
        Debug.Assert(diagnosticInfo is null || diagnosticInfo.Severity == DiagnosticSeverity.Error);
        Initialize(diagnosticInfo, dependencies: ImmutableHashSet<TModuleSymbol>.Empty);
    }

    public void Initialize(TModuleSymbol? primaryDependency, UseSiteInfo<TModuleSymbol> useSiteInfo)
    {
        Initialize(useSiteInfo.DiagnosticInfo, GetDependenciesToCache(primaryDependency, useSiteInfo));
    }

    private static ImmutableHashSet<TModuleSymbol> GetDependenciesToCache(TModuleSymbol? primaryDependency, UseSiteInfo<TModuleSymbol> useSiteInfo)
    {
        var secondaryDependencies = useSiteInfo.SecondaryDependencies ?? ImmutableHashSet<TModuleSymbol>.Empty;
        Debug.Assert(primaryDependency is object || (useSiteInfo.PrimaryDependency is null && secondaryDependencies.IsEmpty));
        Debug.Assert(primaryDependency == useSiteInfo.PrimaryDependency || useSiteInfo.DiagnosticInfo?.Severity == DiagnosticSeverity.Error);
        if (useSiteInfo.PrimaryDependency is object)
        {
            return secondaryDependencies.Remove(useSiteInfo.PrimaryDependency);
        }

        return secondaryDependencies;
    }

    public UseSiteInfo<TModuleSymbol> ToUseSiteInfo(TModuleSymbol primaryDependency)
    {
        Expand(_info, out var diagnosticInfo, out var dependencies);

        if (diagnosticInfo?.Severity == DiagnosticSeverity.Error)
        {
            return new UseSiteInfo<TModuleSymbol>(diagnosticInfo);
        }

        return new UseSiteInfo<TModuleSymbol>(diagnosticInfo, primaryDependency, dependencies);
    }

    private void Initialize(DiagnosticInfo? diagnosticInfo, ImmutableHashSet<TModuleSymbol> dependencies)
    {
        _info = Compact(diagnosticInfo, dependencies);
    }

    private static object? Compact(DiagnosticInfo? diagnosticInfo, ImmutableHashSet<TModuleSymbol> dependencies)
    {
        object? info;

        if (dependencies.IsEmpty)
        {
            info = diagnosticInfo;
        }
        else if (diagnosticInfo is null)
        {
            info = dependencies;
        }
        else
        {
            info = new Boxed(diagnosticInfo, dependencies);
        }

        return info;
    }

    public void InterlockedCompareExchange(TModuleSymbol? primaryDependency, UseSiteInfo<TModuleSymbol> value)
    {
        if ((object?)_info == Sentinel)
        {
            var info = Compact(value.DiagnosticInfo, GetDependenciesToCache(primaryDependency, value));
            Interlocked.CompareExchange(ref _info, info, Sentinel);
        }
    }

    public UseSiteInfo<TModuleSymbol> InterlockedInitialize(TModuleSymbol? primaryDependency, UseSiteInfo<TModuleSymbol> value)
    {
        var info = Compact(value.DiagnosticInfo, GetDependenciesToCache(primaryDependency, value));
        Debug.Assert(info is not null);

        info = Interlocked.CompareExchange(ref _info, info, null);
        if (info == null)
        {
            return value;
        }

        Expand(info, out var diagnosticInfo, out var dependencies);
        return new UseSiteInfo<TModuleSymbol>(diagnosticInfo, value.PrimaryDependency, dependencies);
    }

    private static void Expand(object? info, out DiagnosticInfo? diagnosticInfo, out ImmutableHashSet<TModuleSymbol>? dependencies)
    {
        switch (info)
        {
            case null:
                diagnosticInfo = null;
                dependencies = null;
                break;

            case DiagnosticInfo d:
                diagnosticInfo = d;
                dependencies = null;
                break;

            case ImmutableHashSet<TModuleSymbol> a:
                diagnosticInfo = null;
                dependencies = a;
                break;

            default:
                var boxed = (Boxed)info;
                diagnosticInfo = boxed.DiagnosticInfo;
                dependencies = boxed.Dependencies;
                break;
        }
    }

    private class Boxed
    {
        /// <summary>
        /// Diagnostic info that should be reported at the use site of the symbol, or null if there is none.
        /// </summary>
        public readonly DiagnosticInfo DiagnosticInfo;

        /// <summary>
        /// The set of assemblies the use site will depend upon, excluding assembly for core library.
        /// Empty or null if <see cref="Boxed.DiagnosticInfo"/> is an error.
        /// </summary>
        public readonly ImmutableHashSet<TModuleSymbol> Dependencies;

        public Boxed(DiagnosticInfo diagnosticInfo, ImmutableHashSet<TModuleSymbol> dependencies)
        {
            Debug.Assert(!dependencies.IsEmpty);
            DiagnosticInfo = diagnosticInfo;
            Dependencies = dependencies;
        }
    }
}
