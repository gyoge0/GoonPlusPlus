using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GoonPlusPlus.Controls;

public partial class AcrylicBlurView : UserControl
{
    public AcrylicBlurView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}