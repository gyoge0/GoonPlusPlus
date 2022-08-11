using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class BottomBarTabViewModel : ViewModelBase
{
    public enum TabIdx
    {
        Compile,
        Run
    }

    private int _currentTabIdx;

    public BottomBarTabViewModel()
    {
        Instance = this;
    }

    public static BottomBarTabViewModel Instance { get; private set; }

    public int CurrentTabIdx
    {
        get => _currentTabIdx;
        set => this.RaiseAndSetIfChanged(ref _currentTabIdx, value);
    }
}