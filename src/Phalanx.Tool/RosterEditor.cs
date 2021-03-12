using System.Linq;
using WarHub.ArmouryModel.Source;
using static WarHub.ArmouryModel.Source.NodeFactory;

namespace Phalanx.Tool
{
    public record RosterEditor(GamesystemNode Gamesystem, CatalogueNode Catalogue, RosterNode Roster)
    {
        public ForceAddEditor AddForce(ForceEntryNode forceEntry) => new(this, forceEntry);
        public SelectionAddEditor AddSelection(SelectionEntryNode selectionEntry) => new(this, selectionEntry);


        public record ForceAddEditor(RosterEditor Root, ForceEntryNode ForceEntry)
        {
            public RosterEditor ToRoot()
            {
                return Root with
                {
                    Roster = Root.Roster.AddForces(Force(ForceEntry))
                };
            }
        }

        public record SelectionAddEditor(RosterEditor Root, SelectionEntryNode SelectionEntry)
        {
            public RosterEditor To(ForceNode force)
            {
                var selection =
                    Selection(SelectionEntry, SelectionEntry.Id)
                    .AddCosts(SelectionEntry.Costs);
                var forces = Root.Roster.Forces.NodeList.ToList();
                forces[forces.IndexOf(force)] = force.AddSelections(selection);
                return Root with
                {
                    Roster = Root.Roster.WithForces(forces.ToNodeList())
                };
            }
        }
    }
}
