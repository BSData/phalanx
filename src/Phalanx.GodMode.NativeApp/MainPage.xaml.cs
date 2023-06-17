namespace Phalanx.GodMode.NativeApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
        Window.Title = "GodMode";
    }
}
