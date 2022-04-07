using WarHub.ArmouryModel.Source;

namespace Phalanx.App;

class CataloguePrinter : SourceWalker
{
    private int Depth { get; set; } = 0;


    public override void DefaultVisit(SourceNode node){
        
        
        if(node is EntryBaseNode)
            Print($"- \"{((EntryBaseNode)node).Name}\" ID= {((EntryBaseNode)node).Id} ({node.GetType().Name})");
        
        else
            Print($"- {node.GetType().Name}");

        Depth++;
        base.DefaultVisit(node);
        Depth--;

    }

    private void Print(string line)
    {
        for (var i = 0; i < Depth; i++)
            Console.Write("  ");
        Console.WriteLine(line);
    }
}
