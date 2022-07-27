using System;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using DynamicData;
using GoonPlusPlus.ViewModels;
using ReactiveUI;

namespace GoonPlusPlus.Controls;

public partial class CodeRunner : UserControl
{
    public CodeRunner()
    {
        InitializeComponent();
        var run = this.FindControl<TextBox>("Run");
        run.AddHandler(
            TextInputEvent,
            (sender, args) =>
            {
                if (args.Text == null) return;
                if (args.Text.Contains('\n'))
                {
                    var lines = args.Text.Split('\n');
                    lines.SkipLast(1).ToList().ForEach(l =>
                    {
                        RunViewModel.Instance.StdInBuilder.AppendLine(l);
                        RunViewModel.Instance.Enqueue();
                    });
                    RunViewModel.Instance.StdInBuilder.Append(lines.Last());
                }
                else RunViewModel.Instance.StdInBuilder.Append(args.Text);
            },
            RoutingStrategies.Tunnel
        );
        var stdout = RunViewModel.Instance.StdOut;
        var stderr = RunViewModel.Instance.StdErr;

        stdout
            .Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .OnItemAdded(_ =>
            {
                var lines = stdout.Items.ToList();
                stdout.Clear();
                lines.ForEach(l => run.Text += l + "\n");
            })
            .Subscribe();

        stderr
            .Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .OnItemAdded(_ =>
            {
                var lines = stderr.Items.ToList();
                stdout.Clear();
                lines.ForEach(l => run.Text += l + "\n");
            })
            .Subscribe();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Collapse_OnTapped(object? sender, RoutedEventArgs _)
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