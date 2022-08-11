using Avalonia.Controls;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace GoonPlusPlus.ViewModels;

public class WorkspaceEditorViewModel : ViewModelBase
{
    private readonly ReadOnlyObservableCollection<string> _cpItems;

    private string _outputDir;
    private string _sourcePath;

    public WorkspaceEditorViewModel()
    {
        var wksp = WorkspaceViewModel.Instance.Workspace!;
        _outputDir = wksp.OutputDir;
        _sourcePath = wksp.SourcePath;
        _cpItems = new ReadOnlyObservableCollection<string>(new ObservableCollection<string>(wksp.Classpath.Items));
        wksp.Classpath.Connect().Bind(out _cpItems).ObserveOn(RxApp.MainThreadScheduler).Subscribe();

        RemoveItems = ReactiveCommand.Create(() => { wksp.Classpath!.Remove(Selection); });

        AddItems = ReactiveCommand.CreateFromTask(async (Window source) =>
            (await new OpenFileDialog { AllowMultiple = true }.ShowAsync(source))
            ?.Where(i => !wksp.Classpath.Items.Contains(i))
            .ToList()
            .ForEach(p => wksp.Classpath.Add(p)));

        this.WhenAnyValue(x => x.OutputDir)
            .Throttle(new TimeSpan(1000))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(o => wksp.OutputDir = o);
        this.WhenAnyValue(x => x.SourcePath)
            .Throttle(new TimeSpan(1000))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(s => wksp.SourcePath = s);
    }

    public ReadOnlyObservableCollection<string> CpItems => _cpItems;
    public string? Selection { get; set; }

    public string OutputDir
    {
        get => _outputDir;
        set => this.RaiseAndSetIfChanged(ref _outputDir, value);
    }

    public string SourcePath
    {
        get => _sourcePath;
        set => this.RaiseAndSetIfChanged(ref _sourcePath, value);
    }

    public ReactiveCommand<Unit, Unit> RemoveItems { get; }
    public ReactiveCommand<Window, Unit> AddItems { get; }
}
