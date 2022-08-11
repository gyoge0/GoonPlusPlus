using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GoonPlusPlus.Workspace;

public partial class WorkspaceEditor : Window
{
    public WorkspaceEditor()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
