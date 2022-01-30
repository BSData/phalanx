using WarHub.ArmouryModel;
using WarHub.ArmouryModel.EditorServices;
using WarHub.ArmouryModel.Source;
using static WarHub.ArmouryModel.Source.NodeFactory;

namespace Phalanx.PlaygroundTool;

class Program
{
    static void Main()
    {
        Console.WriteLine(">>> Building dataset.");
        // create
        var rosterState = RosterState.CreateFromNodes(GetDataset());
        Console.WriteLine(">>> Compilation created, creating global namespace.");
        var globalNamespace = rosterState.Compilation.GlobalNamespace;
        Console.WriteLine(">>> Global namespace retrieved.");

        Console.WriteLine(">>> Testing gamesystem binding in Catalogue symbol.");
        var gamesystem = globalNamespace.Catalogues.Where(x => !x.IsGamesystem).First().Gamesystem;
        Console.WriteLine(">>> Catalogue symbol has bound to gamesystem symbol: " + gamesystem.Name);

        Console.WriteLine(">>> Testing publication binding.");
        var forceEntry1 = globalNamespace.RootCatalogue.AllItems.OfType<IForceEntrySymbol>().First();
        Console.WriteLine($">>> Force '{forceEntry1.Name}' publication bound to '{forceEntry1.PublicationReference?.Publication.Name}'");

        Console.WriteLine(">>> Testing diagnostic listing.");
        var diagnostics = rosterState.Compilation.GetDiagnostics();
        foreach (var diag in diagnostics)
            Console.WriteLine(diag.ToString());
        Console.WriteLine($">>> Diagnostics printed (count: {diagnostics.Length}).");

        // roster modifications
        var printer = new RosterPrinter();
        var rosterEditor = new RosterEditor(rosterState);
        rosterEditor.AddOperation(RosterOperations.CreateRoster() with { Name = "Test Marine Strike Force" });
        Console.WriteLine(">>> New roster created:");
        PrintRoster();
        // change cost limit to 1000 pts
        ChangeAndPrint("Point cost changed:",
            change: x => RosterOperations.ChangeCostLimit(x.Roster.CostLimits[0], 1000));
        // add system force
        ChangeAndPrint("System force added:",
            change: x => RosterOperations.AddForce(x.Gamesystem.ForceEntries[0]));
        // add selection to system force
        ChangeAndPrint("Selection added to system force:",
            change: x => RosterOperations.AddSelection(x.Catalogues[0].SelectionEntries[0], x.Roster.Forces[0]));
        // change count of first selection to 5
        ChangeAndPrint("Change selection count to 5:",
            change: x => RosterOperations.ChangeCountOf(x.Roster.Forces[0].Selections[0], 5));
        // add second selection to system force
        ChangeAndPrint("Selection added to system force:",
            change: x => RosterOperations.AddSelection(x.Catalogues[0].SelectionEntries[0], x.Roster.Forces[0]));
        // remove second selection from system force
        ChangeAndPrint("Selection removed from system force:",
            change: x => RosterOperations.RemoveSelection(x.Roster.Forces[0].Selections[1]));
        // add marine force
        ChangeAndPrint("Marine force added:",
            change: x => RosterOperations.AddForce(x.Catalogues[0].ForceEntries[0]));
        // add selection to marine force
        ChangeAndPrint("Selection added to Marine force:",
            change: x => RosterOperations.AddSelection(x.Catalogues[0].SelectionEntries[1], x.Roster.Forces[1]));
        // remove marine force
        ChangeAndPrint("System force removed:",
            change: x => RosterOperations.RemoveForce(x.Roster.Forces[0]));
        // done
        Console.WriteLine(">>> Finished.");

        void ChangeAndPrint(string documentationText, Func<StateContainer, IRosterOperation> change)
        {
            Console.WriteLine($">>>>>>>>>> {documentationText} <<<<<<<<<<");
            Console.WriteLine();
            rosterEditor.AddOperation(change(new(rosterEditor.State)));
            PrintRoster();
            Console.WriteLine();
        }

        void PrintRoster() => printer.Visit(rosterEditor.State.RosterRequired);
    }

    private record StateContainer(RosterState State)
    {
        public GamesystemNode Gamesystem => State.Gamesystem;
        public ImmutableArray<CatalogueNode> Catalogues => State.Catalogues;
        public RosterNode Roster => State.RosterRequired;
    };

    static IEnumerable<SourceNode> GetDataset()
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

        return ImmutableArray.Create<CatalogueBaseNode>(gamesystem, marineCatalogue, dumbCatalogue);
    }
}
