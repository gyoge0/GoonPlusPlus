using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GoonPlusPlus.Controls.ExplorerTree;

public partial class FileItem : UserControl
{
    public TextBlock TextBlock { get; }

    public FileItem()
    {
        InitializeComponent();
        TextBlock = this.FindControl<TextBlock>("Block");
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}