using System;
using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public abstract class EffectSymbol : LogicSymbol, IEffectSymbol
    {
        public EffectSymbol(ICatalogueItemSymbol containingSymbol) : base(containingSymbol)
        {
        }
    }

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
            EntrySymbol parentSymbol,
            Binder parentBinder,
            BindingDiagnosticContext diagnostics)
        {
            var entry = parentSymbol.Declaration;
            var itemCount = entry.Modifiers.Count + entry.ModifierGroups.Count;
            var builder = ImmutableArray.CreateBuilder<IEffectSymbol>(itemCount);
            foreach (var declaration in entry.Modifiers)
            {
                var effect = CreateEffect(parentSymbol, declaration, parentBinder, diagnostics);
                builder.Add(effect);
            }
            foreach (var declaration in entry.ModifierGroups)
            {
                var effect = CreateEffect(parentSymbol, declaration, parentBinder, diagnostics);
                builder.Add(effect);
            }
            return builder.MoveToImmutable();
        }

        public static IEffectSymbol CreateEffect(
            EntrySymbol parentSymbol,
            ModifierNode declaration,
            Binder parentBinder,
            BindingDiagnosticContext diagnostics)
        {
            if (declaration.Repeats.Count > 0)
            {
                // TODO consider what happens when there are both repeats and conditions
                // create a loop effect
                throw new NotImplementedException();
            }
            if (declaration.Conditions.Count > 0 || declaration.ConditionGroups.Count > 0)
            {
                // TODO consider what happens when there are both repeats and conditions
                // create a conditional effect
                throw new NotImplementedException();
            }
            // no repeats or conditions - plain modifier:
            return new ModifyingEffectSymbol(parentSymbol, declaration);
        }

        public static IEffectSymbol CreateEffect(
            EntrySymbol parentSymbol,
            ModifierGroupNode declaration,
            Binder parentBinder,
            BindingDiagnosticContext diagnostics)
        {
            if (declaration.Repeats.Count > 0)
            {
                // TODO consider what happens when there are both repeats and conditions
                // create a loop effect
                throw new NotImplementedException();
            }
            if (declaration.Conditions.Count > 0 || declaration.ConditionGroups.Count > 0)
            {
                // TODO consider what happens when there are both repeats and conditions
                // create a conditional effect
                throw new NotImplementedException();
            }
            // no repeats or conditions - plain modifier:
            return new ModifyingEffectSymbol(parentSymbol, default!);
        }
    }

    public class MergedConditionDeclaration
    {
        // TODO represents all condition/conditiongroup elements part of a single IConditionSymbol
    }

    public class MergedModifierDeclaration
    {
        // TODO represents all modifier/modifiergroup/repeat? elements part of a single IEffectSymbol
    }

    public class ConditionalEffectSymbol : EffectSymbol, IConditionalEffectSymbol
    {
        public ConditionalEffectSymbol(
            ICatalogueItemSymbol containingSymbol,
            IConditionSymbol condition,
            ImmutableArray<IEffectSymbol> satisfiedEffects,
            ImmutableArray<IEffectSymbol> unsatisfiedEffects)
            : base(containingSymbol)
        {
            Condition = condition;
            SatisfiedEffects = satisfiedEffects;
            UnsatisfiedEffects = unsatisfiedEffects;
        }

        public IConditionSymbol Condition { get; }

        public ImmutableArray<IEffectSymbol> SatisfiedEffects { get; }

        public ImmutableArray<IEffectSymbol> UnsatisfiedEffects { get; }
    }
}
