using Avalonia.Logging;
using DynamicData;
using GoonPlusPlus.TabBar;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoonPlusPlus.Workspace;

[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class WorkspaceModel
{
    public WorkspaceModel(string rootPath)
    {
        RootPath = rootPath;
        SourcePath = rootPath;
        OutputDir = rootPath;
        Classpath.Connect()
            .OnItemAdded(i =>
            {
                if (CpAsList.Contains(i)) return;
                CpAsList.Add(i);
            })
            .OnItemRemoved(i => CpAsList.Add(i))
            .Subscribe();

        TabBuffer.Instance.Buffer.Connect()
            .OnItemAdded(x =>
            {
                if (Tabs.Contains(x)) return;
                Tabs.Add(x);
            })
            .OnItemRemoved(x => Tabs.Remove(x))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe();
    }

    [JsonIgnore] public SourceList<string> Classpath { get; } = new();
    [JsonProperty("classpath")] private List<string> CpAsList { get; } = new();
    public string RootPath { get; set; }
    public string SourcePath { get; set; }
    public string OutputDir { get; set; }
    public List<TabModel> Tabs { get; } = new();

    public static WorkspaceModel? Load(string path)
    {
        var sb = new StringBuilder();
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        using (var reader = new StreamReader(fs))
        {
            while (!reader.EndOfStream)
            {
                sb.Append(reader.ReadLine());
            }
        }

        var ret = JsonConvert.DeserializeObject<WorkspaceModel>(sb.ToString());
        ret?.CpAsList.ToList().ForEach(i => ret.Classpath.Add(i));
        return ret;
    }

    public async Task Save()
    {
        var path = Path.Join(RootPath, "wksp.gpp");
        try
        {
            await using var fs = new FileStream(path, FileMode.Truncate, FileAccess.Write);
            await using var writer = new StreamWriter(fs);
            await writer.WriteAsync(JsonConvert.SerializeObject(this, Formatting.Indented));
        }
        catch (UnauthorizedAccessException)
        {
            Logger.TryGet(LogEventLevel.Warning, LogArea.Binding)?.Log(this, $"Access to file {path} denied");
        }
    }
}