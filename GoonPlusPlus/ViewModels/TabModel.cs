using Avalonia.Logging;
using GoonPlusPlus.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReactiveUI;
using System.IO;
using System.Linq;
using System.Reactive;

namespace GoonPlusPlus.ViewModels;

[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class TabModel : ViewModelBase
{
    private string _content = null!; // will be set by Content.set
    private string? _extension;
    private string _name = null!; // will be set by Name.set
    private string? _path;

    public TabModel(string path) => Path = path;

    public TabModel()
    {
        Path = null;
        Name = "Untitled 1";
        Extension = null;
        Content = string.Empty;
        IsUntitled = true;
    }

    /// <summary>
    ///     Path modifies other properties if it is set.
    /// </summary>
    [JsonProperty]
    public string? Path
    {
        get => _path;
        set
        {
            if (value == null)
            {
                this.RaiseAndSetIfChanged(ref _path, value);
                return;
            }

            try
            {
                Content = File.ReadAllText(value);
                Name = value.Split('\\').Last();
                Extension = value.Split("\\").Last().Contains('.') ? value.Split('.').Last() : null;
                IsUntitled = false;
                this.RaiseAndSetIfChanged(ref _path, value);
            }
            catch (IOException)
            {
                Logger.TryGet(LogEventLevel.Warning, LogArea.Binding)
                    ?.Log(this, $"{value} in use by another application");

                Content = $"{value} is in use by another application";
                Name = $"Untitled {TabBuffer.Instance.NumUntitled() + 1}";
                Extension = null;
                IsUntitled = true;
                this.RaiseAndSetIfChanged(ref _path, null);
            }
        }
    }

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

    [JsonProperty] public bool IsUntitled { get; private set; }

    /// <summary>
    ///     Closes the tab.
    /// </summary>
    public ReactiveCommand<TabModel, Unit> Close { get; } =
        ReactiveCommand.Create<TabModel>(tab => TabBuffer.Instance.RemoveTabs(tab));
}
