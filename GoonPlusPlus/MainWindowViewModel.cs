using GoonPlusPlus.TabBar;
using GoonPlusPlus.Workspace;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;

namespace GoonPlusPlus;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(params string[] paths)
    {
        switch (paths.Length)
        {
            case 1 when new FileInfo(paths.Single()).Name == "wksp.gpp":
                WorkspaceViewModel.Instantiated += _ =>
                {
                    var sb = new StringBuilder();
                    using (var fs = new FileStream(paths.Single(), FileMode.Open, FileAccess.Read))
                    using (var reader = new StreamReader(fs))
                    {
                        while (!reader.EndOfStream)
                        {
                            sb.Append(reader.ReadLine());
                        }
                    }

                    var wksp = JsonConvert.DeserializeObject<WorkspaceModel>(sb.ToString());
                    if (wksp == null) return;
                    WorkspaceViewModel.Instance.Workspace = wksp;
                };
                break;
            case < 1:
            {
                var untitled = new TabModel();
                TabBuffer.Instance.AddTabs(untitled);
                TabBuffer.Instance.CurrentTab = untitled;
                break;
            }
            default:
            {
                var models = paths.Select(p => new TabModel(p)).ToArray();

                TabBuffer.Instance.AddTabs(models);
                TabBuffer.Instance.CurrentTab = models.First();
                break;
            }
        }

        // ReSharper disable once UnusedVariable
        var projectViewModel = new WorkspaceViewModel();
    }
}