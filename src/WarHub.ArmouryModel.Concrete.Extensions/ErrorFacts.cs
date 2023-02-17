using System.Globalization;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal static partial class ErrorFacts
{
    // private static string GetId(ErrorCode errorCode)
    // {
    //     return WhamMessageProvider.Instance.GetIdForErrorCode((int)errorCode);
    // }

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

        // FIXME in far future, replace with localizable resources

        return code switch
        {
            ErrorCode.ERR_GenericError =>
                "There was an uncategorized error. {0}",
            ErrorCode.ERR_SyntaxSupportNotYetImplemented =>
                "Support for this syntax was not yet implemented.",
            ErrorCode.ERR_UnknownEnumerationValue =>
                "Unknown enumeration value '{0}'.",
            ErrorCode.ERR_MissingGamesystem =>
                "Gamesystem is required for compilation, but was not found.",
            ErrorCode.ERR_MultipleGamesystems =>
                "Multiple gamesystems discovered, expected exactly one. Gamesystem '{0}' will be not used. Defaulting to first one provided, '{1}'.",
            ErrorCode.ERR_UnknownModuleType =>
                "This module was not recognized and will not be processed: '{0}'.",
            ErrorCode.ERR_NoBindingCandidates =>
                "No candidates for binding this reference were found (ref='{0}' in {1}). {2}",
            ErrorCode.ERR_MultipleViableBindingCandidates =>
                "Multiple candidates for binding this reference were found (ref='{0}' in {1}).",
            ErrorCode.ERR_UnviableBindingCandidates =>
                "There were candidates for binding this reference (ref='{0}' in {1}), but were disallowed due to some rules (visibility, scope).",
            _ => code.ToString(),
        };

        // return code.ToString();
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
