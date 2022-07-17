using System.Linq;
using DynamicData;
using GoonPlusPlus.Controls;
using GoonPlusPlus.ViewModels;

namespace GoonPlusPlus.Models;

public class TabBuffer
{
    public static TabBuffer Instance { get; } = new();
    public TabModel? CurrentTab { get; set; }
    public BoundEditor? CurrentEditor { get; set; }

    public SourceList<TabModel> Buffer { get; } = new();

    public void AddTabs(params string[] paths) => paths
        .ToList()
        .ConvertAll(p => new TabModel(p))
        .ForEach(Buffer.Add);

    public void AddTabs(params TabModel[] models) => models
        .ToList()
        .ForEach(Buffer.Add);

    public void RemoveTabs(params TabModel[] models)
    {
        // SourceCache won't remove untitled items if their content is changed from ""
        models
            .Where(m => m.IsUntitled)
            .ToList()
            .ForEach(m => m.Content = "");
        Buffer.RemoveMany(models);
    }
}