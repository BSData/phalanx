using System.Globalization;
using WarHub.ArmouryModel.Source.Text;

namespace WarHub.ArmouryModel.Source;

/// <summary>
/// Formats <see cref="Diagnostic"/> messages.
/// </summary>
public class DiagnosticFormatter
{
    /// <summary>
    /// Formats the <see cref="Diagnostic"/> message using the optional <see cref="IFormatProvider"/>.
    /// </summary>
    /// <param name="diagnostic">The diagnostic.</param>
    /// <param name="formatter">The formatter; or null to use the default formatter.</param>
    /// <returns>The formatted message.</returns>
    public virtual string Format(Diagnostic diagnostic, IFormatProvider? formatter = null)
    {
        if (diagnostic == null)
        {
            throw new ArgumentNullException(nameof(diagnostic));
        }

        var culture = formatter as CultureInfo;

        switch (diagnostic.Location.Kind)
        {
            case LocationKind.SourceFile:
                var span = diagnostic.Location.GetLineSpan();
                if (!span.IsValid)
                {
                    goto default;
                }

                string? path, basePath;
                path = span.Path;
                basePath = null;

                return string.Format(formatter, "{0}{1}: {2}: {3}",
                                     FormatSourcePath(path, basePath, formatter),
                                     FormatSourceSpan(span.Span, formatter),
                                     GetMessagePrefix(diagnostic),
                                     diagnostic.GetMessage(culture));

            default:
                return string.Format(formatter, "{0}: {1}",
                                     GetMessagePrefix(diagnostic),
                                     diagnostic.GetMessage(culture));
        }
    }

    internal virtual string FormatSourcePath(string path, string? basePath, IFormatProvider? formatter)
    {
        // ignore base path
        return path;
    }

    internal virtual string FormatSourceSpan(LinePositionSpan span, IFormatProvider? formatter)
    {
        return string.Format("({0},{1})", span.Start.Line + 1, span.Start.Character + 1);
    }

    internal string GetMessagePrefix(Diagnostic diagnostic)
    {
        var prefix = diagnostic.Severity switch
        {
            DiagnosticSeverity.Hidden => "hidden",
            DiagnosticSeverity.Info => "info",
            DiagnosticSeverity.Warning => "warning",
            DiagnosticSeverity.Error => "error",
            _ => throw new InvalidOperationException($"Unexpected value: {diagnostic.Severity}"),
        };
        return string.Format("{0} {1}", prefix, diagnostic.Id);
    }

    internal static readonly DiagnosticFormatter Instance = new();
}
