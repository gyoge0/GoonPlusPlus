﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using Avalonia.Logging;
using ReactiveUI;

namespace GoonPlusPlus.Models;

public class DirectoryNode : ExplorerNode
{
    public ObservableCollection<ExplorerNode> SubNodes { get; } = new();

    private bool _isExpanded;

    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            SubNodes.Clear();
            if (this.RaiseAndSetIfChanged(ref _isExpanded, value)) GetSubfolders();
            else SubNodes.Add(new DirectoryNode("{{ Dummy Node }}"));
        }
    }

    public DirectoryNode(string strFullPath) : base(strFullPath)
    {
        SubNodes.Add(new FileNode("{{ Dummy Node }}"));

    }

    private void GetSubfolders()
    {
        try
        {
            foreach (var path in Directory.GetDirectories(FullPath, "*", SearchOption.TopDirectoryOnly))
            {
                SubNodes.Add(new DirectoryNode(path));
            }

            foreach (var path in Directory.GetFiles(FullPath, "*", SearchOption.TopDirectoryOnly))
            {
                SubNodes.Add(new FileNode(path));
            }
        }
        catch (UnauthorizedAccessException)
        {
            Logger.TryGet(LogEventLevel.Warning, LogArea.Binding)?.Log(this, $"Access denied");
        }
    }
}