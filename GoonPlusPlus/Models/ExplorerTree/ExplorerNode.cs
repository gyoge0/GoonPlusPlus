using System.IO;
using GoonPlusPlus.ViewModels;

namespace GoonPlusPlus.Models.ExplorerTree;

public abstract class ExplorerNode : ViewModelBase
{
    
    public string Name { get; }
    public string FullPath { get; }


    protected ExplorerNode(string strFullPath)
    {
        FullPath = strFullPath;
        Name = Path.GetFileName(strFullPath);
    }
}