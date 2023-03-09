using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ModifierEffectSymbol : ModifierEffectBaseSymbol, IEffectSymbol, INodeDeclaredSymbol<ModifierNode>
{
    private ISymbol? lazyOperand;
    private ISymbol? lazyTargetMember;

    public ModifierEffectSymbol(
        ISymbol? containingSymbol,
        ModifierNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        FunctionKind = declaration.Type switch
        {
            ModifierKind.Set => EffectOperation.SetValue,
            ModifierKind.Increment => EffectOperation.IncrementValue,
            ModifierKind.Decrement => EffectOperation.DecrementValue,
            ModifierKind.Append => EffectOperation.AppendText,
            ModifierKind.Add => EffectOperation.AddCategory,
            ModifierKind.Remove => EffectOperation.RemoveCategory,
            ModifierKind.SetPrimary => EffectOperation.SetCategoryPrimary,
            ModifierKind.UnsetPrimary => EffectOperation.UnsetCategoryPrimary,
            _ => EffectOperation.None,
        };
        if (FunctionKind is EffectOperation.None)
            diagnostics.Add(ErrorCode.ERR_UnknownEnumerationValue, declaration.GetLocation(), declaration.Type);
        TargetKind = declaration.Field switch
        {
            "name" => EffectTargetKind.SymbolName,
            "page" => EffectTargetKind.PublicationPage,
            "hidden" => EffectTargetKind.EntryHiddenState,
            "category" => EffectTargetKind.EntryCategory,
            "description" => EffectTargetKind.RuleDescription,
            { } id => EffectTargetKind.Member,
            null => EffectTargetKind.None,
        };
        if (TargetKind is EffectTargetKind.None)
            diagnostics.Add(ErrorCode.ERR_UnknownEnumerationValue, declaration.GetLocation(), declaration.Field ?? "field");

        // TODO diagnostics when FunctionKind is *Category but TargetKind is not EntryCategory etc.
        // TODO diagnostics when FunctionKind is numeric but OperandValue is not (e.g. increase by 'a'?)
    }

    public new ModifierNode Declaration { get; }

    public override EffectTargetKind TargetKind { get; }

    public override ISymbol? TargetMember => GetOptionalBoundField(ref lazyTargetMember);

    public override EffectOperation FunctionKind { get; }

    public override string? OperandValue => Declaration.Value;

    public override ISymbol? OperandSymbol => GetOptionalBoundField(ref lazyOperand);

    public override ImmutableArray<ModifierEffectBaseSymbol> ChildrenWhenSatisfied =>
        ImmutableArray<ModifierEffectBaseSymbol>.Empty;

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);
        if (TargetKind is EffectTargetKind.EntryCategory)
        {
            lazyOperand = binder.BindCategoryEntrySymbol(Declaration, Declaration.Value, diagnostics);
        }
        else if (TargetKind is EffectTargetKind.Member)
        {
            lazyTargetMember = binder.BindEffectTargetMemberSymbol(Declaration, Declaration.Field, diagnostics);
        }
    }
}
