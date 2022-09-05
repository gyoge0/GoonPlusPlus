using GoonPlusPlus.TabBar;
using GoonPlusPlus.TopMenu;
using ReactiveUI;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Windows.Input;

namespace GoonPlusPlus.FileExplorer;

public class FileNode : ExplorerNode
{
    public static readonly string[] CompilableExtensions =
    {
        "java",
    };

    public static readonly string[] RunnableExtensions =
    {
        "java",
        "py"
    };

    public FileNode(string strFullPath) : base(strFullPath)
    {
        Delete = ReactiveCommand.Create(() =>
        {
            var models = TabBuffer.Instance
                .Buffer
                .Items
                .Where(x => x.Path == FullPath)
                .ToList();

            models.ForEach(x =>
            {
                File.WriteAllText(FullPath, x.Content);
                File.Delete(FullPath);
            });

            TabBuffer.Instance.RemoveTabs(models.ToArray());
        });

        Open = ReactiveCommand.Create(() =>
        {
            var model = new TabModel(FullPath);
            var tabBuffer = TabBuffer.Instance;
            tabBuffer.AddTabs(model);
            tabBuffer.CurrentTab = model;
        });
    }

    public bool IsExpanded { get; }
    public ReactiveCommand<Unit, Unit> Delete { get; }
    public ReactiveCommand<Unit, Unit> Open { get; }
}