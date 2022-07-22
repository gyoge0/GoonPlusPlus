using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using GoonPlusPlus.Models.ExplorerTree;

namespace GoonPlusPlus.Controls;

public partial class FileExplorerView : UserControl
{
    public FileExplorerView()
    {
        InitializeComponent();

        var dataTemplates = this
            .FindControl<TreeView>("ExplorerTree")
            .DataTemplates;

        dataTemplates.Add(new FuncTreeDataTemplate(
            o => o.GetType() == typeof(DirectoryNode),
            (_, _) => new TextBlock { [!TextBlock.TextProperty] = new Binding("Name", BindingMode.OneWay) },
            o => ((DirectoryNode)o).SubNodes
        ));

        dataTemplates.Add(new FuncTreeDataTemplate(
            o => o.GetType() == typeof(FileNode),
            (_, _) => new TextBlock { [!TextBlock.TextProperty] = new Binding("Name", BindingMode.OneWay) },
            _ => null
        ));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}