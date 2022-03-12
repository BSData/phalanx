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
        var templateBuilder = Handlebars.Compile(format.HandlebarsTemplate);
        var context = new
        {
            roster = roster.Core,
            // TODO consider more things, e.g. configuration/options of the template
        };
        return templateBuilder(context);
    }
}
