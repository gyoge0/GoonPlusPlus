using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using DynamicData;
using System;
using System.Text;

namespace GoonPlusPlus.CodeRunner.Run;

public partial class RunOutputBox : TextBox, IStyleable
{
    private int _accessCaretIndex;
    private bool _caretCanMove;
    private StringBuilder _editingBuilder = new();


    private StringBuilder _stdInBuilder = new();
    private int _vCaretIndex;

    public RunOutputBox()
    {
        InitializeComponent();
        _accessCaretIndex = CaretIndex;

        // TODO: Handle pasting
        PastingFromClipboard += (_, args) => args.Handled = true;
        CuttingToClipboard += (_, args) => args.Handled = true;
    }

    Type IStyleable.StyleKey => typeof(TextBox);

    protected override void OnKeyDown(KeyEventArgs e)
    {
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        if (!IsEnabled) return;

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (e.Key)
        {
            case Key.Back:
                e.Handled = true;
                if (VCaretIndex >= 1)
                {
                    // VCaretIndex points to space after char
                    VCaretIndex--;
                    AccessCaretIndex--;
                    // VCaretIndex points to index of char
                    EditingBuilder.Remove(VCaretIndex, 1);
                    Text = Text.Remove(AccessCaretIndex, 1);
                }

                break;
            case Key.Delete:
                e.Handled = true;
                // VCaretIndex is the index of the char after the caret
                if (VCaretIndex < EditingBuilder.Length)
                {
                    EditingBuilder.Remove(VCaretIndex, 1);
                    Text = Text.Remove(AccessCaretIndex, 1);
                }

                break;
            case Key.Return:
                e.Handled = true;

                EditingBuilder.Append('\n');
                StdInBuilder.Append(EditingBuilder);
                EditingBuilder.Clear();
                VCaretIndex = 0;

                Text ??= string.Empty;
                Text = Text.Insert(AccessCaretIndex, "\n");
                // AccessCaretIndex skips the \n
                AccessCaretIndex++;

                RunViewModel.Instance.StdIn.Add(StdInBuilder.ToString());
                StdInBuilder.Clear();

                break;
            case Key.Left:
                e.Handled = true;
                if (VCaretIndex >= 1)
                {
                    // VCaretIndex points to space after char
                    VCaretIndex--;
                    AccessCaretIndex--;
                }

                break;
            case Key.Right:
                e.Handled = true;
                // VCaretIndex is the index of the char after the caret
                if (VCaretIndex < EditingBuilder.Length)
                {
                    VCaretIndex++;
                    AccessCaretIndex++;
                }

                break;
            case Key.Home:
            case Key.End:
                e.Handled = true;
                break;
            default:
                e.Handled = false;
                break;
        }
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        VCaretIndex += e.Text?.Length ?? 0;
        AccessCaretIndex += e.Text?.Length ?? 0;
        EditingBuilder.Append(e.Text);
        base.OnTextInput(e);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    #region DirectProperties

    public StringBuilder StdInBuilder
    {
        get => _stdInBuilder;
        set => SetAndRaise(StdInBuilderProperty, ref _stdInBuilder, value);
    }

    public StringBuilder EditingBuilder
    {
        get => _editingBuilder;
        set => SetAndRaise(EditingBuilderProperty, ref _editingBuilder, value);
    }

    public int VCaretIndex
    {
        get => _vCaretIndex;
        set => SetAndRaise(VCaretIndexProperty, ref _vCaretIndex, value);
    }

    public int AccessCaretIndex
    {
        get => _accessCaretIndex;
        set
        {
            _caretCanMove = true;
            SetAndRaise(AccessCaretIndexProperty, ref _accessCaretIndex, value);
            CaretIndex = value;
            _caretCanMove = false;
        }
    }

    public bool CaretCanMove
    {
        get => _caretCanMove;
        set => SetAndRaise(CaretCanMoveProperty, ref _caretCanMove, value);
    }


    public static readonly DirectProperty<RunOutputBox, StringBuilder> StdInBuilderProperty =
        AvaloniaProperty.RegisterDirect<RunOutputBox, StringBuilder>("StdInBuilder",
            o => o.StdInBuilder,
            (o, v) => o.StdInBuilder = v);

    public static readonly DirectProperty<RunOutputBox, StringBuilder> EditingBuilderProperty =
        AvaloniaProperty.RegisterDirect<RunOutputBox, StringBuilder>("EditingBuilder",
            o => o.EditingBuilder,
            (o, v) => o.EditingBuilder = v);

    public static readonly DirectProperty<RunOutputBox, int> VCaretIndexProperty =
        AvaloniaProperty.RegisterDirect<RunOutputBox, int>("VCaretIndex",
            o => o.VCaretIndex,
            (o, v) => o.VCaretIndex = v);

    public static readonly DirectProperty<RunOutputBox, int> AccessCaretIndexProperty =
        AvaloniaProperty.RegisterDirect<RunOutputBox, int>("AccessCaretIndex",
            o => o.AccessCaretIndex,
            (o, v) => o.AccessCaretIndex = v);

    public static readonly DirectProperty<RunOutputBox, bool> CaretCanMoveProperty =
        AvaloniaProperty.RegisterDirect<RunOutputBox, bool>("CaretCanMove",
            o => o.CaretCanMove,
            (o, v) => o.CaretCanMove = v);

    #endregion
}
