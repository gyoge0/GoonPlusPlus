using Avalonia.Logging;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;

namespace GoonPlusPlus.FileExplorer;

public class DirectoryNode : ExplorerNode
{
    private bool _isExpanded;

    public DirectoryNode(string strFullPath) : base(strFullPath)
    {
        if (!IsEmpty) SubNodes.Add(new FileNode("{{ Dummy Node }}"));
        Expand = ReactiveCommand.Create(() => { IsExpanded = true; });
        Open = ReactiveCommand.Create(() => FileExplorerViewModel.OpenFolder(this));
    }

    public ObservableCollection<ExplorerNode> SubNodes { get; } = new();

    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            SubNodes.Clear();
            if (this.RaiseAndSetIfChanged(ref _isExpanded, value)) GetSubfolders();
            else if (IsEmpty) SubNodes.Add(new FileNode("{{ Dummy Node }}"));
        }
    }

    public bool IsEmpty => Directory.GetFiles(FullPath).Length > 0;

    public ReactiveCommand<Unit, Unit> Expand { get; }
    public ReactiveCommand<Unit, Unit> Open { get; }

    private void GetSubfolders()
    {
        foreach (var path in Directory.GetDirectories(FullPath, "*", SearchOption.TopDirectoryOnly))
            try
            {
                SubNodes.Add(new DirectoryNode(path));
            }
            catch (UnauthorizedAccessException)
            {
                Logger.TryGet(LogEventLevel.Warning, LogArea.Binding)?.Log(this, $"Access denied to directory ${path}");
            }

        foreach (var path in Directory.GetFiles(FullPath, "*", SearchOption.TopDirectoryOnly))
            try
            {
                SubNodes.Add(new FileNode(path));
            }
            catch (UnauthorizedAccessException)
            {
                Logger.TryGet(LogEventLevel.Warning, LogArea.Binding)?.Log(this, $"Access denied to file ${path}");
            }
    }
}