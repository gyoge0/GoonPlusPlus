using System.Reactive;
using GoonPlusPlus.Models;
using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        var untitled = new TabModel();
        TabBuffer.Instance.AddTabs(untitled);
        TabBuffer.Instance.CurrentTab = untitled;
    }

}