using System;
using System.Collections.Immutable;
using Phalanx.Tool.Editor;
using WarHub.ArmouryModel.Source;
using static WarHub.ArmouryModel.Source.NodeFactory;

namespace Phalanx.Tool
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");
            var printer = new RosterPrinter();
            // create
            var rosterEditor = RosterEditor.Create(GetDataset()).WithName("Test Marine Strike Force");
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
            // add marine force
            ChangeAndPrint("Marine force added:",
                change: x => x.AddForce(x.Catalogues[0].ForceEntries[0]).ToRoot());
            // add selection to marine force
            ChangeAndPrint("Selection added to Marine force:",
                change: x => x.AddSelection(x.Catalogues[0].SelectionEntries[0]).To(x.Roster.Forces[1]));
            // done
            Console.WriteLine(">>> Finished.");

            void ChangeAndPrint(string documentationText, Func<RosterEditor, IRosterOperation> change)
            {
                Change(change);
                Console.WriteLine($">>>>>>>>>> {documentationText} <<<<<<<<<<");
                PrintRoster();
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
                    SelectionEntry("Basic Team")
                    .AddCosts(
                        Cost(gamesystem.CostTypes[0], 10)));
            return new(gamesystem, ImmutableArray.Create(marineCatalogue));
        }
    }
}
