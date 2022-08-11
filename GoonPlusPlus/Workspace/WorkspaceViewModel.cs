using ReactiveUI;

namespace GoonPlusPlus.Workspace;

public class WorkspaceViewModel : ViewModelBase
{
    public delegate void InstantiatedEventHandler(object sender);

    private WorkspaceModel? _workspace;

    public WorkspaceViewModel()
    {
        Instance = this;
        OnInstantiated();
    }

    public static WorkspaceViewModel Instance { get; private set; } = null!;

    public WorkspaceModel? Workspace
    {
        get => _workspace;
        set => this.RaiseAndSetIfChanged(ref _workspace, value);
    }

    public static event InstantiatedEventHandler? Instantiated;

    private void OnInstantiated() => Instantiated?.Invoke(this);
}
