namespace WarHub.ArmouryModel;

public abstract record CompilationOptions
{
    /// <summary>
    /// When <see langword="true"/>, allows binder to search for entry reference targets
    /// in nested entries - entries deeper than only shared/root level.
    /// Defaults to <see langword="true"/>.
    /// </summary>
    public bool BindEntryReferencesToNestedEntries { get; init; } = true;
}
