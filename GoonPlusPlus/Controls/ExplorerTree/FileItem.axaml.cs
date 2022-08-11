using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GoonPlusPlus.Controls.ExplorerTree;

public partial class FileItem : UserControl
{
    public FileItem()
    {
        InitializeComponent();
        TextBlock = this.FindControl<TextBlock>("Block");
    }

    public TextBlock TextBlock { get; }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}