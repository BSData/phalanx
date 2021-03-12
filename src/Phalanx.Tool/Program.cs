using System;
using WarHub.ArmouryModel.Source;
using static WarHub.ArmouryModel.Source.NodeFactory;

namespace Phalanx.Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var printer = new RosterPrinter();
            var (gamesystem, marineCatalogue) = GetDataset();
            var roster = Roster(gamesystem, "Test Marine Strike Force");
            var rosterEditor = new RosterEditor(gamesystem, marineCatalogue, roster);
            Console.WriteLine(">>> New roster created:");
            PrintRoster();
            rosterEditor = rosterEditor.AddForce(gamesystem.ForceEntries[0]).ToRoot();
            Console.WriteLine(">>> Force added:");
            PrintRoster();
            rosterEditor = rosterEditor.AddSelection(marineCatalogue.SelectionEntries[0]).To(rosterEditor.Roster.Forces[0]);
            Console.WriteLine(">>> Selection added:");
            PrintRoster();
            Console.WriteLine(">>> Finished.");

            void PrintRoster() => printer.Visit(rosterEditor.Roster);
        }

        static (GamesystemNode, CatalogueNode) GetDataset()
        {
            var teamsCategory = CategoryEntry("team");
            var gamesystem = Gamesystem("test")
                .AddCostTypes(
                    CostType("pts"))
                .AddCategoryEntries(teamsCategory)
                .AddForceEntries(
                    ForceEntry("detachment")
                    .AddCategoryLinks(
                        CategoryLink(teamsCategory)));
            var marineCatalogue = Catalogue(gamesystem, "Marine Corps")
                .AddSelectionEntries(
                    SelectionEntry("Basic Team")
                    .AddCosts(
                        Cost(gamesystem.CostTypes[0], 10)));
            return (gamesystem, marineCatalogue);
        }
    }
}
