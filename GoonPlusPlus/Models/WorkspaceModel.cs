using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DynamicData;
using GoonPlusPlus.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GoonPlusPlus.Models;

[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class WorkspaceModel
{
    public WorkspaceModel(string rootPath)
    {
        RootPath = rootPath;
        OutputDir = rootPath;
        Classpath.Connect()
            .OnItemAdded(i =>
            {
                if (CpAsList.Contains(i)) return;
                CpAsList.Add(i);
            })
            .OnItemRemoved(i => CpAsList.Add(i))
            .Subscribe();
    }

    [JsonIgnore] public SourceList<string> Classpath { get; } = new();
    [JsonProperty("classpath")] private List<string> CpAsList { get; } = new();
    public string RootPath { get; set; }
    public string OutputDir { get; set; }
    public List<TabModel> Tabs { get; } = new();

    public static WorkspaceModel? Load(string path)
    {
        var ret = JsonConvert.DeserializeObject<WorkspaceModel>(File.ReadAllText(path));
        ret?.CpAsList.ToList().ForEach(i => ret.Classpath.Add(i));
        return ret;
    }

    public void Save(string path)
    {
        File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
    }
}