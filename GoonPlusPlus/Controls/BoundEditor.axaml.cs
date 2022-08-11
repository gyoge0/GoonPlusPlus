using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using GoonPlusPlus.Models;
using ReactiveUI;
using TextMateSharp.Grammars;

namespace GoonPlusPlus.Controls;

public partial class BoundEditor : UserControl
{
    private string _boundText = null!;
    private string? _extension;
    private readonly RegistryOptions _registryOptions = new(ThemeName.DarkPlus);

    public static readonly DirectProperty<BoundEditor, string> BoundTextProperty =
        AvaloniaProperty.RegisterDirect<BoundEditor, string>("BoundText", o => o.BoundText, (o, v) => o.BoundText = v);


    public static readonly DirectProperty<BoundEditor, string?> ExtensionProperty =
        AvaloniaProperty.RegisterDirect<BoundEditor, string?>("Extension", o => o.Extension, (o, v) => o.Extension = v);


    public TextEditor EditArea { get; }

    public BoundEditor()
    {
        InitializeComponent();
        TabBuffer.Instance.CurrentEditor = this;
        EditArea = this.FindControl<TextEditor>("Editor");

        var textMate = EditArea.InstallTextMate(_registryOptions);

        this.WhenAnyValue(x => x.Extension)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(x =>
            {
                if (x == null) return;
                try
                {
                    textMate.SetGrammar(
                        _registryOptions.GetScopeByLanguageId(_registryOptions.GetLanguageByExtension("." + x).Id)
                    );
                    Logger.TryGet(LogEventLevel.Debug, LogArea.Control)
                        ?.Log(this, $"Found language for extension {x}");
                }
                catch (NullReferenceException)
                {
                    Logger.TryGet(LogEventLevel.Error, LogArea.Control)
                        ?.Log(this, $"No language found for extension \" {x} \"");
                }
            });


        this.WhenAnyValue(x => x.BoundText)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(x =>
            {
                if (EditArea.Document.Text != x && x != null) EditArea.Document.Text = x;
            });


        EditArea.WhenAnyValue(x => x.Document.Text)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(x =>
            {
                if (BoundText != x) BoundText = x;
            });
    }


    public string BoundText
    {
        get => _boundText;
        set => SetAndRaise(BoundTextProperty, ref _boundText, value);
    }

    public string? Extension
    {
        get => _extension;
        set => SetAndRaise(ExtensionProperty, ref _extension, value);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}