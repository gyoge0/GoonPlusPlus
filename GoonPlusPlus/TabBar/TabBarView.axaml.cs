using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GoonPlusPlus.TabBar;

public partial class TabBarView : UserControl
{
    public TabBarView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
