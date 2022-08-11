using DynamicData;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;

namespace GoonPlusPlus.ViewModels;

public class RunViewModel : ViewModelBase
{
    public static readonly string Appdata =
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Goon++/";

    private int _accessCaretIndex;
    private bool _caretCanMove;
    private StringBuilder _editingBuilder = new();

    private Process? _runProcess;
    private StringBuilder _stdInBuilder = new();
    private string _text = string.Empty;
    private int _vCaretIndex;

    public RunViewModel()
    {
        Instance = this;

        this.WhenAnyValue(x => x.RunProcess)
            .WhereNotNull()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ =>
            {
                Text = string.Empty;
                VCaretIndex = 0;
                AccessCaretIndex = 0;
                StdIn.Clear();
            });

        Output.Connect()
            .OnItemAdded(l =>
            {
                // The process adds a null character when it terminates
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (l == null) return;

                StdInBuilder.Append(EditingBuilder);
                EditingBuilder.Clear();
                VCaretIndex = 0;
                // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
                Text ??= string.Empty;

                Text = Text.Insert(AccessCaretIndex, l);
                AccessCaretIndex += l.Length;
            })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe();

        StdIn.Connect()
            .OnItemAdded(l =>
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (l == null) return;
                if (RunProcess == null) return;
                RunProcess.StandardInput.WriteLine(l);
            })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe();
    }

    public static RunViewModel Instance { get; private set; } = null!;

    #region ReactiveProperties

    public Process? RunProcess
    {
        get => _runProcess;
        set => this.RaiseAndSetIfChanged(ref _runProcess, value);
    }

    public StringBuilder StdInBuilder
    {
        get => _stdInBuilder;
        set => this.RaiseAndSetIfChanged(ref _stdInBuilder, value);
    }

    public StringBuilder EditingBuilder
    {
        get => _editingBuilder;
        set => this.RaiseAndSetIfChanged(ref _editingBuilder, value);
    }

    public int VCaretIndex
    {
        get => _vCaretIndex;
        set => this.RaiseAndSetIfChanged(ref _vCaretIndex, value);
    }

    public int AccessCaretIndex
    {
        get => _accessCaretIndex;
        set => this.RaiseAndSetIfChanged(ref _accessCaretIndex, value);
    }

    public bool CaretCanMove
    {
        get => _caretCanMove;
        set => this.RaiseAndSetIfChanged(ref _caretCanMove, value);
    }

    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }

    #endregion


    #region StandardBuffers

    public SourceList<string> Output { get; } = new();
    public SourceList<string> StdIn { get; } = new();

    #endregion
}
