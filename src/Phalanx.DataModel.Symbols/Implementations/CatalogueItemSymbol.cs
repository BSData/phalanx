using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class CatalogueItemSymbol : Symbol, ICatalogueItemSymbol
{
    protected CatalogueItemSymbol(
        ICatalogueItemSymbol containingSymbol,
        SourceNode declaration,
        BindingDiagnosticContext diagnostics)
    {
        ContainingSymbolCore = containingSymbol;
        Id = (declaration as IIdentifiableNode)?.Id;
        Name = (declaration as INameableNode)?.Name ?? string.Empty;
        Comment = (declaration as CommentableNode)?.Comment;
    }

    public string? Id { get; }

    public sealed override string Name { get; }

    public sealed override string? Comment { get; }

    public sealed override ISymbol ContainingSymbol => ContainingSymbolCore;

    protected ICatalogueItemSymbol ContainingSymbolCore { get; }

    public ICatalogueSymbol ContainingCatalogue => ContainingSymbolCore.ContainingCatalogue;
}
