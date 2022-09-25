using Avalonia.Controls;
using Avalonia.Logging;
using CliWrap;
using CliWrap.Buffered;
using DynamicData;
using DynamicData.Binding;
using GoonPlusPlus.CodeRunner;
using GoonPlusPlus.CodeRunner.Compile;
using GoonPlusPlus.CodeRunner.Run;
using GoonPlusPlus.Customization;
using GoonPlusPlus.FileExplorer;
using GoonPlusPlus.TabBar;
using GoonPlusPlus.Workspace;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using WorkspaceEditor = GoonPlusPlus.Workspace.WorkspaceEditor;

namespace GoonPlusPlus.TopMenu;

public class TopMenuViewModel : ViewModelBase
{
    private bool _fileCanCompile;
    private bool _fileCanRun;
    private bool _openProject;

    public TopMenuViewModel()
    {
        TabBuffer.Instance.WhenPropertyChanged(x => x.CurrentTab)
            .WhereNotNull()
            .Subscribe(x =>
            {
                FileCanCompile = FileNode.CompilableExtensions.Contains(x.Value?.Extension);
                FileCanRun = FileNode.RunnableExtensions.Contains(x.Value?.Extension);
            });
        WorkspaceViewModel.Instantiated += sender => (sender as WorkspaceViewModel)
            .WhenAnyValue(x => x!.Workspace)
            .Subscribe(p => OpenProject = p == null);
    }

    public ReactiveCommand<Window, Unit> OpenFile { get; } = ReactiveCommand.CreateFromTask(async (Window source) =>
    {
        var paths = await new OpenFileDialog().ShowAsync(source);
        TabBuffer.Instance.AddTabs(paths ?? Array.Empty<string>());
    });

    /// <summary>
    ///     Opens an untitled tab.
    /// </summary>
    public ReactiveCommand<Unit, Unit> NewTab { get; } = ReactiveCommand.Create(() =>
        TabBuffer.Instance.AddTabs(new TabModel { Name = $"Untitled {TabBuffer.Instance.NumUntitled() + 1}" }));

    /// <summary>
    ///     Saves the current tab if it is not untitled.
    /// </summary>
    public ReactiveCommand<Unit, Unit> SaveFile { get; } = ReactiveCommand.CreateFromTask(async () =>
    {
        var wksp = WorkspaceViewModel.Instance.Workspace?.Save();

        var tab = TabBuffer.Instance.CurrentTab;
        if (tab == null || tab.IsUntitled) return;

        await using (var fs = new FileStream(tab.Path!, FileMode.Truncate, FileAccess.Write))
        await using (var writer = new StreamWriter(fs))
        {
            await writer.WriteAsync(tab.Content);
        }

        if (wksp == null) return;
        await wksp;
    });

    /// <summary>
    ///     Opens a file chooser menu to allow the user to choose what file to save the current tab's content to.
    ///     Also modifies the tab's data to match that of the chosen file.
    /// </summary>
    public ReactiveCommand<Window, Unit> SaveFileAs { get; } = ReactiveCommand.CreateFromTask(async (Window source) =>
    {
        var wksp = WorkspaceViewModel.Instance.Workspace?.Save();

        var path = await new SaveFileDialog().ShowAsync(source);

        if (path == null) return;
        var tab = TabBuffer.Instance.CurrentTab;
        if (tab == null) return;

        // TODO: figure out if this code still needs to be here
        // remove the tab if it is open
        // prevents duplicate tabs
        // var dup = TabBuffer.Instance.Buffer.Items.Where(k => !k.IsUntitled)
        //     .FirstOrDefault(k => k.Path!.ToLower().Equals(path.ToLower()));
        // if (dup != null) TabBuffer.Instance.RemoveTabs(dup);

        await using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        await using var writer = new StreamWriter(fs);
        await writer.WriteAsync(tab.Content);

        tab.Path = path;
        tab.Name = Path.GetFileName(tab.Path);

        if (wksp == null) return;
        await wksp;
    });

    /// <summary>
    ///     Closes the current open tab.
    /// </summary>
    public ReactiveCommand<Unit, Unit> CloseFile { get; } = ReactiveCommand.Create(() =>
    {
        if (TabBuffer.Instance.CurrentTab == null) return;
        TabBuffer.Instance.RemoveTabs(TabBuffer.Instance.CurrentTab);
    });

