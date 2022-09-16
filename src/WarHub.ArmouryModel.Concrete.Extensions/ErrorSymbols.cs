using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

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
        ResourceKind.Characteristic => new ErrorResourceDefinitionSymbol(),
        ResourceKind.Cost => new ErrorResourceDefinitionSymbol(),
        ResourceKind.Profile => new ErrorResourceDefinitionSymbol(),
        ResourceKind.Error => new ErrorResourceDefinitionSymbol(),
        ResourceKind.Publication => new ErrorPublicationSymbol(),
        _ => throw new NotSupportedException($"Can't instantiate error symbol for '{kind}' resource definition."),
    };

    public static ErrorSymbolBase CreateResourceEntry(ResourceKind kind) => kind switch

    {
        ResourceKind.Characteristic => new ErrorCharacteristicSymbol(),
        ResourceKind.Cost => new ErrorCostSymbol(),
        ResourceKind.Profile => new ErrorProfileSymbol(),
        ResourceKind.Rule => new ErrorRuleSymbol(),
        ResourceKind.Group => new ErrorResourceEntrySymbol(),
        ResourceKind.Error => new ErrorResourceEntrySymbol(),
        _ => throw new NotSupportedException($"Can't instantiate error symbol for '{kind}' resource."),
    };

    public static ErrorSymbolBase CreateContainerEntry(ContainerKind kind) => kind switch
    {
        ContainerKind.Selection => new ErrorSelectionEntrySymbol(),
        ContainerKind.SelectionGroup => new ErrorSelectionEntryGroupSymbol(),
        ContainerKind.Category => new ErrorCategoryEntrySymbol(),
        ContainerKind.Force => new ErrorForceEntrySymbol(),
        ContainerKind.Error => new ErrorContainerEntrySymbol(),
        _ => throw new NotSupportedException($"Can't instantiate error symbol for '{kind}' resource."),
    };

    internal record ErrorSymbolBase : ISymbol, IErrorSymbol
    {
        SymbolKind ISymbol.Kind => SymbolKind.Error;

        public string? Id { get; init; }

        public string Name { get; init; } = "Error";

        public string? Comment => null;

        public ISymbol? ContainingSymbol => null;

        public IModuleSymbol? ContainingModule => null;

        public IGamesystemNamespaceSymbol? ContainingNamespace => null;

        public ImmutableArray<ISymbol> CandidateSymbols { get; init; } = ImmutableArray<ISymbol>.Empty;

        public CandidateReason CandidateReason => CandidateSymbols.IsEmpty
            ? CandidateReason.None
            : ResultKind.ToCandidateReason();

        public LookupResultKind ResultKind { get; init; }

        public DiagnosticInfo? ErrorInfo { get; init; }

        public bool ErrorUnreported { get; init; }

        void ISymbol.Accept(SymbolVisitor visitor)
        {
            visitor.VisitError(this);
        }

        TResult ISymbol.Accept<TResult>(SymbolVisitor<TResult> visitor)
        {
            return visitor.VisitError(this);
        }

        TResult ISymbol.Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitError(this, argument);
        }
    }

    internal record ErrorPublicationSymbol : ErrorResourceDefinitionSymbol, IPublicationSymbol
    {
        string? IPublicationSymbol.ShortName => null;

        string? IPublicationSymbol.Publisher => null;

        DateOnly? IPublicationSymbol.PublicationDate => null;

        Uri? IPublicationSymbol.PublicationUrl => null;
    }

    internal record ErrorResourceDefinitionSymbol : ErrorSymbolBase, IResourceDefinitionSymbol
    {
        ResourceKind IResourceDefinitionSymbol.ResourceKind => ResourceKind.Error;

        ImmutableArray<IResourceDefinitionSymbol> IResourceDefinitionSymbol.Definitions =>
            ImmutableArray<IResourceDefinitionSymbol>.Empty;
    }

    internal record ErrorCostSymbol : ErrorResourceEntrySymbol, ICostSymbol
    {
        decimal ICostSymbol.Value => default;
        string ICostSymbol.TypeId => string.Empty;

    }

    internal record ErrorCharacteristicSymbol : ErrorResourceEntrySymbol, ICharacteristicSymbol
    {
        string ICharacteristicSymbol.Value => string.Empty;
    }

    internal record ErrorProfileSymbol : ErrorResourceEntrySymbol, IProfileSymbol
    {
        IResourceDefinitionSymbol Type { get; init; } = new ErrorResourceDefinitionSymbol();

        IResourceDefinitionSymbol? IResourceEntrySymbol.Type => Type;

        ImmutableArray<ICharacteristicSymbol> IProfileSymbol.Characteristics => ImmutableArray<ICharacteristicSymbol>.Empty;
    }

    internal record ErrorRuleSymbol : ErrorResourceEntrySymbol, IRuleSymbol
    {
        string IRuleSymbol.DescriptionText => string.Empty;
    }

    internal record ErrorCategoryEntrySymbol : ErrorContainerEntrySymbol, ICategoryEntrySymbol
    {
        bool ICategoryEntrySymbol.IsPrimaryCategory => false;

        ICategoryEntrySymbol? ICategoryEntrySymbol.ReferencedEntry => null;
    }

    internal record ErrorForceEntrySymbol : ErrorContainerEntrySymbol, IForceEntrySymbol
    {
        ImmutableArray<IForceEntrySymbol> IForceEntrySymbol.ChildForces =>
            ImmutableArray<IForceEntrySymbol>.Empty;

        ImmutableArray<ICategoryEntrySymbol> IForceEntrySymbol.Categories =>
            ImmutableArray<ICategoryEntrySymbol>.Empty;
    }

    internal record ErrorSelectionEntrySymbol : ErrorSelectionEntryContainerSymbol, ISelectionEntrySymbol
    {
        SelectionEntryKind ISelectionEntrySymbol.EntryKind => SelectionEntryKind.Upgrade;
    }

    internal record ErrorSelectionEntryGroupSymbol : ErrorSelectionEntryContainerSymbol, ISelectionEntryGroupSymbol
    {
        ISelectionEntryContainerSymbol? ISelectionEntryGroupSymbol.DefaultSelectionEntry => null;
    }

    internal record ErrorSelectionEntryContainerSymbol : ErrorContainerEntrySymbol, ISelectionEntryContainerSymbol
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

        ImmutableArray<IResourceEntrySymbol> IEntrySymbol.Resources => ImmutableArray<IResourceEntrySymbol>.Empty;
    }

    internal record ErrorResourceEntrySymbol : ErrorEntryBaseSymbol, IResourceEntrySymbol
    {
        ResourceKind IResourceEntrySymbol.ResourceKind => ResourceKind.Error;

        IResourceEntrySymbol? IResourceEntrySymbol.ReferencedEntry => null;

        IResourceDefinitionSymbol? IResourceEntrySymbol.Type => null;
    }

    internal record ErrorContainerEntrySymbol : ErrorEntryBaseSymbol, IContainerEntrySymbol
    {
        ContainerKind IContainerEntrySymbol.ContainerKind => ContainerKind.Error;

        ImmutableArray<IConstraintSymbol> IContainerEntrySymbol.Constraints =>
            ImmutableArray<IConstraintSymbol>.Empty;

        ImmutableArray<ICostSymbol> IContainerEntrySymbol.Costs =>
            ImmutableArray<ICostSymbol>.Empty;
    }

    internal record ErrorGamesystemSymbol : ErrorCatalogueBaseSymbol
    {
        public override bool IsGamesystem => true;

        public override ICatalogueSymbol Gamesystem => this;
    }

    internal record ErrorCatalogueSymbol : ErrorCatalogueBaseSymbol
    {
        public Func<ICatalogueSymbol>? GamesystemProvider { get; init; }

        public override bool IsGamesystem => false;

        public override ICatalogueSymbol Gamesystem =>
            GamesystemProvider?.Invoke() ?? new ErrorGamesystemSymbol();
    }

    internal abstract record ErrorCatalogueBaseSymbol : ErrorSymbolBase, ICatalogueSymbol
    {
        bool ICatalogueSymbol.IsLibrary => false;

        public abstract bool IsGamesystem { get; }

        public abstract ICatalogueSymbol Gamesystem { get; }

        ImmutableArray<ICatalogueReferenceSymbol> ICatalogueSymbol.CatalogueReferences =>
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
