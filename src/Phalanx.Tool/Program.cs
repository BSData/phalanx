using Phalanx.Tool.Editor;
using WarHub.ArmouryModel.Source;
using static WarHub.ArmouryModel.Source.NodeFactory;

namespace Phalanx.Tool;

class Program
{
    static void Main()
    {
        Console.WriteLine(">>> Building dataset.");
        // create
        var dataset = GetDataset();
        // roster modifications
        var printer = new RosterPrinter();
        var rosterEditor = RosterEditor.Create(dataset).WithName("Test Marine Strike Force");
        Console.WriteLine(">>> New roster created:");
        PrintRoster();
        // change cost limit to 1000 pts
        ChangeAndPrint("Point cost changed:",
            change: x => x.ChangeCostLimit(x.Roster.CostLimits[0]).To(1000));
        // add system force
        ChangeAndPrint("System force added:",
            change: x => x.AddForce(x.Gamesystem.ForceEntries[0]).ToRoot());
        // add selection to system force
        ChangeAndPrint("Selection added to system force:",
            change: x => x.AddSelection(x.Catalogues[0].SelectionEntries[0]).To(x.Roster.Forces[0]));
        // change count of first selection to 5
        ChangeAndPrint("Change selection count to 5:",
            change: x => x.ChangeCountOf(x.Roster.Forces[0].Selections[0]).To(5));
        // add second selection to system force
        ChangeAndPrint("Selection added to system force:",
            change: x => x.AddSelection(x.Catalogues[0].SelectionEntries[0]).To(x.Roster.Forces[0]));
        // remove second selection from system force
        ChangeAndPrint("Selection removed from system force:",
            change: x => x.RemoveSelection(x.Roster.Forces[0].Selections[1]));
        // add marine force
        ChangeAndPrint("Marine force added:",
            change: x => x.AddForce(x.Catalogues[0].ForceEntries[0]).ToRoot());
        // add selection to marine force
        ChangeAndPrint("Selection added to Marine force:",
            change: x => x.AddSelection(x.Catalogues[0].SelectionEntries[1]).To(x.Roster.Forces[1]));
        // remove marine force
        ChangeAndPrint("System force removed:",
            change: x => x.RemoveForce(x.Roster.Forces[0]));
        // done
        Console.WriteLine(">>> Finished.");

        void ChangeAndPrint(string documentationText, Func<RosterEditor, IRosterOperation> change)
        {
            Change(change);
            Console.WriteLine($">>>>>>>>>> {documentationText} <<<<<<<<<<");
            Console.WriteLine();
            PrintRoster();
            Console.WriteLine();
        }

        void PrintRoster() => printer.Visit(rosterEditor.Roster);

        void Change(Func<RosterEditor, IRosterOperation> change) =>
            rosterEditor = rosterEditor.Apply(change);
    }

    static Dataset GetDataset()
    {
        var teamsCategory = CategoryEntry("team");
        var pub1 = Publication("pub1");
        var gamesystem = Gamesystem("TestGst")
            .AddPublications(pub1)
            .AddProfileTypes(
                ProfileType("Unit stats")
                .AddCharacteristicTypes(
                    CharacteristicType("Move")))
            .AddCostTypes(
                CostType("pts"))
            .AddCategoryEntries(teamsCategory)
            .AddForceEntries(
                ForceEntry("System Detachment")
                .WithPublicationId(pub1.Id)
                .AddCategoryLinks(
                    CategoryLink(teamsCategory)));
        var dumbCatalogue = Catalogue(gamesystem, "Import me!");
        var marineCatalogue = Catalogue(gamesystem, "Marine Corps")
            .AddCatalogueLinks(
                CatalogueLink(dumbCatalogue))
            .AddForceEntries(
                ForceEntry("Marine Detachment")
                .AddCategoryLinks(
                    CategoryLink(teamsCategory)))
            .AddSharedProfiles(
                Profile(gamesystem.ProfileTypes[0])
                .AddCharacteristics(
                    Characteristic(gamesystem.ProfileTypes[0].CharacteristicTypes[0], "10cm")))
            .AddSelectionEntries(
                SelectionEntry("Drone")
                .AddCosts(
                    Cost(gamesystem.CostTypes[0], 15)),
                SelectionEntry("Basic Team")
                .AddSelectionEntries(
                    SelectionEntry("Marine Guy")
                    .AddCosts(
                        Cost(gamesystem.CostTypes[0], 10))
                    .AddConstraints(
                        Constraint(type: ConstraintKind.Minimum, value: 5),
                        Constraint(type: ConstraintKind.Maximum, value: 10))));

        return new(ImmutableArray.Create<CatalogueBaseNode>(gamesystem, marineCatalogue, dumbCatalogue));
    }
}
