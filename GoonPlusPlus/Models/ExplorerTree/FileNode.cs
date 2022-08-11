namespace GoonPlusPlus.Models.ExplorerTree;

public class FileNode : ExplorerNode
{
    public static readonly string[] CompilableExtensions = { "java" };

    public static readonly string[] RunnableExtensions = { "java" };

    public FileNode(string strFullPath) : base(strFullPath) { }

    public bool IsExpanded { get; }
}
