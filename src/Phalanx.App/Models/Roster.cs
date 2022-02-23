namespace Phalanx.App.Models;

public class Roster
{
    public int Id { get; set; }
    public int FactionId { get; set; }
    public int SubfactionId { get; set; }
    public int GameSystemId { get; set; }
    public string? Name { get; set; }
    public string? Notes { get; set; }
    public int Points { get; set; }
    public int Powerlevel { get; set; }
    public List<Unit> Units { get; set; } = new List<Unit>();
}
