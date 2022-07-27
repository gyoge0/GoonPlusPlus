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
        if (sender is not TabItem { Content: TextBox box } item) return;
        var open = !box.IsVisible;
        box.IsVisible = open;
        var grid = item.FindAncestorOfType<Grid>();
        grid.RowDefinitions[2].Height = new GridLength(open ? 250 : 60);
        grid.RowDefinitions[2].MinHeight = open ? 120 : 60;
        (grid.Children[1] as GridSplitter)!.IsEnabled = open;
    }

    private void TabControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs _)
    {
        if (sender is not TabControl { SelectedContent: TextBox box } control) return;
        box.IsVisible = true;
        var grid = control.FindAncestorOfType<Grid>();
        if (grid == null) return;
        grid.RowDefinitions[2].Height = new GridLength(250);
        grid.RowDefinitions[2].MinHeight = 120;
        (grid.Children[1] as GridSplitter)!.IsEnabled = true;
    }
}