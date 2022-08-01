using System;
using System.IO;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using GoonPlusPlus.Models;
using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class TopMenuViewModel : ViewModelBase
{
    public ReactiveCommand<Window, Unit> OpenFile { get; } = ReactiveCommand.CreateFromTask(async (Window source) =>
    {
        var paths = await new OpenFileDialog().ShowAsync(source);
        TabBuffer.Instance.AddTabs(paths ?? Array.Empty<string>());
    });

    /// <summary>
    /// Opens an untitled tab.
    /// </summary>
    public ReactiveCommand<Unit, Unit> NewTab { get; } = ReactiveCommand.Create(() =>
        TabBuffer.Instance.AddTabs(new TabModel { Name = $"Untitled {TabBuffer.Instance.NumUntitiled() + 1}" }));

    /// <summary>
    /// Saves the current tab if it is not untitled.
    /// </summary>
    public ReactiveCommand<Unit, Unit> SaveFile { get; } = ReactiveCommand.CreateFromTask(async () =>
    {
        var tab = TabBuffer.Instance.CurrentTab;
        if (tab == null || tab.IsUntitled) return;
        await File.WriteAllTextAsync(
            tab.Path!,
            tab.Content
        );
    });

    /// <summary>
    /// Opens a file chooser menu to allow the user to choose what file to save the current tab's content to.
    /// Also modifies the tab's data to match that of the chosen file.
    /// </summary>
    public ReactiveCommand<Window, Unit> SaveFileAs { get; } = ReactiveCommand.CreateFromTask(async (Window source) =>
    {
        var path = await new SaveFileDialog().ShowAsync(source);
        
        if (path == null) return;
        var tab = TabBuffer.Instance.CurrentTab;
        if (tab == null) return;

        // remove the tab if it is open
        // prevents duplicate tabs
        var dup = TabBuffer.Instance
            .Buffer
            .Items
            .Where(k => !k.IsUntitled)
            .FirstOrDefault(k => k.Path!.ToLower().Equals(path.ToLower()));

        if (dup != null) TabBuffer.Instance.RemoveTabs(dup);

        tab.LoadFromFile(path);
        await File.WriteAllTextAsync(
            path,
            tab.Content
        );
    });

    /// <summary>
    /// Closes the current open tab.
    /// </summary>
    public ReactiveCommand<Unit, Unit> CloseFile { get; } = ReactiveCommand.Create(() =>
    {
        if (TabBuffer.Instance.CurrentTab == null) return;
        TabBuffer.Instance.RemoveTabs(TabBuffer.Instance.CurrentTab);
    });

    /// <summary>
    /// If the current tab has a selection highlighted, will copy it to the system clipboard.
    /// </summary>
    public ReactiveCommand<Unit, Unit> CopyText { get; } = ReactiveCommand.Create(() =>
    {
        if (TabBuffer.Instance.CurrentEditor == null) return;
        TabBuffer.Instance.CurrentEditor.EditArea.Cut();
    });

    /// <summary>
    /// If the current tab has a selection highlighted, will copy it to the system clipboard and delete the selection.
    /// </summary>
    public ReactiveCommand<Unit, Unit> CutText { get; } = ReactiveCommand.Create(() =>
    {
        if (TabBuffer.Instance.CurrentEditor == null) return;
        TabBuffer.Instance.CurrentEditor.EditArea.Cut();
    });

    /// <summary>
    /// If the current tab has a selection highlighted, will paste the contents of the system clipboard to replace the selection.
    /// Otherwise, will insert the contents of the system clipboard at the current cursor position.
    /// </summary>
    public ReactiveCommand<Unit, Unit> PasteText { get; } = ReactiveCommand.Create(() =>
    {
        if (TabBuffer.Instance.CurrentEditor == null) return;
        TabBuffer.Instance.CurrentEditor.EditArea.Paste();
    });
}