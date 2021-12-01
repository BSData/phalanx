using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class LogicSymbol : Symbol, IConditionSymbol
{
    public LogicSymbol(ICatalogueItemSymbol containingSymbol)
    {
        ContainingSymbolCore = containingSymbol;
    }

    public sealed override ISymbol ContainingSymbol => ContainingSymbolCore;

    protected ICatalogueItemSymbol ContainingSymbolCore { get; }

    public ICatalogueSymbol ContainingCatalogue => ContainingSymbolCore.ContainingCatalogue;

    internal override Compilation DeclaringCompilation => ((CatalogueBaseSymbol)ContainingCatalogue).DeclaringCompilation;

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
        ICatalogueItemSymbol containingSymbol,
        ModifierNode declaration,
        DiagnosticBag diagnostics)
    {
        return new ModifierEffectSymbol(containingSymbol, declaration, diagnostics);
    }

    public static IEffectSymbol CreateEffect(
        ICatalogueItemSymbol containingSymbol,
        ModifierGroupNode declaration,
        DiagnosticBag diagnostics)
    {
        return new ModifierGroupEffectSymbol(containingSymbol, declaration, diagnostics);
    }
}
