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

        this.FindControl<TreeView>("ExplorerTree").ItemTemplate = new FuncTreeDataTemplate(
            o => o is ExplorerNode,
            (o, _) =>
            {
                var control = new ExplorerNodeBaseControl(o);
                control.AttachedToLogicalTree += (_, _) =>
                {
                    if (control.Parent is not TreeViewItem p) return;
                    p.ContextMenu = new ContextMenu
                    {
                        Items = o switch
                        {
                            FileNode node => new MenuItem[]
                            {
                                new() { Header = "Open", Command = node.Open },
                                new() { Header = "Delete", Command = node.Delete }
                            },
                            DirectoryNode { IsEmpty: false } node => new MenuItem[]
                            {
                                new() { Header = "Expand", Command = node.Expand },
                                new() { Header = "Open", Command = node.Open }
                            },
                            DirectoryNode { IsEmpty: true } node => new MenuItem[]
                            {
                                new() { Header = "Open", Command = node.Open }
                            },
                            _ => throw new ArgumentOutOfRangeException(nameof(o), o, null)
                        }
                    };

                    p.DoubleTapped += (_, args) =>
                    {
                        switch (o)
                        {
                            case DirectoryNode { IsEmpty: false } node:
                                node.IsExpanded = !node.IsExpanded;
                                break;
                            case DirectoryNode { IsEmpty: true }: break;
                            case FileNode node:
                                var buffer = TabBuffer.Instance;
                                buffer.AddTabs(node.FullPath);
                                buffer.CurrentTab = buffer.Buffer.Items.Last();
                                break;
                            default: throw new ArgumentOutOfRangeException(nameof(o), o, null);
                        }

                        args.Handled = true;
                    };
                };

                return control;
            },
            o => (o as DirectoryNode)?.SubNodes
        );
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}