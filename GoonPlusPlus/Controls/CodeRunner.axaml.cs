using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

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

    private void Collapse_OnTapped(object? sender, RoutedEventArgs e)
    {
        if (sender is not TabItem item) return;
        var open = !item.IsSelected;
        item.IsSelected = open;
        item.Parent!.FindDescendantOfType<TextBox>().IsVisible = open;
        var grid = item.FindAncestorOfType<Grid>();
        grid.RowDefinitions[2].Height = new GridLength(open ? 250 : 60);
        grid.RowDefinitions[2].MinHeight = open ? 120 : 60;
        (grid.Children[1] as GridSplitter)!.IsEnabled = open;
    }
}