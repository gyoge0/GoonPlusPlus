using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using Avalonia.Controls;
using GoonPlusPlus.Models.ExplorerTree;
using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class FileExplorerViewModel : ViewModelBase
{
    private static string HomeFolder { get; } = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private static FileExplorerViewModel _instance = null!;

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
        _instance = this;
        UndoStack.Push(Root[0]);
    }

    public Stack<DirectoryNode> UndoStack { get; } = new();
    public Stack<DirectoryNode> RedoStack { get; } = new();

    public ReactiveCommand<Unit, Unit> Undo { get; } = ReactiveCommand.Create(() =>
    {
        if (_instance.UndoStack.Count <= 1) return;

        var current = _instance.UndoStack.Pop();
        _instance.RedoStack.Push(current);

        var node = _instance.UndoStack.Peek();
        SwapFolder(node);
    });

    public ReactiveCommand<Unit, Unit> Redo { get; } = ReactiveCommand.Create(() =>
    {
        if (_instance.RedoStack.Count < 1) return;
        var node = _instance.RedoStack.Pop();
        _instance.UndoStack.Push(node);
        SwapFolder(node);
    });

    public ReactiveCommand<Unit, Unit> GoToParent { get; } = ReactiveCommand.Create(() =>
    {
        var parent = Directory.GetParent(_instance.Root[0].FullPath);
        if (parent == null) return;
        var node = new DirectoryNode(parent.FullName);
        SwapFolder(node);

        if (_instance.RedoStack.Count < 1) _instance.UndoStack.Push(node);
        else if (_instance.RedoStack.Peek() != node)
        {
            _instance.RedoStack.Clear();
            _instance.UndoStack.Push(node);
        }
        else if (_instance.RedoStack.Peek() == node) _instance.UndoStack.Push(_instance.RedoStack.Pop());
    });

    public ReactiveCommand<Window, Unit> OpenFolder { get; } = ReactiveCommand.CreateFromTask(async (Window source) =>
    {
        var path = await new OpenFolderDialog { Directory = _instance.Root[0].FullPath }.ShowAsync(source);
        if (path == null) return;
        var node = new DirectoryNode(path);
        SwapFolder(node);
        
        if (_instance.RedoStack.Count < 1) _instance.UndoStack.Push(node);
        else if (_instance.RedoStack.Peek() != node)
        {
            _instance.RedoStack.Clear();
            _instance.UndoStack.Push(node);
        }
        else if (_instance.RedoStack.Peek() == node) _instance.UndoStack.Push(_instance.RedoStack.Pop());
    });

    /// <summary>
    /// Opens the path in the file explorer tree.
    /// </summary>
    /// <param name="node">The path to open.</param>
    private static void SwapFolder(DirectoryNode node)
    {
        var isExpanded = _instance.Root[0].IsExpanded;
        _instance.Root[0] = node;
        if (node.IsExpanded != isExpanded) node.IsExpanded = isExpanded;
    }

    public ReactiveCommand<Unit, Unit> Refresh { get; } = ReactiveCommand.Create(() =>
    {
        // this will force a refresh
        _instance.Root[0].IsExpanded = false;
        _instance.Root[0].IsExpanded = true;
    });
}