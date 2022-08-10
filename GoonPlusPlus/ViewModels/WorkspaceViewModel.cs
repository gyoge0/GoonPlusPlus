using GoonPlusPlus.Models;
using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class WorkspaceViewModel : ViewModelBase
{
    public static WorkspaceViewModel Instance { get; private set; } = null!;

    private WorkspaceModel? _workspace;

    public WorkspaceModel? Workspace
    {
        get => _workspace;
        set => this.RaiseAndSetIfChanged(ref _workspace, value);
    }


    public delegate void InstantiatedEventHandler(object sender);

    public static event InstantiatedEventHandler? Instantiated;
    private void OnInstantiated() => Instantiated?.Invoke(this);

    public WorkspaceViewModel()
    {
        Instance = this;
        OnInstantiated();
    }
}