using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using DynamicData.Binding;
using GoonPlusPlus.Models;
using GoonPlusPlus.Models.ExplorerTree;
using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class TopMenuViewModel : ViewModelBase
{
    public ReactiveCommand<Window, Unit> OpenFile { get; } = ReactiveCommand.Create((Window source) =>
    {
        var paths = new OpenFileDialog().ShowAsync(source).Result;
        TabBuffer.Instance.AddTabs(paths ?? Array.Empty<string>());
    });

    /// <summary>
    /// Opens an untitled tab.
    /// </summary>
    public ReactiveCommand<Unit, Unit> NewTab { get; } = ReactiveCommand.Create(() =>
    {
        var tabs = TabBuffer.Instance
            .Buffer
            .Items
            .Where(k => k.IsUntitled)
            .Select(k => k.Name.Split(" ").Last())
            .Select(int.Parse)
            .ToArray();

        var num = tabs.Any() ? tabs.MaxBy(x => x) : 0;


        var tab = new TabModel
        {
            Name = $"Untitled {++num}"
        };

        TabBuffer.Instance
            .AddTabs(tab);
    });

    /// <summary>
    /// Saves the current tab if it is not untitled.
    /// </summary>
    public ReactiveCommand<Unit, Unit> SaveFile { get; } = ReactiveCommand.Create(() =>
    {
        var tab = TabBuffer.Instance.CurrentTab;
        if (tab == null || tab.IsUntitled) return;
        File.WriteAllTextAsync(
            tab.Path!,
            tab.Content
        );
    });

    /// <summary>
    /// Opens a file chooser menu to allow the user to choose what file to save the current tab's content to.
    /// Also modifies the tab's data to match that of the chosen file.
    /// </summary>
    public ReactiveCommand<Window, Unit> SaveFileAs { get; } = ReactiveCommand.Create((Window source) =>
    {
        var paths = new OpenFileDialog().ShowAsync(source).Result;
        if (paths is not { Length: > 0 }) return;
        var tab = TabBuffer.Instance.CurrentTab;
        if (tab == null) return;
        var file = paths.First();

        // remove the tab if it is open
        // prevents duplicate tabs
        var dup = TabBuffer.Instance
            .Buffer
            .Items
            .Where(k => !k.IsUntitled)
            .FirstOrDefault(k => k.Path!.ToLower().Equals(file.ToLower()));
        if (dup != null)
        {
            TabBuffer.Instance.RemoveTabs(dup);
        }

        tab.LoadFromFile(file);
        File.WriteAllTextAsync(
            file,
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

    private bool _fileCanCompile;

    public bool FileCanCompile
    {
        get => _fileCanCompile;
        set => this.RaiseAndSetIfChanged(ref _fileCanCompile, value);
    }

    public TopMenuViewModel()
    {
        TabBuffer.Instance
            .WhenPropertyChanged(x => x.CurrentTab)
            .WhereNotNull()
            .Subscribe(x => FileCanCompile = FileNode.CompilableExtensions.Contains(x.Value?.Extension));
    }

    public ReactiveCommand<Unit, Unit> Compile { get; } = ReactiveCommand.CreateFromTask(async () =>
    {
        var currentTab = TabBuffer.Instance.CurrentTab;
        if (currentTab == null) return;


        using var compile = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "javac", Arguments = currentTab.Path, RedirectStandardOutput = true,
                RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true
            }
        };

        compile.Start();
        await compile.WaitForExitAsync();
        var vm = CompileViewModel.Instance;

        vm.CompileOutput = string.Empty;

        var stdout = await compile.StandardOutput.ReadToEndAsync();
        if (stdout != string.Empty)
        {
            vm.CompileOutput += "<-- Standard Output -->\n\n";
            vm.CompileOutput += stdout;
            vm.CompileOutput += "<-- End Standard Output --> \n\n";
        }

        var stderr = await compile.StandardError.ReadToEndAsync();
        if (stderr != string.Empty)
        {
            vm.CompileOutput += "<-- Standard Error -->\n\n";
            vm.CompileOutput += stderr;
            vm.CompileOutput += "<-- End Standard Error --> \n\n";
        }

        vm.CompileOutput += $"Process exited with code {compile.ExitCode} --- Compilation ";
        vm.CompileOutput += compile.ExitCode == 0 ? "Successful" : "Failed";
        vm.CompileOutput += ".";
    });
}