    /// <summary>
    ///     If the current tab has a selection highlighted, will copy it to the system clipboard.
    /// </summary>
    public ReactiveCommand<Unit, Unit> CopyText { get; } = ReactiveCommand.Create(() =>
    {
        if (TabBuffer.Instance.CurrentEditor == null) return;
        TabBuffer.Instance.CurrentEditor.EditArea.Cut();
    });

    /// <summary>
    ///     If the current tab has a selection highlighted, will copy it to the system clipboard and delete the selection.
    /// </summary>
    public ReactiveCommand<Unit, Unit> CutText { get; } = ReactiveCommand.Create(() =>
    {
        if (TabBuffer.Instance.CurrentEditor == null) return;
        TabBuffer.Instance.CurrentEditor.EditArea.Cut();
    });

    /// <summary>
    ///     If the current tab has a selection highlighted, will paste the contents of the system clipboard to replace the
    ///     selection.
    ///     Otherwise, will insert the contents of the system clipboard at the current cursor position.
    /// </summary>
    public ReactiveCommand<Unit, Unit> PasteText { get; } = ReactiveCommand.Create(() =>
    {
        if (TabBuffer.Instance.CurrentEditor == null) return;
        TabBuffer.Instance.CurrentEditor.EditArea.Paste();
    });

    public ReactiveCommand<Unit, Unit> EditCustomCfg { get; } = ReactiveCommand.Create(
        () => TabBuffer.Instance.AddTabs(CustomCfg.Instance.ConfigFilePath));

    public ReactiveCommand<Unit, Unit> ReloadCustomCfg { get; } =
        ReactiveCommand.Create(() => CustomCfg.Instance.LoadConfig());


    public ReactiveCommand<Unit, Unit> Create { get; } = ReactiveCommand.CreateFromTask(async () =>
    {
        var fullPath = FileExplorerViewModel.Instance.Root[0].FullPath;
        var workspace = new WorkspaceModel(fullPath);
        // ReSharper disable once StringLiteralTypo
        var path = Path.Join(fullPath, "wksp.gpp");
        TabBuffer.Instance.Buffer.Items.ToList().ForEach(workspace.Tabs.Add);

        try
        {
            File.Create(path).Close();
            await workspace.Save();
        }
        catch (UnauthorizedAccessException)
        {
            Logger.TryGet(LogEventLevel.Warning, LogArea.Binding)?.Log(workspace, $"Access denied to {path}");
            return;
        }

        WorkspaceViewModel.Instance.Workspace = workspace;
    });

    public ReactiveCommand<Window, Unit> Open { get; } = ReactiveCommand.CreateFromTask(async (Window source) =>
    {
        var selected = await new OpenFileDialog
        {
            Filters = { new FileDialogFilter { Name = "Goon++ Project (*.gpp)", Extensions = { "gpp" } } },
        }.ShowAsync(source);

        if (selected == null || selected.Length < 1 || selected[0].Split(".").Last() != "gpp") return;

        var proj = WorkspaceModel.Load(selected[0]);
        if (proj == null) return;
        WorkspaceViewModel.Instance.Workspace = proj;
    });

    public ReactiveCommand<Unit, Unit> Exit { get; } = ReactiveCommand.CreateFromTask(async () =>
    {
        var proj = WorkspaceViewModel.Instance.Workspace!;
        var task = proj.Save();
        WorkspaceViewModel.Instance.Workspace = null;
        await task;
    });

    public ReactiveCommand<Window, Unit> Configure { get; } = ReactiveCommand.CreateFromTask(
        async (Window source) => await new WorkspaceEditor().ShowDialog(source));

    public bool FileCanCompile
    {
        get => _fileCanCompile;
        set => this.RaiseAndSetIfChanged(ref _fileCanCompile, value);
    }

    public bool FileCanRun
    {
        get => _fileCanRun;
        set => this.RaiseAndSetIfChanged(ref _fileCanRun, value);
    }

    public bool OpenProject
    {
        get => _openProject;
        set => this.RaiseAndSetIfChanged(ref _openProject, value);
    }

