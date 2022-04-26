using System.Text.Json;
using System.Text.Json.Serialization;

namespace WarHub.ArmouryModel.EditorServices.Formatting;

internal static class FormatManifestResources
{
    private const string FormatJsonSuffix = ".format.json";

    public static IEnumerable<string> GetAllTemplatesResourceNames() =>
        typeof(FormatManifestResources).Assembly.GetManifestResourceNames()
        .Where(x => x.StartsWith("Templates.", StringComparison.Ordinal));

    public static IEnumerable<string> GetFormatJsonResourceNames() =>
        GetAllTemplatesResourceNames()
        .Where(x => x.EndsWith(FormatJsonSuffix, StringComparison.Ordinal));

    public static Stream? OpenDataResource(string name) =>
        typeof(FormatManifestResources).Assembly.GetManifestResourceStream(name);

    public static string? TryGetResourceAsString(string name)
    {
        using var stream = OpenDataResource(name);
        if (stream is null)
        {
            return null;
        }
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static T? DeserializeResource<T>(string name)
    {
        using var stream = OpenDataResource(name)
            ?? throw new InvalidOperationException("Resource not found: " + name);
        return JsonSerializer.Deserialize<T>(stream, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        });
    }

    public static RosterFormat LoadFormatDefinition(string formatJsonName)
    {
        var format = DeserializeResource<RosterFormat>(formatJsonName);
        if (format is null)
        {
            throw new InvalidOperationException("Format failed to deserialize.");
        }

        var resourceName = formatJsonName.Replace(FormatJsonSuffix, ".handlebars", StringComparison.Ordinal);
        if (format is { Method: FormatMethod.Handlebars, Template: null }
            && TryGetResourceAsString(resourceName) is { } template)
        {
            format = format with
            {
                Template = template,
            };
        }
        return format;
    }
}
