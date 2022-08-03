using System.Linq;
using GoonPlusPlus.Models;

namespace GoonPlusPlus.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(params string[] paths)
    {
        if (paths.Length < 1)
        {
            var untitled = new TabModel();
            TabBuffer.Instance.AddTabs(untitled);
            TabBuffer.Instance.CurrentTab = untitled;
        }
        else
        {
            var models = paths
                .Select(p => new TabModel(p))
                .ToArray();

            TabBuffer.Instance.AddTabs(models);
            TabBuffer.Instance.CurrentTab = models.First();
        }
    }
}