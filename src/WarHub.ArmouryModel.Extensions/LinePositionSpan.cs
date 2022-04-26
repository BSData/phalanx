using System.Globalization;

namespace WarHub.ArmouryModel;

/// <summary>
/// Immutable span represented by a pair of line number and index within the line.
/// </summary>
public readonly struct LinePositionSpan : IEquatable<LinePositionSpan>
{

    /// <summary>
    /// Creates <see cref="LinePositionSpan"/>.
    /// </summary>
    /// <param name="start">Start position.</param>
    /// <param name="end">End position.</param>
    /// <exception cref="ArgumentException"><paramref name="end"/> precedes <paramref name="start"/>.</exception>
    public LinePositionSpan(LinePosition start, LinePosition end)
    {
        if (end < start)
        {
            throw new ArgumentException("End must not be less than start.", nameof(end));
        }

        Start = start;
        End = end;
    }

    /// <summary>
    /// Gets the start position of the span.
    /// </summary>
    public LinePosition Start { get; }

    /// <summary>
    /// Gets the end position of the span.
    /// </summary>
    public LinePosition End { get; }

    public override bool Equals(object? obj)
    {
        return obj is LinePositionSpan span && Equals(span);
    }

    public bool Equals(LinePositionSpan other)
    {
        return Start.Equals(other.Start)
            && End.Equals(other.End);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start.GetHashCode(), End.GetHashCode());
    }

    public static bool operator ==(LinePositionSpan left, LinePositionSpan right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(LinePositionSpan left, LinePositionSpan right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Provides a string representation for <see cref="LinePositionSpan"/>.
    /// </summary>
    /// <example>(0,0)-(5,6)</example>
    public override string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "({0})-({1})", Start, End);
    }
}
