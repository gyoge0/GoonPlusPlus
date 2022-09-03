using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using System;

namespace GoonPlusPlus.FileExplorer;

public class ExplorerNodeBaseControl : UserControl
{
    public ExplorerNodeBaseControl(object o)
    {
        DataContext = o;
        Content = new StackPanel
        {
            Spacing = 10,
            Orientation = Orientation.Horizontal,
            Children =
            {
                new PathIcon { Data = Icon },
                new TextBlock { [!TextBlock.TextProperty] = new Binding("Name", BindingMode.OneWay) },
            },
        };
    }


    private Geometry Icon => (Application.Current!.FindResource(
        DataContext switch
        {
            DirectoryNode { IsEmpty: false } => "FolderOpenRegular",
            DirectoryNode { IsEmpty: true } => "FolderRegular",
            FileNode => "DocumentRegular",
            _ => throw new ArgumentOutOfRangeException(),
        }
    ) as Geometry)!;
}