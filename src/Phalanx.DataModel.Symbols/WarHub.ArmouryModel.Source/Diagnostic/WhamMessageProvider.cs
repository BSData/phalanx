using System.Globalization;
using Phalanx.DataModel.Symbols;
using Phalanx.DataModel.Symbols.Implementation;

namespace WarHub.ArmouryModel.Source;

internal sealed class WhamMessageProvider : CommonMessageProvider
{
    public static readonly WhamMessageProvider Instance = new();

    private WhamMessageProvider()
    {
    }

    public override DiagnosticSeverity GetSeverity(int code)
    {
        return ErrorFacts.GetSeverity((ErrorCode)code);
    }

    public override string LoadMessage(int code, CultureInfo? language)
    {
        return ErrorFacts.GetMessage((ErrorCode)code, language);
    }

    public override LocalizableString GetMessageFormat(int code)
    {
        return ErrorFacts.GetMessageFormat((ErrorCode)code);
    }

    public override LocalizableString GetDescription(int code)
    {
        return ErrorFacts.GetDescription((ErrorCode)code);
    }

    public override LocalizableString GetTitle(int code)
    {
        return ErrorFacts.GetTitle((ErrorCode)code);
    }

    public override string? GetHelpLink(int code)
    {
        return ErrorFacts.GetHelpLink((ErrorCode)code);
    }

    public override string GetCategory(int code)
    {
        return ErrorFacts.GetCategory((ErrorCode)code);
    }

    public override string CodePrefix => "WHAM";


    // Given a message identifier (e.g., CS0219), severity, warning as error and a culture, 
    // get the entire prefix (e.g., "error CS0219:" for C#) used on error messages.
    public override string GetMessagePrefix(string id, DiagnosticSeverity severity, bool isWarningAsError, CultureInfo? culture)
    {
        return string.Format(culture, "{0} {1}",
            severity == DiagnosticSeverity.Error || isWarningAsError ? "error" : "warning",
            id);
    }

    public override int GetWarningLevel(int code)
    {
        return ErrorFacts.GetWarningLevel((ErrorCode)code);
    }

    public override Type ErrorCodeType => typeof(ErrorCode);

    public override Diagnostic CreateDiagnostic(int code, Location location, params object[] args)
    {
        var info = new WhamDiagnosticInfo((ErrorCode)code, args, ImmutableArray<Symbol>.Empty, ImmutableArray<Location>.Empty);
        return new WhamDiagnostic(info, location);
    }

    public override Diagnostic CreateDiagnostic(DiagnosticInfo info)
    {
        return new WhamDiagnostic(info, Location.None);
    }

    public override string GetErrorDisplayString(ISymbol symbol)
    {
        // show extra info if possible
        return symbol.ToString() ?? symbol.GetType().Name;
        // return SymbolDisplay.ToDisplayString(symbol, SymbolDisplayFormat.CSharpShortErrorMessageFormat);
    }

    public override ReportDiagnostic GetDiagnosticReport(DiagnosticInfo diagnosticInfo, CompilationOptions options)
    {
        return diagnosticInfo.Severity switch
        {
            DiagnosticSeverity.Hidden => ReportDiagnostic.Hidden,
            DiagnosticSeverity.Info => ReportDiagnostic.Info,
            DiagnosticSeverity.Warning => ReportDiagnostic.Warn,
            DiagnosticSeverity.Error => ReportDiagnostic.Error,
            _ => throw new InvalidOperationException($"Unknown value: {diagnosticInfo.Severity}"),
        };
    }

}
