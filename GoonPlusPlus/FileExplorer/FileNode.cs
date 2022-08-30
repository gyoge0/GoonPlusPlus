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

    public FileNode(string strFullPath) : base(strFullPath) { }

    public bool IsExpanded { get; }
}
