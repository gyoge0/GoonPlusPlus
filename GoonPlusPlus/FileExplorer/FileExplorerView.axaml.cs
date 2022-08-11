using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using GoonPlusPlus.TabBar;
using System.Linq;

namespace GoonPlusPlus.FileExplorer;

public partial class FileExplorerView : UserControl
{
    public FileExplorerView()
    {
        InitializeComponent();

        var dataTemplates = this.FindControl<TreeView>("ExplorerTree").DataTemplates;

        dataTemplates.Add(new FuncTreeDataTemplate(o => o.GetType() == typeof(DirectoryNode),
            (o, _) =>
            {
                if (((DirectoryNode) o).SubNodes.Count > 0)
                {
                    var folderItem = new FolderItem();
                    folderItem.TextBlock.Bind(TextBlock.TextProperty, new Binding("Name", BindingMode.OneWay));
                    folderItem.DoubleTapped +=
                        (_, _) => ((DirectoryNode) o).IsExpanded = !((DirectoryNode) o).IsExpanded;
                    return folderItem;
                }
                else
                {
                    var folderItem = new EmptyFolderItem();
                    folderItem.TextBlock.Bind(TextBlock.TextProperty, new Binding("Name", BindingMode.OneWay));
                    return folderItem;
                }
            },
            o => ((DirectoryNode) o).SubNodes));

        dataTemplates.Add(new FuncTreeDataTemplate(o => o.GetType() == typeof(FileNode),
            (o, _) =>
            {
                var fileItem = new FileItem();
                fileItem.TextBlock.Bind(TextBlock.TextProperty, new Binding("Name", BindingMode.OneWay));
                fileItem.DoubleTapped += (_, _) =>
                {
                    TabBuffer.Instance.AddTabs(((FileNode) o).FullPath);
                    TabBuffer.Instance.CurrentTab = TabBuffer.Instance.Buffer.Items.Last();
                };
                return fileItem;
            },
            _ => null));
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
