using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reactive;
using Avalonia.Logging;
using GoonPlusPlus.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class TabModel : ViewModelBase, IEquatable<TabModel>
{
    private string _name = null!; // will be set by Name.set
    private string? _extension;
    private string _content = null!; // will be set by Content.set
    [JsonProperty]
    public string? Path { get; private set; }

    [JsonProperty]
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public string? Extension
    {
        get => _extension;
        set => this.RaiseAndSetIfChanged(ref _extension, value);
    }

    public string Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }

    [JsonProperty]
    public bool IsUntitled { get; private set; }

    public TabModel(string path)
    {
        try
        {
            Path = path;
            Name = Path.Split('\\').Last();
            Extension = Path.Split("\\").Last().Contains('.') ? Path.Split('.').Last() : null;
            Content = File.ReadAllText(path);
            IsUntitled = false;
        }
        catch (IOException)
        {
            Logger.TryGet(LogEventLevel.Warning, LogArea.Binding)?.Log(this, $"{path} in use by another application");
            Path = null;
            Name = $"Untitled {TabBuffer.Instance.NumUntitled() + 1}";
            Extension = null;
            Content = $"{path} is in use by another application";
        }
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