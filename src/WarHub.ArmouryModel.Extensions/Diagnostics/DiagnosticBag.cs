using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel;

/// <summary>
/// Represents a mutable bag of diagnostics. You can add diagnostics to the bag,
/// and also get all the diagnostics out of the bag (the bag implements
/// IEnumerable&lt;Diagnostics&gt;. Once added, diagnostics cannot be removed, and no ordering
/// is guaranteed.
/// 
/// It is ok to Add diagnostics to the same bag concurrently on multiple threads.
/// It is NOT ok to Add concurrently with Clear or Free operations.
/// </summary>
/// <remarks>The bag is optimized to be efficient when containing zero errors.</remarks>
[DebuggerDisplay("{GetDebuggerDisplay(), nq}")]
[DebuggerTypeProxy(typeof(DebuggerProxy))]
internal class DiagnosticBag
{
    // The lazyBag field is populated lazily -- the first time an error is added.
    private ConcurrentQueue<Diagnostic>? lazyBag;

    /// <summary>
    /// Return true if the bag is completely empty - not even containing void diagnostics.
    /// </summary>
    /// <remarks>
    /// This exists for short-circuiting purposes. Use <see cref="Enumerable.Any{T}(IEnumerable{T})"/>
    /// to get a resolved Tuple(Of NamedTypeSymbol, ImmutableArray(Of Diagnostic)) (i.e. empty after eliminating void diagnostics).
    /// </remarks>
    public bool IsEmptyWithoutResolution
    {
        get
        {
            // It should be safe to access this here, since we normally have a collect phase and
            // then a report phase, and we shouldn't be called during the "report" phase. We
            // also never remove diagnostics, so the worst that happens is that we don't return
            // an element that is added a split second after this is called.
            var bag = lazyBag;
            return bag == null || bag.IsEmpty;
        }
    }

