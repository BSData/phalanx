using System.Collections.Generic;
using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class LogicSymbol : Symbol, IConditionSymbol
{
    public LogicSymbol(ICatalogueItemSymbol containingSymbol)
    {
        ContainingSymbol = containingSymbol;
    }

    public override string Name => "";

    public override string? Comment => null;

    public override SymbolKind Kind => SymbolKind.Logic;

    public ICatalogueSymbol ContainingCatalogue => ContainingSymbol.ContainingCatalogue;

    public override ICatalogueItemSymbol ContainingSymbol { get; }

    public static ImmutableArray<IEffectSymbol> CreateEffects(
        EntrySymbol containingSymbol,
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return CreateChildEffects().ToImmutableArray();

        IEnumerable<IEffectSymbol> CreateChildEffects()
        {
            foreach (var item in containingSymbol.Declaration.Modifiers)
            {
                yield return CreateEffect(containingSymbol, item, binder, diagnostics);
            }
            foreach (var item in containingSymbol.Declaration.ModifierGroups)
            {
                yield return CreateEffect(containingSymbol, item, binder, diagnostics);
            }
        }
    }

    public static IEffectSymbol CreateEffect(
        ICatalogueItemSymbol containingSymbol,
        ModifierNode declaration,
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return new ModifierEffectSymbol(containingSymbol, declaration, diagnostics);
    }

    public static IEffectSymbol CreateEffect(
        ICatalogueItemSymbol containingSymbol,
        ModifierGroupNode declaration,
        Binder binder,
        BindingDiagnosticContext diagnostics)
    {
        return new ModifierGroupEffectSymbol(containingSymbol, declaration, binder, diagnostics);
    }
}
