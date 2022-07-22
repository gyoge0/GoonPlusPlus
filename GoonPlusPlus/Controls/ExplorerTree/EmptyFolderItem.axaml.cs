using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;

namespace GoonPlusPlus.Controls.ExplorerTree;

public partial class EmptyFolderItem : UserControl
{
    public TextBlock TextBlock { get; }

    public EmptyFolderItem()
    {
        InitializeComponent();
        TextBlock = this.FindControl<TextBlock>("Block");
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}