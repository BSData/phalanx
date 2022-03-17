using Microsoft.Extensions.Options;
using WarHub.ArmouryModel.EditorServices.Formatting;

namespace Phalanx.App.Pages.Printing;

public class RosterFormatsProvider
{
    private readonly Options options;

    public RosterFormatsProvider(IOptions<Options> options)
    {
        this.options = options.Value;
    }

    public IEnumerable<RosterFormat> Formats => options.Formats;

    public class Options
    {
        public List<RosterFormat> Formats { get; } = RosterFormatter.BuiltinFormatters.ToList();
    }
}
