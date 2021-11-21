namespace Phalanx.DataModel;

public class DiagnosticBag
{
    public static DiagnosticBag GetInstance()
    {
        return new();
    }

    internal void Add(string v)
    {
        throw new NotImplementedException();
    }
}
