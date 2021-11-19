using System.Collections.Generic;
using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class ForceEntrySymbol : ContainerEntryBaseSymbol, IForceEntrySymbol
{
    public ForceEntrySymbol(
        ICatalogueItemSymbol containingSymbol,
        ForceEntryNode declaration,
        Binder binder,
        BindingDiagnosticContext diagnostics)
        : base(containingSymbol, declaration, binder, diagnostics)
    {
        Declaration = declaration;

        ChildForces = CreateChildEntries().ToImmutableArray();
        Categories = CreateCategories().ToImmutableArray();

        IEnumerable<IForceEntrySymbol> CreateChildEntries()
        {
            foreach (var item in declaration.ForceEntries)
            {
                yield return CreateEntry(this, item, binder, diagnostics);
            }
        }
        IEnumerable<ICategoryEntrySymbol> CreateCategories()
        {
            foreach (var item in declaration.CategoryLinks)
            {
                yield return CreateEntry(this, item, binder, diagnostics);
            }
        }
    }

    public ImmutableArray<IForceEntrySymbol> ChildForces { get; }

    public ImmutableArray<ICategoryEntrySymbol> Categories { get; }

    internal new ContainerEntryBaseNode Declaration { get; }
}
