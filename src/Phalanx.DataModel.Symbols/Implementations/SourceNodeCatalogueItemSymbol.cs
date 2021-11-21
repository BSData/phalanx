using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class SourceCatalogueItemSymbol : CatalogueItemSymbol
{
    protected SourceCatalogueItemSymbol(
        ICatalogueItemSymbol containingSymbol,
        SourceNode declaration)
        : base(containingSymbol)
    {
        Id = (declaration as IIdentifiableNode)?.Id;
        Name = (declaration as INameableNode)?.Name ?? string.Empty;
        Comment = (declaration as CommentableNode)?.Comment;
    }

    public string? Id { get; }

    public sealed override string Name { get; }

    public sealed override string? Comment { get; }
}
