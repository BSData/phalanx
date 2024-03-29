using WarHub.ArmouryModel.Concrete;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel;

public static class WhamSymbolDeclarationExtensions
{
    public static SourceNode? GetDeclaration(this ISymbol symbol) =>
        GetDeclarationCore<SourceDeclaredSymbol, SourceNode>(symbol);

    public static RosterNode? GetDeclaration(this IRosterSymbol symbol) =>
        GetDeclarationCore<RosterSymbol, RosterNode>(symbol);

    public static SelectionNode? GetDeclaration(this ISelectionSymbol symbol) =>
        GetDeclarationCore<SelectionSymbol, SelectionNode>(symbol);

    public static GamesystemNode? GetGamesystemDeclaration(this ICatalogueSymbol symbol) =>
        GetDeclarationCore<GamesystemSymbol, GamesystemNode>(symbol);

    public static CatalogueNode? GetCatalogueDeclaration(this ICatalogueSymbol symbol) =>
        GetDeclarationCore<CatalogueSymbol, CatalogueNode>(symbol);

    public static CatalogueLinkNode? GetDeclaration(this ICatalogueReferenceSymbol symbol) =>
        GetDeclarationCore<CatalogueReferenceSymbol, CatalogueLinkNode>(symbol);

    public static CategoryEntryNode? GetEntryDeclaration(this ICategoryEntrySymbol symbol) =>
        GetDeclarationCore<CategoryEntrySymbol, CategoryEntryNode>(symbol);

    public static CategoryLinkNode? GetLinkDeclaration(this ICategoryEntrySymbol symbol) =>
        GetDeclarationCore<CategoryLinkSymbol, CategoryLinkNode>(symbol);

    public static CostTypeNode? GetCostTypeDeclaration(this IResourceDefinitionSymbol symbol) =>
        GetDeclarationCore<CostTypeSymbol, CostTypeNode>(symbol);

    public static CharacteristicTypeNode? GetCharacteristicTypeDeclaration(this IResourceDefinitionSymbol symbol) =>
        GetDeclarationCore<CharacteristicTypeSymbol, CharacteristicTypeNode>(symbol);

    public static ProfileTypeNode? GetProfileTypeDeclaration(this IResourceDefinitionSymbol symbol) =>
        GetDeclarationCore<ProfileTypeSymbol, ProfileTypeNode>(symbol);

    public static PublicationNode? GetPublicationDeclaration(this IResourceDefinitionSymbol symbol) =>
        GetDeclarationCore<PublicationSymbol, PublicationNode>(symbol);

    public static PublicationNode? GetDeclaration(this IPublicationSymbol symbol) =>
        GetDeclarationCore<PublicationSymbol, PublicationNode>(symbol);

    public static CostNode? GetDeclaration(this ICostSymbol symbol) =>
        GetDeclarationCore<CostSymbol, CostNode>(symbol);

    public static CharacteristicNode? GetDeclaration(this ICharacteristicSymbol symbol) =>
        GetDeclarationCore<CharacteristicSymbol, CharacteristicNode>(symbol);

    public static ProfileNode? GetEntryDeclaration(this IProfileSymbol symbol) =>
        GetDeclarationCore<ProfileSymbol, ProfileNode>(symbol);

    public static InfoGroupNode? GetGroupDeclaration(this IResourceEntrySymbol symbol) =>
        GetDeclarationCore<ResourceGroupSymbol, InfoGroupNode>(symbol);

    public static InfoLinkNode? GetLinkDeclaration(this IResourceEntrySymbol symbol) =>
        GetDeclarationCore<ResourceLinkSymbol, InfoLinkNode>(symbol);

    public static RuleNode? GetDeclaration(this IRuleSymbol symbol) =>
        GetDeclarationCore<RuleSymbol, RuleNode>(symbol);

    public static ForceEntryNode? GetDeclaration(this IForceEntrySymbol symbol) =>
        GetDeclarationCore<ForceEntrySymbol, ForceEntryNode>(symbol);

    public static SelectionEntryNode? GetEntryDeclaration(this ISelectionEntryContainerSymbol symbol) =>
        GetDeclarationCore<SelectionEntrySymbol, SelectionEntryNode>(symbol);

    public static SelectionEntryGroupNode? GetEntryGroupDeclaration(this ISelectionEntryContainerSymbol symbol) =>
        GetDeclarationCore<SelectionEntryGroupSymbol, SelectionEntryGroupNode>(symbol);

    public static EntryLinkNode? GetEntryLinkDeclaration(this ISelectionEntryContainerSymbol symbol) =>
        GetDeclarationCore<SelectionEntryLinkSymbol, EntryLinkNode>(symbol);

    public static ConstraintNode? GetDeclaration(this IConstraintSymbol symbol) =>
        GetDeclarationCore<ConstraintSymbol, ConstraintNode>(symbol);

    public static ModifierNode? GetModifierDeclaration(this IEffectSymbol symbol) =>
        GetDeclarationCore<ModifierEffectSymbol, ModifierNode>(symbol);

    public static ModifierGroupNode? GetModifierGroupDeclaration(this IEffectSymbol symbol) =>
        GetDeclarationCore<ModifierGroupEffectSymbol, ModifierGroupNode>(symbol);

    public static RepeatNode? GetRepeatDeclaration(this IEffectSymbol symbol) =>
        GetDeclarationCore<RepeatEffectSymbol, RepeatNode>(symbol);

    public static ConditionNode? GetConditionDeclaration(this IConditionSymbol symbol) =>
        GetDeclarationCore<ConditionSymbol, ConditionNode>(symbol);

    public static ConditionGroupNode? GetConditionGroupDeclaration(this IConditionSymbol symbol) =>
        GetDeclarationCore<ConditionGroupingBaseSymbol.ConditionGroupSymbol, ConditionGroupNode>(symbol);

    private static TNode? GetDeclarationCore<TSymbol, TNode>(ISymbol symbol)
        where TSymbol : ISymbol, INodeDeclaredSymbol<TNode>
        where TNode : SourceNode =>
        symbol is TSymbol { Declaration: var decl } ? decl : null;
}
