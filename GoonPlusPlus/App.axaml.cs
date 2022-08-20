using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GoonPlusPlus.Workspace;

namespace GoonPlusPlus;

public class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel(desktop.Args) };
            desktop.Exit += (_, _) => WorkspaceViewModel.Instance.Workspace?.Save();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
