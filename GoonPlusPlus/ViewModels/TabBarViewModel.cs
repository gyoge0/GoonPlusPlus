using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using GoonPlusPlus.Models;
using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class TabBarViewModel : ViewModelBase
{
    private readonly ReadOnlyObservableCollection<TabModel> _tabs;
    private int _currentTabMirror;

    public TabBarViewModel()
    {
        TabBuffer.Instance
            .Buffer
            .Connect()
            .Bind(out _tabs)
            .OnItemRemoved(f =>
            {
                if (f != TabBuffer.Instance.CurrentTab) return;
                CurrentTabMirror = _tabs.Count - 1;
                TabBuffer.Instance.CurrentTab = _tabs.Count > 0 ? _tabs.Last() : null;
            })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe();

        WorkspaceViewModel.Instantiated += sender => (sender as WorkspaceViewModel)
            .WhenAnyValue(x => x!.Workspace)
            .WhereNotNull()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(p =>
            {
                TabBuffer.Instance
                    .Buffer
                    .Clear();
                TabBuffer.Instance
                    .AddTabs(p.Tabs.ToArray());
            });
    }

    public ReadOnlyObservableCollection<TabModel> Tabs => _tabs;


    public int CurrentTabMirror
    {
        get => _currentTabMirror;
        set
        {
            _currentTabMirror = value;
            // no tabs open
            if (_tabs.Count <= value || value < 0) return;
            TabBuffer.Instance
                .CurrentTab = _tabs[value];
        }
    }
}