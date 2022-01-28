using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal static class ErrorSymbols
{
    public static TErrorSymbol WithErrorDetailsFrom<TErrorSymbol>(this TErrorSymbol @this, ErrorSymbolBase other)
        where TErrorSymbol : ErrorSymbolBase
    {
        return @this with
        {
            CandidateSymbols = other.CandidateSymbols,
            ResultKind = other.ResultKind,
            ErrorInfo = other.ErrorInfo,
            ErrorUnreported = other.ErrorUnreported,
        };
    }

    public static ErrorSymbolBase CreateResourceDefinition(ResourceKind kind) => kind switch

    {
        ResourceKind.Characteristic => new ErrorCharacteristicTypeSymbol(),
        ResourceKind.Cost => new ErrorCostTypeSymbol(),
        ResourceKind.Profile => new ErrorProfileTypeSymbol(),
        _ => throw new NotSupportedException($"Can't instantiate error symbol for '{kind}' resource definition."),
    };

    public static ErrorSymbolBase CreateResourceEntry(ResourceKind kind) => kind switch

    {
        ResourceKind.Characteristic => new ErrorCharacteristicTypeSymbol(),
        ResourceKind.Cost => new ErrorCostTypeSymbol(),
        ResourceKind.Profile => new ErrorProfileSymbol(),
        ResourceKind.Publication => new ErrorPublicationSymbol(),
        ResourceKind.Rule => new ErrorRuleSymbol(),
        ResourceKind.Group => new ErrorResourceGroupSymbol(),
        _ => throw new NotSupportedException($"Can't instantiate error symbol for '{kind}' resource."),
    };

    public static ErrorSymbolBase CreateContainerEntry(ContainerEntryKind kind) => kind switch
    {
        ContainerEntryKind.Selection => new ErrorSelectionEntrySymbol(),
        ContainerEntryKind.SelectionGroup => new ErrorSelectionEntryGroupSymbol(),
        ContainerEntryKind.Category => new ErrorCategoryEntrySymbol(),
        ContainerEntryKind.Force => new ErrorForceEntrySymbol(),
        _ => throw new NotSupportedException($"Can't instantiate error symbol for '{kind}' resource."),
    };

    internal record ErrorSymbolBase : ISymbol, IErrorSymbol
    {
        public virtual SymbolKind Kind => SymbolKind.Error;

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

    internal record ErrorPublicationSymbol : ErrorSymbolBase, IPublicationSymbol
    {
        public override SymbolKind Kind => SymbolKind.Resource;

        public ResourceKind ResourceKind => ResourceKind.Publication;

        string? IPublicationSymbol.ShortName => null;

        string? IPublicationSymbol.Publisher => null;

        DateOnly? IPublicationSymbol.PublicationDate => null;

        Uri? IPublicationSymbol.PublicationUrl => null;
    }

    internal record ErrorCharacteristicTypeSymbol : ErrorSymbolBase, ICharacteristicTypeSymbol
    {
        public override SymbolKind Kind => SymbolKind.ResourceDefinition;

        public ResourceKind ResourceKind => ResourceKind.Characteristic;
    }

    internal record ErrorCostTypeSymbol : ErrorSymbolBase, ICostTypeSymbol
    {
        public override SymbolKind Kind => SymbolKind.ResourceDefinition;

        public ResourceKind ResourceKind => ResourceKind.Cost;
    }

    internal record ErrorProfileTypeSymbol : ErrorSymbolBase, IProfileTypeSymbol
    {
        public override SymbolKind Kind => SymbolKind.ResourceDefinition;

        public ResourceKind ResourceKind => ResourceKind.Profile;

        ImmutableArray<ICharacteristicTypeSymbol> IProfileTypeSymbol.CharacteristicTypes =>
            ImmutableArray<ICharacteristicTypeSymbol>.Empty;
    }

    internal record ErrorProfileSymbol : ErrorResourceEntryBaseSymbol, IProfileSymbol
    {
        public override ResourceKind ResourceKind => ResourceKind.Profile;

        IProfileTypeSymbol Type { get; init; } = new ErrorProfileTypeSymbol();

        IProfileTypeSymbol IProfileSymbol.Type => Type;

        IResourceDefinitionSymbol? IResourceEntrySymbol.Type => Type;

        ImmutableArray<ICharacteristicSymbol> IProfileSymbol.Characteristics => ImmutableArray<ICharacteristicSymbol>.Empty;
    }

    internal record ErrorRuleSymbol : ErrorResourceEntryBaseSymbol, IRuleSymbol
    {
        public override ResourceKind ResourceKind => ResourceKind.Rule;

        string IRuleSymbol.DescriptionText => string.Empty;
    }

    internal record ErrorResourceGroupSymbol : ErrorResourceEntryBaseSymbol, IResourceGroupSymbol
    {
        public override ResourceKind ResourceKind => ResourceKind.Group;

        ImmutableArray<IResourceEntrySymbol> IResourceGroupSymbol.Resources =>
            ImmutableArray<IResourceEntrySymbol>.Empty;
    }

    internal record ErrorCategoryEntrySymbol : ErrorContainerEntryBaseSymbol, ICategoryEntrySymbol
    {
        public override ContainerEntryKind ContainerKind => ContainerEntryKind.Category;

        bool ICategoryEntrySymbol.IsPrimaryCategory => false;

        ICategoryEntrySymbol? ICategoryEntrySymbol.ReferencedEntry => null;
    }

    internal record ErrorForceEntrySymbol : ErrorContainerEntryBaseSymbol, IForceEntrySymbol
    {
        public override ContainerEntryKind ContainerKind => ContainerEntryKind.Force;

        ImmutableArray<IForceEntrySymbol> IForceEntrySymbol.ChildForces =>
            ImmutableArray<IForceEntrySymbol>.Empty;

        ImmutableArray<ICategoryEntrySymbol> IForceEntrySymbol.Categories =>
            ImmutableArray<ICategoryEntrySymbol>.Empty;
    }

    internal record ErrorSelectionEntrySymbol : ErrorSelectionEntryContainerSymbol, ISelectionEntrySymbol
    {
        public override ContainerEntryKind ContainerKind => ContainerEntryKind.Selection;

        SelectionEntryKind ISelectionEntrySymbol.EntryKind => SelectionEntryKind.Upgrade;
    }

    internal record ErrorSelectionEntryGroupSymbol : ErrorSelectionEntryContainerSymbol, ISelectionEntryGroupSymbol
    {
        public override ContainerEntryKind ContainerKind => ContainerEntryKind.SelectionGroup;

        ISelectionEntrySymbol? ISelectionEntryGroupSymbol.DefaultSelectionEntry => null;
    }

    internal abstract record ErrorSelectionEntryContainerSymbol : ErrorContainerEntryBaseSymbol, ISelectionEntryContainerSymbol
    {
        ICategoryEntrySymbol? ISelectionEntryContainerSymbol.PrimaryCategory => null;

        ImmutableArray<ICategoryEntrySymbol> ISelectionEntryContainerSymbol.Categories =>
            ImmutableArray<ICategoryEntrySymbol>.Empty;

        ImmutableArray<ISelectionEntryContainerSymbol> ISelectionEntryContainerSymbol.ChildSelectionEntries =>
            ImmutableArray<ISelectionEntryContainerSymbol>.Empty;

        ISelectionEntryContainerSymbol? ISelectionEntryContainerSymbol.ReferencedEntry => null;
    }

    internal abstract record ErrorEntryBaseSymbol : ErrorSymbolBase, IEntrySymbol
    {
        IEntrySymbol? IEntrySymbol.ReferencedEntry => null;

        bool IEntrySymbol.IsHidden => false;

        bool IEntrySymbol.IsReference => false;

        IPublicationReferenceSymbol? IEntrySymbol.PublicationReference => null;

        ImmutableArray<IEffectSymbol> IEntrySymbol.Effects => ImmutableArray<IEffectSymbol>.Empty;
    }

    internal abstract record ErrorResourceEntryBaseSymbol : ErrorEntryBaseSymbol, IResourceEntrySymbol
    {
        public override SymbolKind Kind => SymbolKind.Resource;

        IResourceEntrySymbol? IResourceEntrySymbol.ReferencedEntry => null;

        IResourceDefinitionSymbol? IResourceEntrySymbol.Type => null;

        public abstract ResourceKind ResourceKind { get; }
    }

    internal abstract record ErrorContainerEntryBaseSymbol : ErrorEntryBaseSymbol, IContainerEntrySymbol
    {
        public override SymbolKind Kind => SymbolKind.ContainerEntry;

        public abstract ContainerEntryKind ContainerKind { get; }

        ImmutableArray<IConstraintSymbol> IContainerEntrySymbol.Constraints =>
            ImmutableArray<IConstraintSymbol>.Empty;

        ImmutableArray<IResourceEntrySymbol> IContainerEntrySymbol.Resources =>
            ImmutableArray<IResourceEntrySymbol>.Empty;
    }

    internal record ErrorGamesystemSymbol : ErrorCatalogueBaseSymbol
    {
        public override bool IsGamesystem => true;

        public override ICatalogueSymbol Gamesystem => this;
    }

    internal record ErrorCatalogueSymbol : ErrorCatalogueBaseSymbol
    {
        private readonly Func<ICatalogueSymbol> gamesystemFunc;

        public ErrorCatalogueSymbol(Func<ICatalogueSymbol> gamesystemFunc)
        {
            this.gamesystemFunc = gamesystemFunc;
        }

        public override bool IsGamesystem => false;

        public override ICatalogueSymbol Gamesystem => gamesystemFunc();
    }

    internal abstract record ErrorCatalogueBaseSymbol : ErrorSymbolBase, ICatalogueSymbol
    {
        public override SymbolKind Kind => SymbolKind.Catalogue;

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