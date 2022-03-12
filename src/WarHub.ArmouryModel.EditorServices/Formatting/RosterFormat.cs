namespace WarHub.ArmouryModel.EditorServices.Formatting;

public record RosterFormat
{
    public string? Name { get; init; }

    public OutputFormat OutputFormat { get; init; } = OutputFormat.PlainText;

    public string? HandlebarsTemplate { get; init; }
}