    public ReactiveCommand<Unit, Unit> Compile { get; } = ReactiveCommand.CreateFromTask(async () =>
    {
        var currentTab = TabBuffer.Instance.CurrentTab;
        if (currentTab == null) return;

        // ReSharper disable once IdentifierTypo
        var compilevm = CompileViewModel.Instance;
        var wksp = WorkspaceViewModel.Instance.Workspace;

        var compile = Cli.Wrap("javac")
            .WithArguments(args =>
            {
                args.Add(currentTab.Path!);

                if (wksp == null) return;

                args.Add("--source-path");
                args.Add(wksp.SourcePath);

                if (wksp.Classpath.Count > 0)
                {
                    args.Add("-cp");

                    var sb = new StringBuilder();
                    wksp.Classpath.Items.ToList().ForEach(d => sb.Append($"{d};"));

                    args.Add(sb.ToString());
                }

                if (!Directory.Exists(wksp.OutputDir)) Directory.CreateDirectory(wksp.OutputDir);

                args.Add("-d");
                args.Add($"{wksp.OutputDir}");
            })
            .WithValidation(CommandResultValidation.None);

        var res = await compile.ExecuteBufferedAsync();

        BottomBarTabViewModel.Instance.CurrentTabIdx = (int)BottomBarTabViewModel.TabIdx.Compile;

        compilevm.CompileOutput = string.Empty;

        if (res.StandardOutput.Length > 0)
        {
            compilevm.CompileOutput += "<-- Standard Output -->\n\n";
            compilevm.CompileOutput += res.StandardOutput;
            compilevm.CompileOutput += "<-- End Standard Output --> \n\n";
        }

        if (res.StandardError.Length > 0)
        {
            compilevm.CompileOutput += "<-- Standard Error -->\n\n";
            compilevm.CompileOutput += res.StandardError;
            compilevm.CompileOutput += "\n<-- End Standard Error --> \n\n";
        }

        compilevm.CompileOutput += $"Process exited with code {res.ExitCode} --- Compilation ";
        compilevm.CompileOutput += res.ExitCode == 0 ? "Successful" : "Failed";
        compilevm.CompileOutput += ".";
    });

    public ReactiveCommand<Unit, Unit> Run { get; } = ReactiveCommand.CreateFromTask(async () =>
    {
        var currentTab = TabBuffer.Instance.CurrentTab;
        if (currentTab == null) return;

        var sb = new StringBuilder();
        var wksp = WorkspaceViewModel.Instance.Workspace;

        switch (currentTab.Extension)
        {
            case "java":
                if (wksp != null)
                {
                    sb.Append(" -cp \"");
                    sb.Append((string?)$"{wksp.OutputDir};");
                    wksp.Classpath.Items.ToList().ForEach(j => sb.Append(j + ";"));
                    sb.Append("\" ");
                }

                sb.Append(Path.GetFileNameWithoutExtension(currentTab.Path));
                break;
            case "py":
                sb.Append(Path.GetFileName(currentTab.Path));
                break;
        }


        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = currentTab.Extension switch
                {
                    "java" => "java.exe",
                    "py" => "python.exe",
                    _ => throw new ArgumentOutOfRangeException()
                },
                Arguments = sb.ToString(),
                WorkingDirectory = Path.GetDirectoryName(currentTab.Path),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
            },
        };
        process.Start();

        var automator = new ConsoleAutomator(process.StandardInput, process.StandardOutput);

        automator.StandardInputRead += (_, args) => RunViewModel.Instance.Output.Add(args.Input);
        automator.StartAutomating();
        RunViewModel.Instance.RunProcess = process;
        BottomBarTabViewModel.Instance.CurrentTabIdx = (int)BottomBarTabViewModel.TabIdx.Run;

        await process.WaitForExitAsync();
        RunViewModel.Instance.RunProcess = null;
        var stderr = process.StandardError.ReadToEnd();
        if (stderr.Length > 0)
        {
            RunViewModel.Instance.Output.Add("\n\n<-- Standard Error -->\n\n");
            RunViewModel.Instance.Output.Add(stderr);
            RunViewModel.Instance.Output.Add("\n<-- End Standard Error --> \n\n");
        }
    });
}