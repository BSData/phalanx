using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

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

    public override void Accept(SymbolVisitor visitor) =>
        visitor.VisitLogic(this);

    public override TResult Accept<TResult>(SymbolVisitor<TResult> visitor) =>
        visitor.VisitLogic(this);

    public override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument) =>
        visitor.VisitLogic(this, argument);

    public static ImmutableArray<EffectSymbol> CreateEffects(
        EntrySymbol containingSymbol,
        EntryBaseNode declaration,
        DiagnosticBag diagnostics)
    {
        return CreateChildEffects().ToImmutableArray();

        IEnumerable<EffectSymbol> CreateChildEffects()
        {
            foreach (var item in declaration.Modifiers)
            {
                yield return CreateEffect(containingSymbol, item, diagnostics);
            }
            foreach (var item in declaration.ModifierGroups)
            {
                yield return CreateEffect(containingSymbol, item, diagnostics);
            }
        }
    }

    public static EffectSymbol CreateEffect(
        ISymbol containingSymbol,
        ModifierNode declaration,
        DiagnosticBag diagnostics)
    {
        return new ModifierEffectSymbol(containingSymbol, declaration, diagnostics);
    }

    public static EffectSymbol CreateEffect(
        ISymbol containingSymbol,
        ModifierGroupNode declaration,
        DiagnosticBag diagnostics)
    {
        return new ModifierGroupEffectSymbol(containingSymbol, declaration, diagnostics);
    }
}