    /// <summary>
    /// Returns true if the bag has any diagnostics with DefaultSeverity=Error. Does not consider warnings or informationals
    /// or warnings promoted to error via /warnaserror.
    /// </summary>
    /// <remarks>
    /// Resolves any lazy diagnostics in the bag.
    /// 
    /// Generally, this should only be called by the creator (modulo pooling) of the bag (i.e. don't use bags to communicate -
    /// if you need more info, pass more info).
    /// </remarks>
    public bool HasAnyErrors()
    {
        if (IsEmptyWithoutResolution)
        {
            return false;
        }

        foreach (var diagnostic in Bag)
        {
            if (diagnostic.DefaultSeverity == DiagnosticSeverity.Error)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if the bag has any non-lazy diagnostics with DefaultSeverity=Error. Does not consider warnings or informationals
    /// or warnings promoted to error via /warnaserror.
    /// </summary>
    /// <remarks>
    /// Does not resolve any lazy diagnostics in the bag.
    /// 
    /// Generally, this should only be called by the creator (modulo pooling) of the bag (i.e. don't use bags to communicate -
    /// if you need more info, pass more info).
    /// </remarks>
    internal bool HasAnyResolvedErrors()
    {
        if (IsEmptyWithoutResolution)
        {
            return false;
        }

        foreach (var diagnostic in Bag)
        {
            if (diagnostic.DefaultSeverity == DiagnosticSeverity.Error)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Add a diagnostic to the bag.
    /// </summary>
    public void Add(Diagnostic diag)
    {
        var bag = Bag;
        bag.Enqueue(diag);
    }

    /// <summary>
    /// Add multiple diagnostics to the bag.
    /// </summary>
    public void AddRange<T>(ImmutableArray<T> diagnostics) where T : Diagnostic
    {
        if (!diagnostics.IsDefaultOrEmpty)
        {
            var bag = Bag;
            for (var i = 0; i < diagnostics.Length; i++)
            {
                bag.Enqueue(diagnostics[i]);
            }
        }
    }

    /// <summary>
    /// Add multiple diagnostics to the bag.
    /// </summary>
    public void AddRange(IEnumerable<Diagnostic> diagnostics)
    {
        foreach (var diagnostic in diagnostics)
        {
            Bag.Enqueue(diagnostic);
        }
    }

    /// <summary>
    /// Add another DiagnosticBag to the bag.
    /// </summary>
    public void AddRange(DiagnosticBag bag)
    {
        if (!bag.IsEmptyWithoutResolution)
        {
            AddRange(bag.Bag);
        }
    }

    /// <summary>
    /// Add another DiagnosticBag to the bag and free the argument.
    /// </summary>
    public void AddRangeAndFree(DiagnosticBag bag)
    {
        AddRange(bag);
        bag.Free();
    }

    /// <summary>
    /// Seal the bag so no further errors can be added, while clearing it and returning the old set of errors.
    /// Return the bag to the pool.
    /// </summary>
    public ImmutableArray<TDiagnostic> ToReadOnlyAndFree<TDiagnostic>() where TDiagnostic : Diagnostic
    {
        var oldBag = lazyBag;
        Free();

        return ToReadOnlyCore<TDiagnostic>(oldBag);
    }

    public ImmutableArray<Diagnostic> ToReadOnlyAndFree()
    {
        return ToReadOnlyAndFree<Diagnostic>();
    }

    public ImmutableArray<TDiagnostic> ToReadOnly<TDiagnostic>() where TDiagnostic : Diagnostic
    {
        var oldBag = lazyBag;
        return ToReadOnlyCore<TDiagnostic>(oldBag);
    }

    public ImmutableArray<Diagnostic> ToReadOnly()
    {
        return ToReadOnly<Diagnostic>();
    }

    private static ImmutableArray<TDiagnostic> ToReadOnlyCore<TDiagnostic>(ConcurrentQueue<Diagnostic>? oldBag) where TDiagnostic : Diagnostic
    {
        if (oldBag == null)
        {
            return ImmutableArray<TDiagnostic>.Empty;
        }

        var builder = ImmutableArray.CreateBuilder<TDiagnostic>();

        foreach (TDiagnostic diagnostic in oldBag) // Cast should be safe since all diagnostics should be from same language.
        {
            builder.Add(diagnostic);
        }

        return builder.ToImmutable();
    }


    /// <remarks>
    /// Generally, this should only be called by the creator (modulo pooling) of the bag (i.e. don't use bags to communicate -
    /// if you need more info, pass more info).
    /// </remarks>
    public IEnumerable<Diagnostic> AsEnumerable() => Bag;

    internal IEnumerable<Diagnostic> AsEnumerableWithoutResolution()
    {
        // PERF: don't make a defensive copy - callers are internal and won't modify the bag.
        return lazyBag ?? Enumerable.Empty<Diagnostic>();
    }

    internal int Count => lazyBag?.Count ?? 0;

    public override string ToString()
    {
        if (IsEmptyWithoutResolution)
        {
            // TODO(cyrusn): do we need to localize this?
            return "<no errors>";
        }
        else
        {
            var builder = new StringBuilder();
            foreach (var diag in Bag) // NOTE: don't force resolution
            {
                builder.AppendLine(diag.ToString());
            }
            return builder.ToString();
        }
    }

    /// <summary>
    /// Get the underlying concurrent storage, creating it on demand if needed.
    /// NOTE: Concurrent Adding to the bag is supported, but concurrent Clearing is not.
    ///       If one thread adds to the bug while another clears it, the scenario is 
    ///       broken and we cannot do anything about it here.
    /// </summary>
    private ConcurrentQueue<Diagnostic> Bag
    {
        get
        {
            var bag = lazyBag;
            if (bag != null)
            {
                return bag;
            }

            var newBag = new ConcurrentQueue<Diagnostic>();
            return Interlocked.CompareExchange(ref lazyBag, newBag, null) ?? newBag;
        }
    }

    // clears the bag.
    /// NOTE: Concurrent Adding to the bag is supported, but concurrent Clearing is not.
    ///       If one thread adds to the bug while another clears it, the scenario is 
    ///       broken and we cannot do anything about it here.
    internal void Clear()
    {
        var bag = lazyBag;
        if (bag != null)
        {
            lazyBag = null;
        }
    }

    #region "Poolable"

    internal static DiagnosticBag GetInstance()
    {
        // DiagnosticBag bag = s_poolInstance.Allocate();
        // return bag;
        return new DiagnosticBag();
    }

    internal void Free()
    {
        Clear();
        // s_poolInstance.Free(this);
    }

    // private static readonly ObjectPool<DiagnosticBag> s_poolInstance = CreatePool(128);
    // private static ObjectPool<DiagnosticBag> CreatePool(int size)
    // {
    //     return new ObjectPool<DiagnosticBag>(() => new DiagnosticBag(), size);
    // }

    #endregion

    #region Debugger View

    internal sealed class DebuggerProxy
    {
        private readonly DiagnosticBag bag;

        public DebuggerProxy(DiagnosticBag bag)
        {
            this.bag = bag;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public object[] Diagnostics
        {
            get
            {
                var lazyBag = bag.lazyBag;
                if (lazyBag != null)
                {
                    return lazyBag.ToArray();
                }
                else
                {
                    return Array.Empty<object>();
                }
            }
        }
    }

    private string GetDebuggerDisplay()
    {
        return "Count = " + (lazyBag?.Count ?? 0);
    }
    #endregion
}
