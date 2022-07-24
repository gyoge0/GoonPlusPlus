using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GoonPlusPlus.Controls;

public partial class CodeRunner : UserControl
{
    public CodeRunner()
    {
        InitializeComponent();
        this.FindControl<TabItem>("Compile");
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}