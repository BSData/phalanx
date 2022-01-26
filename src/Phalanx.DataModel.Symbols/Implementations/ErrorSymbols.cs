using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal static class ErrorSymbols
{
    internal class ErrorSymbolBase : ISymbol, IErrorSymbol
    {
        public SymbolKind Kind => SymbolKind.Error;

        public string? Id { get; init; }

        public string Name { get; init; } = "Error";

        public string? Comment => null;

        public ISymbol? ContainingSymbol => null;

        public ICatalogueSymbol? ContainingCatalogue => null;

        public IGamesystemNamespaceSymbol? ContainingNamespace => null;

        public ImmutableArray<ISymbol> CandidateSymbols { get; init; } = ImmutableArray<ISymbol>.Empty;

        public CandidateReason CandidateReason => CandidateSymbols.IsEmpty
            ? CandidateReason.None
            : ResultKind.ToCandidateReason();

        public LookupResultKind ResultKind { get; init; }

        public DiagnosticInfo? ErrorInfo { get; init; }

        public bool ErrorUnreported { get; init; }
    }

    internal class ErrorPublicationSymbol : ErrorSymbolBase, ICharacteristicTypeSymbol
    {
        public ResourceKind ResourceKind => ResourceKind.Publication;
    }

    internal class ErrorCharacteristicTypeSymbol : ErrorSymbolBase, ICharacteristicTypeSymbol
    {
        public ResourceKind ResourceKind => ResourceKind.Characteristic;
    }

    internal class ErrorCostTypeSymbol : ErrorSymbolBase, ICostTypeSymbol
    {
        public ResourceKind ResourceKind => ResourceKind.Cost;
    }

    internal class ErrorProfileTypeSymbol : ErrorSymbolBase, IProfileTypeSymbol
    {
        public ResourceKind ResourceKind => ResourceKind.Profile;

        ImmutableArray<ICharacteristicTypeSymbol> IProfileTypeSymbol.CharacteristicTypes =>
            ImmutableArray<ICharacteristicTypeSymbol>.Empty;
    }

    internal class ErrorProfileSymbol : ErrorResourceEntryBaseSymbol, IProfileSymbol
    {
        public override ResourceKind ResourceKind => ResourceKind.Profile;

        IProfileTypeSymbol Type { get; init; } = new ErrorProfileTypeSymbol();

        IProfileTypeSymbol IProfileSymbol.Type => Type;

        IResourceDefinitionSymbol? IResourceEntrySymbol.Type => Type;

        ImmutableArray<ICharacteristicSymbol> IProfileSymbol.Characteristics => ImmutableArray<ICharacteristicSymbol>.Empty;
    }

    internal class ErrorRuleSymbol : ErrorResourceEntryBaseSymbol, IRuleSymbol
    {
        public override ResourceKind ResourceKind => ResourceKind.Rule;

        string IRuleSymbol.DescriptionText => string.Empty;
    }

    internal class ErrorResourceGroupSymbol : ErrorResourceEntryBaseSymbol, IResourceGroupSymbol
    {
        public override ResourceKind ResourceKind => ResourceKind.Group;

        ImmutableArray<IResourceEntrySymbol> IResourceGroupSymbol.Resources =>
            ImmutableArray<IResourceEntrySymbol>.Empty;
    }

    internal class ErrorCategoryEntrySymbol : ErrorEntryBaseSymbol, ICategoryEntrySymbol
    {
        bool ICategoryEntrySymbol.IsPrimaryCategory => false;

        ICategoryEntrySymbol? ICategoryEntrySymbol.ReferencedEntry => null;
    }

    internal class ErrorForceEntrySymbol : ErrorEntryBaseSymbol, IForceEntrySymbol
    {
        ImmutableArray<IForceEntrySymbol> IForceEntrySymbol.ChildForces =>
            ImmutableArray<IForceEntrySymbol>.Empty;

        ImmutableArray<ICategoryEntrySymbol> IForceEntrySymbol.Categories =>
            ImmutableArray<ICategoryEntrySymbol>.Empty;
    }

    internal class ErrorSelectionEntrySymbol : ErrorSelectionEntryContainerSymbol, ISelectionEntrySymbol
    {
        bool ISelectionEntryContainerSymbol.IsSelectionEntry => true;

        SelectionEntryKind ISelectionEntrySymbol.EntryKind => SelectionEntryKind.Upgrade;
    }

    internal class ErrorSelectionEntryGroupSymbol : ErrorSelectionEntryContainerSymbol, ISelectionEntryGroupSymbol
    {
        bool ISelectionEntryContainerSymbol.IsSelectionGroup => true;

        ISelectionEntrySymbol? ISelectionEntryGroupSymbol.DefaultSelectionEntry => null;
    }

    internal abstract class ErrorSelectionEntryContainerSymbol : ErrorEntryBaseSymbol, ISelectionEntryContainerSymbol
    {
        ICategoryEntrySymbol? ISelectionEntryContainerSymbol.PrimaryCategory => null;

        bool ISelectionEntryContainerSymbol.IsSelectionEntry => false;

        bool ISelectionEntryContainerSymbol.IsSelectionGroup => false;

        ImmutableArray<ICategoryEntrySymbol> ISelectionEntryContainerSymbol.Categories =>
            ImmutableArray<ICategoryEntrySymbol>.Empty;

        ImmutableArray<ISelectionEntryContainerSymbol> ISelectionEntryContainerSymbol.ChildSelectionEntries =>
            ImmutableArray<ISelectionEntryContainerSymbol>.Empty;

        ISelectionEntryContainerSymbol? ISelectionEntryContainerSymbol.ReferencedEntry => null;
    }

    internal class ErrorEntryBaseSymbol : ErrorSymbolBase, IContainerEntrySymbol
    {
        IEntrySymbol? IEntrySymbol.ReferencedEntry => null;

        ImmutableArray<IConstraintSymbol> IContainerEntrySymbol.Constraints =>
            ImmutableArray<IConstraintSymbol>.Empty;

        ImmutableArray<IResourceEntrySymbol> IContainerEntrySymbol.Resources =>
            ImmutableArray<IResourceEntrySymbol>.Empty;

        bool IEntrySymbol.IsHidden => false;

        bool IEntrySymbol.IsReference => false;

        IPublicationReferenceSymbol? IEntrySymbol.PublicationReference => null;

        ImmutableArray<IEffectSymbol> IEntrySymbol.Effects => ImmutableArray<IEffectSymbol>.Empty;
    }

    internal abstract class ErrorResourceEntryBaseSymbol : ErrorEntryBaseSymbol, IResourceEntrySymbol
    {

        IResourceEntrySymbol? IResourceEntrySymbol.ReferencedEntry => null;

        IResourceDefinitionSymbol? IResourceEntrySymbol.Type => null;

        public abstract ResourceKind ResourceKind { get; }
    }

    internal class ErrorGamesystemSymbol : ErrorCatalogueBaseSymbol
    {
        public override bool IsGamesystem => true;

        public override ICatalogueSymbol Gamesystem => this;
    }

    internal class ErrorCatalogueSymbol : ErrorCatalogueBaseSymbol
    {
        private readonly Func<ICatalogueSymbol> gamesystemFunc;

        public ErrorCatalogueSymbol(Func<ICatalogueSymbol> gamesystemFunc)
        {
            this.gamesystemFunc = gamesystemFunc;
        }

        public override bool IsGamesystem => false;

        public override ICatalogueSymbol Gamesystem => gamesystemFunc();
    }

    internal abstract class ErrorCatalogueBaseSymbol : ErrorSymbolBase, ICatalogueSymbol
    {
        bool ICatalogueSymbol.IsLibrary => false;

        public abstract bool IsGamesystem { get; }

        public abstract ICatalogueSymbol Gamesystem { get; }

        ImmutableArray<ICatalogueReferenceSymbol> ICatalogueSymbol.Imports =>
            ImmutableArray<ICatalogueReferenceSymbol>.Empty;

        ImmutableArray<ISymbol> ICatalogueSymbol.AllItems =>
            ImmutableArray<ISymbol>.Empty;

        ImmutableArray<IResourceDefinitionSymbol> ICatalogueSymbol.ResourceDefinitions =>
            ImmutableArray<IResourceDefinitionSymbol>.Empty;

        ImmutableArray<IContainerEntrySymbol> ICatalogueSymbol.RootContainerEntries =>
            ImmutableArray<IContainerEntrySymbol>.Empty;

        ImmutableArray<IResourceEntrySymbol> ICatalogueSymbol.RootResourceEntries =>
            ImmutableArray<IResourceEntrySymbol>.Empty;

        ImmutableArray<ISelectionEntryContainerSymbol> ICatalogueSymbol.SharedSelectionEntryContainers =>
            ImmutableArray<ISelectionEntryContainerSymbol>.Empty;

        ImmutableArray<IResourceEntrySymbol> ICatalogueSymbol.SharedResourceEntries =>
            ImmutableArray<IResourceEntrySymbol>.Empty;
    }
}
