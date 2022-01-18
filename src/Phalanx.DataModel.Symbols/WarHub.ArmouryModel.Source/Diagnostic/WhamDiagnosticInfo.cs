using Phalanx.DataModel.Symbols.Implementation;

namespace WarHub.ArmouryModel.Source;

internal sealed class WhamDiagnosticInfo : DiagnosticInfo
{
    public static readonly DiagnosticInfo EmptyErrorInfo = new WhamDiagnosticInfo(0);

    internal ImmutableArray<Symbol> Symbols { get; }

    internal WhamDiagnosticInfo(ErrorCode code)
        : this(code, Array.Empty<object>(), ImmutableArray<Symbol>.Empty, ImmutableArray<Location>.Empty)
    {
    }

    internal WhamDiagnosticInfo(ErrorCode code, params object[] args)
        : this(code, args, ImmutableArray<Symbol>.Empty, ImmutableArray<Location>.Empty)
    {
    }

    internal WhamDiagnosticInfo(ErrorCode code, ImmutableArray<Symbol> symbols, object[] args)
        : this(code, args, symbols, ImmutableArray<Location>.Empty)
    {
    }

    internal WhamDiagnosticInfo(ErrorCode code, object[] args, ImmutableArray<Symbol> symbols, ImmutableArray<Location> additionalLocations)
        : base(WhamMessageProvider.Instance, (int)code, args)
    {
        Symbols = symbols;
        AdditionalLocations = additionalLocations.IsDefaultOrEmpty ? Array.Empty<Location>() : additionalLocations;
    }

    public override IReadOnlyList<Location> AdditionalLocations { get; }

    internal new ErrorCode Code => (ErrorCode)base.Code;

    internal static bool IsEmpty(DiagnosticInfo info) => (object)info == EmptyErrorInfo;
}
