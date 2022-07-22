using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GoonPlusPlus.Controls.ExplorerTree;

public partial class FolderItem : UserControl
{
    public TextBlock TextBlock { get; }

    public FolderItem()
    {
        InitializeComponent();
        TextBlock = this.FindControl<TextBlock>("Block");
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}