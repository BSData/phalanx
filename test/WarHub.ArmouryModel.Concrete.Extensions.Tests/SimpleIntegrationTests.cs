using FluentAssertions;
using WarHub.ArmouryModel.Concrete;
using WarHub.ArmouryModel.Source;
using Xunit;

namespace WarHub.ArmouryModel;

public class SimpleIntegrationTests
{
    [Fact]
    public void EmptyCatalogueAndGamesystem_compile()
    {
        // arrange
        var gst = NodeFactory.Gamesystem("foo");
        var cat = NodeFactory.Catalogue(gst, "bar");
        var compilation = WhamCompilation.Create();
        // act
        var result = compilation.AddSourceTrees(SourceTree.CreateForRoot(gst), SourceTree.CreateForRoot(cat));
        // assert
        result.GetDiagnostics().Should().BeEmpty();
    }

    [Fact]
    public void Compilation_connects_root_entry_link_to_shared_entry()
    {
        // arrange
        var gst = NodeFactory.Gamesystem("foo");
        var cat = NodeFactory.Catalogue(gst, "bar")
            .AddSharedSelectionEntries(
                NodeFactory.SelectionEntry("entry").Tee(out var entry))
            .AddEntryLinks(
                NodeFactory.EntryLink(entry));
        var compilation = WhamCompilation.Create();
        // act
        var result = compilation.AddSourceTrees(SourceTree.CreateForRoot(gst), SourceTree.CreateForRoot(cat));
        // assert
        result.GetDiagnostics().Should().BeEmpty();
        var catalogue = result.GlobalNamespace.Catalogues.Single(x => !x.IsGamesystem);
        catalogue.RootContainerEntries.Single().ReferencedEntry
            .Should().Be(catalogue.SharedSelectionEntryContainers.Single());
    }

    [Fact]
    public void Given_bad_root_entry_link_When_GetDiagnostics_Then_returns_error()
    {
        // arrange
        var invalidEntry = NodeFactory.SelectionEntry("entry");
        var gst = NodeFactory.Gamesystem("foo");
        var cat = NodeFactory.Catalogue(gst, "bar")
            .AddEntryLinks(
                NodeFactory.EntryLink(invalidEntry));
        var compilation = WhamCompilation.Create();
        // act
        var result = compilation.AddSourceTrees(SourceTree.CreateForRoot(gst), SourceTree.CreateForRoot(cat));
        // assert
        result.GetDiagnostics().Should().ContainSingle()
            .Which.Should().Match<Diagnostic>(x =>
                x.Id == "WHAM0006"
                && x.GetMessage(null) == "ERR_NoBindingCandidates"
                && x.Severity == DiagnosticSeverity.Error);
        var catalogue = result.GlobalNamespace.Catalogues.Single(x => !x.IsGamesystem);
        catalogue.RootContainerEntries.Single().ReferencedEntry
            .Should().BeAssignableTo<IErrorSymbol>();
    }
}
