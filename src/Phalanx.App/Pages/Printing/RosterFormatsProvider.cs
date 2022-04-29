using Microsoft.Extensions.Options;
using WarHub.ArmouryModel.EditorServices.Formatting;

namespace Phalanx.App.Pages.Printing;

public class RosterFormatsProvider
{
    public RosterFormatsProvider(IOptions<Options> options)
    {
        Formats = options.Value.Formats.ToImmutableArray();
    }

    public ImmutableArray<RosterFormat> Formats { get; }

    public class Options
    {
        public List<RosterFormat> Formats { get; } = RosterFormatter.BuiltinFormatters.ToList();
    }
}
