using FluentAssertions;
using Phalanx.SampleDataset;
using WarHub.ArmouryModel.Concrete;
using Xunit;

namespace WarHub.ArmouryModel;

public class SampleDatasetIntegrationTests
{
    [Fact]
    public void Dataset_compiles_and_binds()
    {
        var xmlWorkspace = SampleDataResources.CreateXmlWorkspace();
        var compilation = WhamCompilation.Create(
            xmlWorkspace.Documents.Select(x => SourceTree.CreateForRoot(x.GetRootAsync().Result!)).ToImmutableArray());
        var gamesystem = compilation.GlobalNamespace.RootCatalogue;
        var grannies = compilation.GlobalNamespace.Catalogues.First(x => x.Name.Contains("Grannies"));
        var roster1 = compilation.GlobalNamespace.Rosters.First(x => x.Name.Contains("Roster 1"));
        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        // asserts

        // first complete by getting diagnostics, make sure there are none
        compilation.GetDiagnostics(timeoutCts.Token).Should().BeEmpty();

        // catalogue's gamesystem is bound
        grannies.Gamesystem.Should().Be(gamesystem);

        // group's default selection is bound
        var gameSizeShared = gamesystem.SharedSelectionEntryContainers.First(x => x.Name == "Game Size");
        var gameSizeGroup = (ISelectionEntryGroupSymbol)gameSizeShared.ChildSelectionEntries.Single();
        gameSizeGroup.DefaultSelectionEntry.Should().Be(gameSizeGroup.ChildSelectionEntries.First(x => x.Name == "100 Points"));

        // root link's target is bound
        var gameSizeRoot = gamesystem.RootContainerEntries.First(x => x.Name == "Game Size" && x.IsReference);
        gameSizeRoot.ReferencedEntry.Should().Be(gameSizeShared);

        // roster
        // force's source entry is bound
        var convoyForce = roster1.Forces.Single();
        var convoyForceEntry = gamesystem.RootContainerEntries.First(x => x.Name == "Convoy");
        convoyForce.SourceEntry.Should().Be(convoyForceEntry);

        // force's rule is bound
        convoyForce.Resources.First(x => x.Name.Contains("Gold")).SourceEntry
            .Should().Be(grannies.RootResourceEntries.First(x => x.Name.Contains("Gold")));

        // force contains No Category
        convoyForce.Categories.Should().Contain(x => x.Name == compilation.NoCategoryEntrySymbol.Name && x.SourceEntry == compilation.NoCategoryEntrySymbol);

        // roster selection is fully bound
        var hnbSelection = convoyForce.Selections.First(x => x.Name == "Horse and Buggy");
        var hnbEntryLink = grannies.RootContainerEntries.First(x => x.Name == "Horse and Buggy");
        var hnbEntry = grannies.SharedSelectionEntryContainers.First(x => x.Name == "Horse and Buggy");
        // entry links is bound to entry
        hnbEntryLink.ReferencedEntry.Should().Be(hnbEntry);
        // source entry path is fully bound
        hnbSelection.SourceEntryPath.SourceEntries.Should().BeEquivalentTo(new[] { hnbEntryLink, hnbEntry });
        // source entry is bound
        hnbSelection.SourceEntry.Should().Be(hnbEntry);

        // selection's profile is bound
        var hnbProfileEntry = hnbEntry.Resources.First(x => x.Name == "Horse and Buggy" && x.ResourceKind == ResourceKind.Profile);
        hnbSelection.Resources.Single().SourceEntry.Should().Be(hnbProfileEntry);

        // nested selection is fully bound
        var driverGroup = hnbEntry.ChildSelectionEntries.First(x => x.Name.Contains("Driver"));
        var driverEntry = driverGroup.ChildSelectionEntries.First(x => x.Name == "Granny Trooper").ReferencedEntry;
        var driverSelection = hnbSelection.Selections.First(x => x.GetDeclaration()!.EntryGroupId!.Contains(driverGroup.Id!));
        driverSelection.SourceEntry.Should().Be(driverEntry);
    }
}
