using GoonPlusPlus.ViewModels;

namespace GoonPlusPlus.Models;

public class FileNode : ExplorerNode
{
    private readonly bool _isExpanded;

    public bool IsExpanded => _isExpanded;

    public FileNode(string strFullPath) : base(strFullPath)
    {
    }
}