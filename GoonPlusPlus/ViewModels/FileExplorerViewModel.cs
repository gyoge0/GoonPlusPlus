using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData.Binding;
using GoonPlusPlus.Models;
using ReactiveUI;
using Path = System.IO.Path;

namespace GoonPlusPlus.ViewModels;

public class FileExplorerViewModel
{
    private static string HomeFolder { get; } = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    public ObservableCollection<ExplorerNode> SelectedItems { get; } = new();

    // ReSharper disable once CollectionNeverQueried.Global
    public ObservableCollection<DirectoryNode> Root { get; } = new()
    {
        new DirectoryNode(HomeFolder)
        {
            IsExpanded = true
        }
    };

    public FileExplorerViewModel()
    {
        TabBuffer.Instance
            .WhenPropertyChanged(x => x.CurrentTab)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(x =>
            {
                if (x.Value == null || x.Value.IsUntitled) return;
                Root[0] = new DirectoryNode(Path.GetDirectoryName(x.Value.Path!) ?? HomeFolder) { IsExpanded = true };
            });
    }
}