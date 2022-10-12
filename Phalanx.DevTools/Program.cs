using System.Text.Json;
using Phalanx.SampleDataset;
using WarHub.ArmouryModel;
using WarHub.ArmouryModel.EditorServices;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.Source.BattleScribe;
using static WarHub.ArmouryModel.Source.NodeFactory;

namespace Phalanx.DevTools;

class Program
{
    static void Main()
    {
        Console.WriteLine("Loading sample dataset:");
        var ws = SampleDataResources.CreateXmlWorkspace();
        foreach (var doc in ws.Documents)
        {
            Console.WriteLine($"{doc.Filepath} ({doc.Kind})");
        }
        var state = RosterState.CreateFromNodes(ws.Documents.Select(x => x.GetRootAsync().Result!));
        Console.WriteLine("Verifying sample dataset compiles:");
        var sampleCompilation = state.Compilation;
        foreach (var diagnostic in sampleCompilation.GetDiagnostics())
        {
            Console.WriteLine(diagnostic.ToString());
        }
        // Compare existing CATs and determine what content is similar, and make a map from that.
        Console.WriteLine("Comparing Red and Green Vehicles:");

        var greenCat = state.Catalogues.First(cat => cat.Name == "Vehicles - Green");
        var redCat = state.Catalogues.First(cat => cat.Name == "Vehicles - Red");

        var linkNodeMap = new Dictionary<string, NodeMap>();

        foreach (var link in greenCat.EntryLinks)
        {
            if (link.Id is not null && link.Name is not null)
            {
                Console.WriteLine("Link to: {0} with ID: {1}", link.Name, link.Id);
                linkNodeMap.Add(link.Name, new NodeMap(greenCat, link.Id, link));
            }
        }
        foreach (var link in redCat.EntryLinks)
        {
            if (link.Id is not null && link.Name is not null)
            {
                Console.WriteLine("Link to: {0} with ID: {1}", link.Name, link.Id);
                var nodeExistsInTree = linkNodeMap.TryGetValue(link.Name, out var node);
                if (nodeExistsInTree && node is not null)
                {
                    node.AddInstance(redCat, link.Id);
                }
            }
        }
        Console.WriteLine("Nodes in cat:");
        foreach (var node in linkNodeMap)
        {
            Console.WriteLine("> {0}", node.Key);
            foreach (var instance in node.Value.idInEachCat)
            {
                Console.WriteLine("> > Cat: {0} Id: {1}", instance.Key, instance.Value);
            }
        }

        // Make a copy of a CAT with new IDs.

        // Apply generated map file (or known good map file) to new (or existing) cat file.
    }
}
