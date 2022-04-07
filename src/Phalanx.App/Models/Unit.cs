namespace Phalanx.App.Models;

public class Unit
{
    public int SubfactionId { get; set; }
    public int OrgSlotId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PointsPerModel { get; set; }
}
