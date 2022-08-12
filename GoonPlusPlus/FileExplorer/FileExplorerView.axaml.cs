using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using GoonPlusPlus.TabBar;
using System;
using System.Linq;

namespace GoonPlusPlus.FileExplorer;

public partial class FileExplorerView : UserControl
{
    public FileExplorerView()
    {
        InitializeComponent();

        this.FindControl<TreeView>("ExplorerTree").ItemTemplate = new FuncTreeDataTemplate(o => o is ExplorerNode,
            (o, _) =>
            {
                var control = new ExplorerNodeBaseControl(o);
                
                control.DoubleTapped += o switch
                {
                    DirectoryNode node when node.SubNodes.Count > 1 => (_, _) => node.IsExpanded = true,
                    FileNode => (_, _) =>
                    {
                        var buffer = TabBuffer.Instance;
                        buffer.AddTabs(((FileNode) o).FullPath);
                        buffer.CurrentTab = buffer.Buffer.Items.Last();
                    },
                    DirectoryNode => (_, _) => { },
                    _ => throw new ArgumentOutOfRangeException(nameof(o), o, null),
                };

                return control;
            },
            o => (o as DirectoryNode)?.SubNodes);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
