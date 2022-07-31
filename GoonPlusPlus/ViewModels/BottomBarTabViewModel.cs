using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class BottomBarTabViewModel : ViewModelBase
{
    public static BottomBarTabViewModel Instance { get; private set; }
    private int _currentTabIdx;

    public int CurrentTabIdx
    {
        get => _currentTabIdx;
        set => this.RaiseAndSetIfChanged(ref _currentTabIdx, value);
    }

    public BottomBarTabViewModel()
    {
        Instance = this;
    }

    public enum TabIdx
    {
        Compile,
        Run,
    }
}