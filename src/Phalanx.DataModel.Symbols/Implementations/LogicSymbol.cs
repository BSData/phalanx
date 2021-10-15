using System.Collections.Generic;
using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
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
            EntrySymbol entry,
            Binder parentBinder,
            BindingDiagnosticContext diagnostics)
        {
            return CreateChildEffects().ToImmutableArray();

            IEnumerable<IEffectSymbol> CreateChildEffects()
            {
                foreach (var item in entry.Declaration.Modifiers)
                {
                    yield return CreateEffect(entry, item, parentBinder, diagnostics);
                }
                foreach (var item in entry.Declaration.ModifierGroups)
                {
                    yield return CreateEffect(entry, item, parentBinder, diagnostics);
                }
            }
        }

        public static IEffectSymbol CreateEffect(
            ICatalogueItemSymbol parentSymbol,
            ModifierNode declaration,
            Binder parentBinder,
            BindingDiagnosticContext diagnostics)
        {
            return new ModifierEffectSymbol(parentSymbol, declaration, diagnostics);
        }

        public static IEffectSymbol CreateEffect(
            ICatalogueItemSymbol parentSymbol,
            ModifierGroupNode declaration,
            Binder parentBinder,
            BindingDiagnosticContext diagnostics)
        {
            return new ModifierGroupEffectSymbol(parentSymbol, declaration, parentBinder, diagnostics);
        }
    }
}
