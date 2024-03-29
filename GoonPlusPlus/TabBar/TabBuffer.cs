﻿using DynamicData;
using ReactiveUI;
using System.Linq;

namespace GoonPlusPlus.TabBar;

public class TabBuffer : ReactiveObject
{
    private TabModel? _currentTab;
    public static TabBuffer Instance { get; } = new();

    public TabModel? CurrentTab
    {
        get => _currentTab;
        set => this.RaiseAndSetIfChanged(ref _currentTab, value);
    }

    public BoundEditor.BoundEditor? CurrentEditor { get; set; }

    public SourceList<TabModel> Buffer { get; } = new();

    public void AddTabs(params string[] paths) => paths.ToList().ConvertAll(p => new TabModel(p)).ForEach(Buffer.Add);

    public void AddTabs(params TabModel[] models) => models.ToList().ForEach(Buffer.Add);

    public void RemoveTabs(params TabModel[] models)
    {
        // SourceCache won't remove untitled items if their content is changed from ""
        models.Where(m => m.IsUntitled).ToList().ForEach(m => m.Content = "");
        Buffer.RemoveMany(models);
    }

    public int NumUntitled()
    {
        var tabs = Instance.Buffer.Items.Where(k => k.IsUntitled)
            .Select(k => k.Name.Split(" ").Last())
            .Select(int.Parse)
            .ToArray();
        return tabs.Any() ? tabs.MaxBy(x => x) : 0;
    }
}
