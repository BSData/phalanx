namespace Phalanx.DataModel.Symbols.Binding;

public class BindingDiagnosticContext
{
    public void Add(string placeholder)
    {
        if (placeholder is null)
        {
            throw new ArgumentNullException(nameof(placeholder));
        }
        // TODO gather diagnostics
    }
}
