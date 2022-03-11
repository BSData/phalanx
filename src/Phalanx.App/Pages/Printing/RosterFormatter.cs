using HandlebarsDotNet;
using Microsoft.Extensions.Options;
using WarHub.ArmouryModel.Source;

namespace Phalanx.App.Pages.Printing;

public class RosterFormatter
{
    private readonly Options options;

    public RosterFormatter(IOptions<Options> options)
    {
        this.options = options.Value;
    }

    /// <summary>
    /// Handlebars template can reference members of <see cref="RosterCore"/>
    /// by accessing the root context's "roster" property: <code>Name: {{roster.name}}</code>.
    /// </summary>
    /// <param name="roster"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public string Format(RosterNode roster, RosterFormat format)
    {
        var templateBuilder = Handlebars.Compile(format.HandlebarsTemplate);
        var context = new
        {
            roster = roster.Core
        };
        return templateBuilder(context);
    }

    public IEnumerable<RosterFormat> Formats => options.Formats;

    public class Options
    {
        public List<RosterFormat> Formats { get; } = new()
        {
            new()
            {
                Name = "Default",
                HandlebarsTemplate = "Roster \"{{roster.name}}\""
            }
        };
    }
}
