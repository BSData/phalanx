using System.Collections.Generic;
using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class CatalogueSymbol : CatalogueBaseSymbol
{
    public CatalogueSymbol(
        IDatasetSymbol containingSymbol,
        CatalogueNode declaration,
        Binder binder,
        BindingDiagnosticContext diagnostics)
        : base(containingSymbol, declaration, binder, diagnostics)
    {
        Declaration = declaration;
        Gamesystem = null!; // TODO bind
        Imports = CreateLinks().ToImmutableArray();

        IEnumerable<ICatalogueReferenceSymbol> CreateLinks()
        {
            foreach (var item in declaration.CatalogueLinks)
            {
                yield return new CatalogueReferenceSymbol(this, item, binder, diagnostics);
            }
        }
    }

    public override bool IsLibrary => Declaration.IsLibrary;

    public override bool IsGamesystem => false;

    public override ICatalogueSymbol Gamesystem { get; }

    public override ImmutableArray<ICatalogueReferenceSymbol> Imports { get; }

    internal CatalogueNode Declaration { get; }
}
