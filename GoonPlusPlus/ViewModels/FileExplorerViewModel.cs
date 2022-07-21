using System;
using System.Collections.ObjectModel;
using GoonPlusPlus.Models;

namespace GoonPlusPlus.ViewModels;

public class FileExplorerViewModel
{
    public ObservableCollection<ExplorerNode> SelectedItems { get; } = new();

    public ObservableCollection<DirectoryNode> Root { get; } = new()
    {
        new DirectoryNode(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
        {
            IsExpanded = true
        }
    };
}