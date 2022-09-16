namespace WarHub.ArmouryModel;

public abstract class SymbolVisitor
{
    public virtual void Visit(ISymbol? symbol)
    {
        if (symbol is not null)
        {
            symbol.Accept(this);
        }
    }

    public virtual void DefaultVisit(ISymbol symbol) { }

    public virtual void VisitError(IErrorSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitGamesystemNamespace(IGamesystemNamespaceSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitCatalogue(ICatalogueSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitRoster(IRosterSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitResourceDefinition(IResourceDefinitionSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitResourceEntry(IResourceEntrySymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitResource(IResourceSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitContainerEntry(IContainerEntrySymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitContainerEntryInstance(IContainerEntryInstanceSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitCatalogueReference(ICatalogueReferenceSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitPublicationReference(IPublicationReferenceSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitEntryReferencePath(IEntryReferencePathSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitConstraint(IConstraintSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitCondition(IConditionSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitEffect(IEffectSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitQuery(IQuerySymbol symbol) =>
        DefaultVisit(symbol);

    public virtual void VisitRosterCost(IRosterCostSymbol symbol) =>
        DefaultVisit(symbol);
}

public abstract class SymbolVisitor<TResult>
{
    protected abstract TResult DefaultResult { get; }

    public virtual TResult Visit(ISymbol? symbol)
    {
        if (symbol is not null)
        {
            return symbol.Accept(this);
        }
        return DefaultResult;
    }

    public virtual TResult DefaultVisit(ISymbol symbol) => DefaultResult;

    public virtual TResult VisitError(IErrorSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitGamesystemNamespace(IGamesystemNamespaceSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitCatalogue(ICatalogueSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitRoster(IRosterSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitResourceDefinition(IResourceDefinitionSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitResourceEntry(IResourceEntrySymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitResource(IResourceSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitContainerEntry(IContainerEntrySymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitContainerEntryInstance(IContainerEntryInstanceSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitCatalogueReference(ICatalogueReferenceSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitPublicationReference(IPublicationReferenceSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitEntryReferencePath(IEntryReferencePathSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitConstraint(IConstraintSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitCondition(IConditionSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitEffect(IEffectSymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitQuery(IQuerySymbol symbol) =>
        DefaultVisit(symbol);

    public virtual TResult VisitRosterCost(IRosterCostSymbol symbol) =>
        DefaultVisit(symbol);
}

public abstract class SymbolVisitor<TArgument, TResult>
{
    protected abstract TResult DefaultResult { get; }

    public virtual TResult Visit(ISymbol? symbol, TArgument argument)
    {
        if (symbol is not null)
        {
            return symbol.Accept(this, argument);
        }
        return DefaultResult;
    }

    public virtual TResult DefaultVisit(ISymbol symbol, TArgument argument) =>
    DefaultResult;

    public virtual TResult VisitError(IErrorSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitGamesystemNamespace(IGamesystemNamespaceSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitCatalogue(ICatalogueSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitRoster(IRosterSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitResourceDefinition(IResourceDefinitionSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitResourceEntry(IResourceEntrySymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitResource(IResourceSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitContainerEntry(IContainerEntrySymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitContainerEntryInstance(IContainerEntryInstanceSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitCatalogueReference(ICatalogueReferenceSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitPublicationReference(IPublicationReferenceSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitEntryReferencePath(IEntryReferencePathSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitConstraint(IConstraintSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitCondition(IConditionSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitEffect(IEffectSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitQuery(IQuerySymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);

    public virtual TResult VisitRosterCost(IRosterCostSymbol symbol, TArgument argument) =>
        DefaultVisit(symbol, argument);
}
