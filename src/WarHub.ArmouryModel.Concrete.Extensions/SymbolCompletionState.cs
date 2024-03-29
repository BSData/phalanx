using System.Diagnostics;
using System.Text;

namespace WarHub.ArmouryModel.Concrete;

/// <summary>
/// Tracks completed parts of a symbol.
/// From: https://sourceroslyn.io/#Microsoft.CodeAnalysis.CSharp/Symbols/SymbolCompletionState.cs
/// </summary>
internal struct SymbolCompletionState
{
    /// <summary>
    /// This field keeps track of the <see cref="CompletionPart"/>s for which we already retrieved
    /// diagnostics. We shouldn't return from ForceComplete (i.e. indicate that diagnostics are
    /// available) until this is equal to <see cref="CompletionPart.All"/>, except that when completing
    /// with a given position, we might not complete <see cref="CompletionPart"/>.Member*.
    /// 
    /// Since completeParts is used as a flag indicating completion of other assignments 
    /// it must be volatile to ensure the read is not reordered/optimized to happen 
    /// before the writes.
    /// </summary>
    private volatile int completeParts;

    internal int IncompleteParts
    {
        get
        {
            return ~completeParts & (int)CompletionPart.All;
        }
    }

    internal bool HasComplete(CompletionPart part)
    {
        // completeParts is used as a flag indicating completion of other assignments 
        // Volatile.Read is used to ensure the read is not reordered/optimized to happen 
        // before the writes.
        return (completeParts & (int)part) == (int)part;
    }

    internal bool NotePartComplete(CompletionPart part)
    {
        // passing volatile completeParts byref is ok here.
        // ThreadSafeFlagOperations.Set performs interlocked assignments
#pragma warning disable 0420
        return ThreadSafeFlagOperations.Set(ref completeParts, (int)part);
#pragma warning restore 0420
    }

    /// <summary>
    /// Produce the next (i.e. lowest) CompletionPart (bit) that is not set.
    /// </summary>
    internal CompletionPart NextIncompletePart
    {
        get
        {
            // NOTE: It's very important to store this value in a local.
            // If we were to inline the field access, the value of the
            // field could change between the two accesses and the formula
            // might not produce a result with a single 1-bit.
            var incomplete = IncompleteParts;
            var next = incomplete & ~(incomplete - 1);
            Debug.Assert(HasAtMostOneBitSet(next), "ForceComplete won't handle the result correctly if more than one bit is set.");
            return (CompletionPart)next;
        }
    }

    /// <remarks>
    /// Since this formula is rather opaque, a demonstration of its correctness is
    /// provided in Roslyn.Compilers.CSharp.UnitTests.CompletionTests.TestHasAtMostOneBitSet.
    /// </remarks>
    internal static bool HasAtMostOneBitSet(int bits)
    {
        return (bits & (bits - 1)) == 0;
    }

    internal void SpinWaitComplete(CompletionPart part, CancellationToken cancellationToken)
    {
        if (HasComplete(part))
        {
            return;
        }

        // Don't return until we've seen all of the requested CompletionParts. This ensures all
        // diagnostics have been reported (not necessarily on this thread).
        var spinWait = new SpinWait();
        while (!HasComplete(part))
        {
            cancellationToken.ThrowIfCancellationRequested();
            spinWait.SpinOnce();
        }
    }

    public override string ToString()
    {
        var result = new StringBuilder();
        result.Append("CompletionParts(");
        var any = false;
        for (var i = 0; ; i++)
        {
            var bit = 1 << i;
            if ((bit & (int)CompletionPart.All) == 0) break;
            if ((bit & completeParts) != 0)
            {
                if (any) result.Append(", ");
                result.Append(i);
                any = true;
            }
        }
        result.Append(')');
        return result.ToString();
    }
}
