using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using GoonPlusPlus.Models.ExplorerTree;
using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class FileExplorerViewModel : ViewModelBase
{
    public FileExplorerViewModel()
    {
        Instance = this;
        UndoStack.Push(Root[0]);
        WorkspaceViewModel.Instantiated += sender => (sender as WorkspaceViewModel)
            .WhenAnyValue(x => x!.Workspace)
            .WhereNotNull()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(p => Root[0] = new DirectoryNode(p.RootPath) { IsExpanded = true });
    }

    private static string HomeFolder { get; } = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    public static FileExplorerViewModel Instance { get; private set; } = null!;

    public ObservableCollection<ExplorerNode> SelectedItems { get; } = new();

    // ReSharper disable once CollectionNeverQueried.Global
    public ObservableCollection<DirectoryNode> Root { get; } = new()
    {
        new DirectoryNode(HomeFolder)
        {
            IsExpanded = true
        }
    };

    public Stack<DirectoryNode> UndoStack { get; } = new();
    public Stack<DirectoryNode> RedoStack { get; } = new();

    public ReactiveCommand<Unit, Unit> Undo { get; } = ReactiveCommand.Create(() =>
    {
        if (Instance.UndoStack.Count <= 1) return;

        var current = Instance.UndoStack.Pop();
        Instance.RedoStack.Push(current);

        var node = Instance.UndoStack.Peek();
        SwapFolder(node);
    });

    public ReactiveCommand<Unit, Unit> Redo { get; } = ReactiveCommand.Create(() =>
    {
        if (Instance.RedoStack.Count < 1) return;
        var node = Instance.RedoStack.Pop();
        Instance.UndoStack.Push(node);
        SwapFolder(node);
    });

    public ReactiveCommand<Unit, Unit> GoToParent { get; } = ReactiveCommand.Create(() =>
    {
        var parent = Directory.GetParent(Instance.Root[0].FullPath);
        if (parent == null) return;
        var node = new DirectoryNode(parent.FullName);
        SwapFolder(node);

        if (Instance.RedoStack.Count < 1)
        {
            Instance.UndoStack.Push(node);
        }
        else if (Instance.RedoStack.Peek() != node)
        {
            Instance.RedoStack.Clear();
            Instance.UndoStack.Push(node);
        }
        else if (Instance.RedoStack.Peek() == node)
        {
            Instance.UndoStack.Push(Instance.RedoStack.Pop());
        }
    });

    public ReactiveCommand<Window, Unit> OpenFolder { get; } = ReactiveCommand.CreateFromTask(async (Window source) =>
    {
        var path = await new OpenFolderDialog { Directory = Instance.Root[0].FullPath }.ShowAsync(source);
        if (path == null) return;
        var node = new DirectoryNode(path);
        SwapFolder(node);

        if (Instance.RedoStack.Count < 1)
        {
            Instance.UndoStack.Push(node);
        }
        else if (Instance.RedoStack.Peek() != node)
        {
            Instance.RedoStack.Clear();
            Instance.UndoStack.Push(node);
        }
        else if (Instance.RedoStack.Peek() == node)
        {
            Instance.UndoStack.Push(Instance.RedoStack.Pop());
        }
    });

    public ReactiveCommand<Unit, Unit> Refresh { get; } = ReactiveCommand.Create(() =>
    {
        // this will force a refresh
        Instance.Root[0].IsExpanded = false;
        Instance.Root[0].IsExpanded = true;
    });

    /// <summary>
    ///     Opens the path in the file explorer tree.
    /// </summary>
    /// <param name="node">The path to open.</param>
    private static void SwapFolder(DirectoryNode node)
    {
        var isExpanded = Instance.Root[0].IsExpanded;
        Instance.Root[0] = node;
        if (node.IsExpanded != isExpanded) node.IsExpanded = isExpanded;
    }
}