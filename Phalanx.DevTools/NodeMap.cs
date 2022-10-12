using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandlebarsDotNet.Collections;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DevTools;
public class NodeMap
{
    public Dictionary<string, string> idInEachCat = new();
    public SourceNode nodeToCopy;

   public NodeMap(CatalogueNode cat, string id, SourceNode nodeToCopy)
    {
        this.nodeToCopy = nodeToCopy;
        AddInstance(cat, id);
    }
    public void AddInstance(CatalogueNode cat, string id)
    {
        if (cat.Id != null)
        {
            idInEachCat.Add(cat.Id, id);

        }
    }
     
}
