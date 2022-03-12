using System.Text.Json;
using System.Text.Json.Serialization;
using HandlebarsDotNet;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.EditorServices.Formatting;

public static class RosterFormatter
{
    /// <summary>
    /// Handlebars template can reference members of <see cref="RosterCore"/>
    /// by accessing the root context's "roster" property: <code>Name: {{roster.name}}</code>.
    /// </summary>
    /// <param name="roster"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string Format(RosterNode roster, RosterFormat format)
    {
        return format.Method switch
        {
            FormatMethod.Handlebars => GetHandlebars(),
            FormatMethod.Json => GetJson(),
            _ => throw new ArgumentException($"Unknown {nameof(FormatMethod)} value '{format.Method}'.", nameof(format)),
        };

        string GetHandlebars()
        {
            var templateBuilder = Handlebars.Compile(format.Template);
            var context = new
            {
                roster = roster.Core,
                // TODO consider more things, e.g. configuration/options of the template
            };
            return templateBuilder(context);
        }
        string GetJson()
        {
            return JsonSerializer.Serialize(roster.Core, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            });
        }
    }
}
