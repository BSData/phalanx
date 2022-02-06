using System.Globalization;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal static partial class ErrorFacts
{
    private static string GetId(ErrorCode errorCode)
    {
        return WhamMessageProvider.Instance.GetIdForErrorCode((int)errorCode);
    }

    internal static DiagnosticSeverity GetSeverity(ErrorCode code)
    {
        if (IsWarning(code))
        {
            return DiagnosticSeverity.Warning;
        }
        else if (IsInfo(code))
        {
            return DiagnosticSeverity.Info;
        }
        else if (IsHidden(code))
        {
            return DiagnosticSeverity.Hidden;
        }
        else
        {
            return DiagnosticSeverity.Error;
        }
    }

    private static bool IsHidden(ErrorCode code) =>
        code.ToString().StartsWith("HDN", StringComparison.Ordinal);

    private static bool IsInfo(ErrorCode code) =>
        code.ToString().StartsWith("INF", StringComparison.Ordinal);

    private static bool IsWarning(ErrorCode code) =>
        code.ToString().StartsWith("WRN", StringComparison.Ordinal);

    public static string GetMessage(ErrorCode code, CultureInfo? culture)
    {
        // string message = ResourceManager.GetString(code.ToString(), culture);
        // return message;
        return code.ToString();
    }

    public static LocalizableString GetMessageFormat(ErrorCode code)
    {
        // return new LocalizableResourceString(code.ToString(), ResourceManager, typeof(ErrorFacts));
        return code.ToString();
    }

    public static LocalizableString GetTitle(ErrorCode code)
    {
        // return new LocalizableResourceString(code.ToString() + s_titleSuffix, ResourceManager, typeof(ErrorFacts));
        return code.ToString();
    }

    public static LocalizableString GetDescription(ErrorCode code)
    {
        // return new LocalizableResourceString(code.ToString() + s_descriptionSuffix, ResourceManager, typeof(ErrorFacts));
        return null!;
    }

    public static string? GetHelpLink(ErrorCode code)
    {
        return null;
    }

    public static string GetCategory(ErrorCode code)
    {
        return Diagnostic.CompilerDiagnosticCategory;
    }

    internal static int GetWarningLevel(ErrorCode code)
    {
        if (IsInfo(code) || IsHidden(code))
        {
            // Info and hidden diagnostics should always be produced because some analyzers depend on them.
            return Diagnostic.InfoAndHiddenWarningLevel;
        }
        if (IsWarning(code))
        {
            return Diagnostic.DefaultWarningLevel;
        }
        // errors are 0
        return 0;
    }
}
