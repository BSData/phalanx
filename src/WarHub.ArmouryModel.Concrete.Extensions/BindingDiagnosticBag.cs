using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal sealed class BindingDiagnosticBag : BindingDiagnosticBag<IModuleSymbol>
{
    public static readonly BindingDiagnosticBag Discarded = new(null, null);

    public BindingDiagnosticBag()
        : this(usePool: false)
    { }

    private BindingDiagnosticBag(bool usePool)
        : base(usePool)
    { }

    public BindingDiagnosticBag(DiagnosticBag? diagnosticBag)
        : base(diagnosticBag, dependenciesBag: null)
    {
    }

    public BindingDiagnosticBag(DiagnosticBag? diagnosticBag, ICollection<IModuleSymbol>? dependenciesBag)
        : base(diagnosticBag, dependenciesBag)
    {
    }

    internal static BindingDiagnosticBag GetInstance()
    {
        return new BindingDiagnosticBag(usePool: true);
    }

    internal static BindingDiagnosticBag GetInstance(bool withDiagnostics, bool withDependencies)
    {
        if (withDiagnostics)
        {
            if (withDependencies)
            {
                return GetInstance();
            }

            return new BindingDiagnosticBag(DiagnosticBag.GetInstance());
        }
        else if (withDependencies)
        {
            return new BindingDiagnosticBag(diagnosticBag: null, PooledHashSet<IModuleSymbol>.GetInstance());
        }
        else
        {
            return Discarded;
        }
    }

    internal static BindingDiagnosticBag GetInstance(BindingDiagnosticBag template)
    {
        return GetInstance(template.AccumulatesDiagnostics, template.AccumulatesDependencies);
    }

    internal static BindingDiagnosticBag Create(BindingDiagnosticBag template)
    {
        if (template.AccumulatesDiagnostics)
        {
            if (template.AccumulatesDependencies)
            {
                return new BindingDiagnosticBag();
            }

            return new BindingDiagnosticBag(new DiagnosticBag());
        }
        else if (template.AccumulatesDependencies)
        {
            return new BindingDiagnosticBag(diagnosticBag: null, new HashSet<IModuleSymbol>());
        }
        else
        {
            return Discarded;
        }
    }

    internal void AddDependencies(Symbol? symbol)
    {
        if (symbol is not null && DependenciesBag is not null)
        {
            AddDependencies(symbol.GetUseSiteInfo());
        }
    }

    internal bool ReportUseSite(Symbol? symbol, SourceNode node)
    {
        return ReportUseSite(symbol, node.GetLocation());
    }

    internal bool ReportUseSite(Symbol? symbol, Location location)
    {
        if (symbol is not null)
        {
            return Add(symbol.GetUseSiteInfo(), location);
        }

        return false;
    }

    protected override bool ReportUseSiteDiagnostic(DiagnosticInfo diagnosticInfo, DiagnosticBag diagnosticBag, Location location)
    {
        return Symbol.ReportUseSiteDiagnostic(diagnosticInfo, diagnosticBag, location);
    }

    internal WhamDiagnosticInfo Add(ErrorCode code, Location location)
    {
        var info = new WhamDiagnosticInfo(code);
        Add(info, location);
        return info;
    }

    internal WhamDiagnosticInfo Add(ErrorCode code, Location location, params object[] args)
    {
        var info = new WhamDiagnosticInfo(code, args);
        Add(info, location);
        return info;
    }

    internal WhamDiagnosticInfo Add(ErrorCode code, Location location, ImmutableArray<Symbol> symbols, params object[] args)
    {
        var info = new WhamDiagnosticInfo(code, args, symbols, ImmutableArray<Location>.Empty);
        Add(info, location);
        return info;
    }

    internal void Add(DiagnosticInfo? info, Location location)
    {
        if (info is not null)
        {
            DiagnosticBag?.Add(info, location);
        }
    }
}
