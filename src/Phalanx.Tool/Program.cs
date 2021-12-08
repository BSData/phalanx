using Phalanx.Tool.Editor;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.SourceAnalysis;
using static WarHub.ArmouryModel.Source.NodeFactory;

namespace Phalanx.Tool;

class Program
{
    static void Main()
    {
        Console.WriteLine("Building dataset.");
        // create
        var dataset = GetDataset();
        Console.WriteLine("Compiling dataset.");
        var compilation = dataset.Compile();
        Console.WriteLine("Compiling dataset finished, creating global namespace.");
        var globalNamespace = compilation.GlobalNamespace;
        Console.WriteLine("Global namespace retrieved.");
        Console.WriteLine("Testing gamesystem binding in Catalogue symbol.");
        var gamesystem = globalNamespace.Catalogues.Where(x => !x.IsGamesystem).First().Gamesystem;
        Console.WriteLine("Catalogue symbol has bound to gamesystem symbol: " + gamesystem.Name);

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
        var gamesystem = Gamesystem("TestGst")
            .AddCostTypes(
                CostType("pts"))
            .AddCategoryEntries(teamsCategory)
            .AddForceEntries(
                ForceEntry("System Detachment")
                .AddCategoryLinks(
                    CategoryLink(teamsCategory)));
        var marineCatalogue = Catalogue(gamesystem, "Marine Corps")
            .AddForceEntries(
                ForceEntry("Marine Detachment")
                .AddCategoryLinks(
                    CategoryLink(teamsCategory)))
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
        var ctx = GamesystemContext.CreateSingle(gamesystem, marineCatalogue);
        return new(ctx);
    }
}
