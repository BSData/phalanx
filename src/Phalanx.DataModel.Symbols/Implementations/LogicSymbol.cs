using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal abstract class LogicSymbol : Symbol, IConditionSymbol
{
    public LogicSymbol(ISymbol containingSymbol)
    {
        ContainingSymbolCore = containingSymbol;
    }

    public sealed override ISymbol ContainingSymbol => ContainingSymbolCore;

    protected ISymbol ContainingSymbolCore { get; }

    public override string? Id => null;

    public override string Name => "";

    public override string? Comment => null;

    public override SymbolKind Kind => SymbolKind.Logic;

    public static ImmutableArray<IEffectSymbol> CreateEffects(
        EntrySymbol containingSymbol,
        DiagnosticBag diagnostics)
    {
        return CreateChildEffects().ToImmutableArray();

        IEnumerable<IEffectSymbol> CreateChildEffects()
        {
            foreach (var item in containingSymbol.Declaration.Modifiers)
            {
                yield return CreateEffect(containingSymbol, item, diagnostics);
            }
            foreach (var item in containingSymbol.Declaration.ModifierGroups)
            {
                yield return CreateEffect(containingSymbol, item, diagnostics);
            }
        }
    }

    public static IEffectSymbol CreateEffect(
        ISymbol containingSymbol,
        ModifierNode declaration,
        DiagnosticBag diagnostics)
    {
        return new ModifierEffectSymbol(containingSymbol, declaration, diagnostics);
    }

    public static IEffectSymbol CreateEffect(
        ISymbol containingSymbol,
        ModifierGroupNode declaration,
        DiagnosticBag diagnostics)
    {
        return new ModifierGroupEffectSymbol(containingSymbol, declaration, diagnostics);
    }
}
