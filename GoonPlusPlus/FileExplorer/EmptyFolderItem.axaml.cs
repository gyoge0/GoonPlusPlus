using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GoonPlusPlus.FileExplorer;

public partial class EmptyFolderItem : UserControl
{
    public EmptyFolderItem()
    {
        InitializeComponent();
        TextBlock = this.FindControl<TextBlock>("Block");
    }

    public TextBlock TextBlock { get; }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
