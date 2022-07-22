﻿namespace GoonPlusPlus.Models.ExplorerTree;

public class FileNode : ExplorerNode
{
    private readonly bool _isExpanded;

    public bool IsExpanded => _isExpanded;

    public FileNode(string strFullPath) : base(strFullPath)
    {
    }
}