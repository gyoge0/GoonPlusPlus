using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reactive;
using GoonPlusPlus.Models;
using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class TabModel : IEquatable<TabModel>
{
    public string? Path { get; private set; }
    public string Name { get; set; }
    public string? Extension { get; set; }

    public string Content { get; set; }

    public bool IsUntitled { get; private set; }

    public TabModel(string path)
    {
        Path = path;
        Name = Path.Split('\\').Last();
        Extension = Path.Split("\\").Last().Contains('.') ? Path.Split('.').Last() : null;
        Content = File.ReadAllText(path);
        IsUntitled = false;
    }

    public TabModel()
    {
        Path = null;
        Name = "Untitled 1";
        Extension = null;
        Content = string.Empty;
        IsUntitled = true;
    }

    public void LoadFromFile(string path)
    {
        Path = path;
        Name = Path.Split('\\').Last();
        Extension = Path.Split("\\").Last().Contains('.') ? Path.Split('.').Last() : null;
        IsUntitled = false;
    }

    /// <summary>
    /// Closes the tab.
    /// </summary>
    public ReactiveCommand<TabModel, Unit> Close { get; } =
        ReactiveCommand.Create<TabModel>((tab) => TabBuffer.Instance.RemoveTabs(tab));

    public bool Equals(TabModel? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return
            Path == other.Path
            && Name == other.Name
            && Extension == other.Extension
            && Content == other.Content
            && IsUntitled == other.IsUntitled;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TabModel)obj);
    }

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        return HashCode.Combine(Path, Name, Extension, Content, IsUntitled);
    }

    public static bool operator ==(TabModel? left, TabModel? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TabModel? left, TabModel? right)
    {
        return !Equals(left, right);
    }
}