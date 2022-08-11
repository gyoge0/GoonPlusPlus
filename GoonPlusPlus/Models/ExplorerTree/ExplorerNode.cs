using GoonPlusPlus.ViewModels;
using System.IO;

namespace GoonPlusPlus.Models.ExplorerTree;

public abstract class ExplorerNode : ViewModelBase
{
    protected ExplorerNode(string strFullPath)
    {
        FullPath = strFullPath;
        Name = Path.GetFileName(strFullPath);
    }

    public string Name { get; }
    public string FullPath { get; }
}
