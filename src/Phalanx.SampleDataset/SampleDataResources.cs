using WarHub.ArmouryModel.Workspaces.BattleScribe;

namespace Phalanx.SampleDataset;

public class SampleDataResources
{
    public static string[] GetDataResourceNames() =>
        typeof(SampleDataResources).Assembly.GetManifestResourceNames();

    public static Stream? OpenDataResource(string name) =>
        typeof(SampleDataResources).Assembly.GetManifestResourceStream(name);

    public static XmlDocument LoadXmlDocumentFromResource(string name)
    {
        using var stream = OpenDataResource(name);
        var fileInfo = stream!.LoadSourceAuto(name);
        return XmlDocument.Create(fileInfo);
    }

    public static XmlWorkspace CreateXmlWorkspace() =>
        XmlWorkspace.CreateFromDocuments(
            GetDataResourceNames()
            .Select(x => LoadXmlDocumentFromResource(x))
            .ToImmutableArray());
}
