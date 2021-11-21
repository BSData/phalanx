using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class ProfileSymbol : EntrySymbol, IProfileSymbol
{
    private readonly ProfileNode declaration;

    public ProfileSymbol(
        ICatalogueItemSymbol containingSymbol,
        ProfileNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        this.declaration = declaration;
        Type = null!; // TODO bind
        Characteristics = CreateCharacteristics().ToImmutableArray();

        IEnumerable<ICharacteristicSymbol> CreateCharacteristics()
        {
            foreach (var item in declaration.Characteristics)
            {
                yield return new CharacteristicSymbol(this, item, diagnostics);
            }
        }
    }

    public IProfileTypeSymbol Type { get; }

    public override SymbolKind Kind => SymbolKind.Resource;

    public ImmutableArray<ICharacteristicSymbol> Characteristics { get; }

    IResourceDefinitionSymbol? IResourceEntrySymbol.Type => Type;

    IResourceEntrySymbol? IResourceEntrySymbol.ReferencedEntry => null;
}
