using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using DynamicData;
using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class WorkspaceEditorViewModel : ViewModelBase
{
    private readonly ReadOnlyObservableCollection<string> _cpItems;

    public ReadOnlyObservableCollection<string> CpItems => _cpItems;
    public string? Selection { get; set; }

    public WorkspaceEditorViewModel()
    {
        var wksp = WorkspaceViewModel.Instance.Workspace!;
        _cpItems = new ReadOnlyObservableCollection<string>(new ObservableCollection<string>(wksp.Classpath.Items));
        wksp.Classpath
            .Connect()
            .Bind(out _cpItems)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe();

        RemoveItems = ReactiveCommand.Create(() => { wksp.Classpath!.Remove(Selection); });

        AddItems = ReactiveCommand.CreateFromTask(async (Window source) =>
            (await new OpenFileDialog { AllowMultiple = true }.ShowAsync(source))?
            .Where(i => !wksp.Classpath.Items.Contains(i))
            .ToList()
            .ForEach(p => wksp.Classpath.Add(p)));
    }

    public ReactiveCommand<Unit, Unit> RemoveItems { get; }
    public ReactiveCommand<Window, Unit> AddItems { get; }
}