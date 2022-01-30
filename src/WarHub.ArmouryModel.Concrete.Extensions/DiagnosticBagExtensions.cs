using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal static class DiagnosticBagExtensions
{
    /// <summary>
    /// Add a diagnostic to the bag.
    /// </summary>
    internal static WhamDiagnosticInfo Add(this DiagnosticBag diagnostics, ErrorCode code, SourceNode node)
    {
        return diagnostics.Add(code, node.GetLocation());
    }

    /// <summary>
    /// Add a diagnostic to the bag.
    /// </summary>
    internal static WhamDiagnosticInfo Add(this DiagnosticBag diagnostics, ErrorCode code, Location location)
    {
        var info = new WhamDiagnosticInfo(code);
        var diag = new WhamDiagnostic(info, location);
        diagnostics.Add(diag);
        return info;
    }

    /// <summary>
    /// Add a diagnostic to the bag.
    /// </summary>
    internal static WhamDiagnosticInfo Add(this DiagnosticBag diagnostics, ErrorCode code, Location location, params object[] args)
    {
        var info = new WhamDiagnosticInfo(code, args);
        var diag = new WhamDiagnostic(info, location);
        diagnostics.Add(diag);
        return info;
    }

    internal static WhamDiagnosticInfo Add(this DiagnosticBag diagnostics, ErrorCode code, Location location, ImmutableArray<Symbol> symbols, params object[] args)
    {
        var info = new WhamDiagnosticInfo(code, args, symbols, ImmutableArray<Location>.Empty);
        var diag = new WhamDiagnostic(info, location);
        diagnostics.Add(diag);
        return info;
    }
}
