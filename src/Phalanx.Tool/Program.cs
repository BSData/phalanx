using System;
using System.Collections.Immutable;
using Phalanx.Tool.Editor;
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
            var rosterEditor = new RosterEditor(new RosterState(GetDataset()));
            // create
            Change(x => x.Create().WithName("Test Marine Strike Force"));
            Console.WriteLine(">>> New roster created:");
            PrintRoster();
            // add system force
            Change(x => x.AddForce(x.Gamesystem.ForceEntries[0]).ToRoot());
            Console.WriteLine(">>> System force added:");
            PrintRoster();
            // add selection to system force
            Change(x => x.AddSelection(x.Catalogues[0].SelectionEntries[0]).To(x.Roster.Forces[0]));
            Console.WriteLine(">>> Selection added to system force:");
            PrintRoster();
            // add marine force
            Change(x => x.AddForce(x.Catalogues[0].ForceEntries[0]).ToRoot());
            Console.WriteLine(">>> Marine force added:");
            PrintRoster();
            // add selection to marine force
            Change(x => x.AddSelection(x.Catalogues[0].SelectionEntries[0]).To(x.Roster.Forces[1]));
            Console.WriteLine(">>> Selection added to Marine force:");
            PrintRoster();
            // done
            Console.WriteLine(">>> Finished.");

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
