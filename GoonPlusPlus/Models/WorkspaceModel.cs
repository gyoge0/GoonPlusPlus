using System.Collections.Generic;
using System.IO;
using GoonPlusPlus.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GoonPlusPlus.Models;

[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class WorkspaceModel
{
    public List<string> Classpath { get; } = new();
    public string RootPath { get; set; }
    public string OutputDir { get; set; }
    public List<TabModel> Tabs { get; } = new();

    public WorkspaceModel(string rootPath)
    {
        RootPath = rootPath;
        OutputDir = rootPath;
    }

    public static WorkspaceModel? Load(string path) =>
        JsonConvert.DeserializeObject<WorkspaceModel>(File.ReadAllText(path));

    public void Save(string path) => File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
}