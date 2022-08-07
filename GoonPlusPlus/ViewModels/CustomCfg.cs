using Avalonia.Media;
using Newtonsoft.Json;
using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class CustomCfg : ViewModelBase
{
    [JsonIgnore] public static CustomCfg Instance { get; } = new();

    private CustomCfg()
    {
    }

    #region Fields

    private IBrush? _editorBackground = Brushes.Transparent;
    private IBrush? _editorForeground = SolidColorBrush.Parse("#bbbbbb");
    private bool _editorShowLineNumbers = true;
    private FontFamily _editorFontFamily = "Jetbrains Mono,Courier New,Monospace";
    private double _editorFontSize = 14;
    private FontStyle _editorFontStyle = FontStyle.Normal;
    private FontWeight _editorFontWeight = FontWeight.Light;
    private IBrush? _compileForeground = Brushes.White;
    private IBrush? _compileBackground = Brushes.Transparent;
    private IBrush? _compileAccent = SolidColorBrush.Parse("#0078d7");
    private FontFamily _compileFontFamily = "Consolas";
    private double _compileFontSize = 14;
    private FontStyle _compileFontStyle = FontStyle.Normal;
    private FontWeight _compileFontWeight = FontWeight.Normal;
    private IBrush? _runForeground = Brushes.White;
    private IBrush? _runBackground = Brushes.Transparent;
    private IBrush? _runAccent = SolidColorBrush.Parse("#0078d7");
    private FontFamily _runFontFamily = "Consolas";
    private double _runFontSize = 14;
    private FontStyle _runFontStyle = FontStyle.Normal;
    private FontWeight _runFontWeight = FontWeight.Normal;
    private IBrush? _fileExplorerButtonBackground = Brushes.Transparent;
    private IBrush? _fileExplorerButtonForeground = Brushes.White;
    private IBrush? _fileExplorerButtonAccent = SolidColorBrush.Parse("#0078d7");
    private IBrush? _fileExplorerButtonHoverBackground = Brushes.DimGray;
    private IBrush? _fileExplorerButtonHoverAccent = SolidColorBrush.Parse("#0078d7");
    private IBrush? _fileExplorerButtonPressedBackground = Brushes.Gray;
    private IBrush? _fileExplorerButtonPressedForeground = Brushes.White;
    private IBrush? _fileExplorerButtonPressedAccent = SolidColorBrush.Parse("#0078d7");
    private IBrush? _fileExplorerItemBackground = Brushes.Transparent;
    private IBrush? _fileExplorerItemForeground = Brushes.White;
    private FontFamily _fileExplorerItemFontFamily = FontFamily.Default;
    private double _fileExplorerItemFontSize = 18;
    private FontStyle _fileExplorerItemFontStyle = FontStyle.Normal;
    private FontWeight _fileExplorerItemFontWeight = FontWeight.ExtraLight;
    private IBrush? _fileExplorerItemHoverForeground = Brushes.White;
    private FontFamily _fileExplorerItemHoverFontFamily = FontFamily.Default;
    private double _fileExplorerItemHoverFontSize = 18;
    private FontStyle _fileExplorerItemHoverFontStyle = FontStyle.Normal;
    private FontWeight _fileExplorerItemHoverFontWeight = FontWeight.ExtraLight;
    private IBrush? _fileExplorerItemSelectedBackground = SolidColorBrush.Parse("#0078d7");
    private IBrush? _fileExplorerItemSelectedForeground = Brushes.White;
    private FontFamily _fileExplorerItemSelectedFontFamily = FontFamily.Default;
    private double _fileExplorerItemSelectedFontSize = 18;
    private FontStyle _fileExplorerItemSelectedFontStyle = FontStyle.Normal;
    private FontWeight _fileExplorerItemSelectedFontWeight = FontWeight.ExtraLight;
    private IBrush? _tabBarBackground = Brushes.Transparent;
    private IBrush? _tabBarHeaderBackground = Brushes.Transparent;
    private IBrush? _tabBarHeaderFontBackground = Brushes.Transparent;
    private IBrush? _tabBarHeaderFontForeground = Brushes.White;
    private FontFamily _tabBarHeaderFontFamily = FontFamily.Default;
    private double _tabBarHeaderFontSize = 28;
    private FontStyle _tabBarHeaderFontStyle = FontStyle.Normal;
    private FontWeight _tabBarHeaderFontWeight = FontWeight.SemiLight;
    private IBrush? _tabBarHeaderButtonBackground = Brushes.Transparent;
    private IBrush? _tabBarHeaderButtonForeground = Brushes.White;
    private double _tabBarHeaderButtonSize = 28;
    private FontWeight _tabBarHeaderButtonWeight = FontWeight.SemiLight;
    private IBrush? _tabBarHeaderButtonHoverBackground = Brushes.DimGray;
    private IBrush? _tabBarHeaderButtonHoverForeground = Brushes.White;
    private double _tabBarHeaderButtonHoverSize = 28;
    private FontWeight _tabBarHeaderButtonHoverWeight = FontWeight.SemiLight;
    private IBrush? _tabBarHeaderButtonPressedBackground = Brushes.LightGray;
    private IBrush? _tabBarHeaderButtonPressedForeground = Brushes.White;
    private double _tabBarHeaderButtonPressedSize = 28;
    private FontWeight _tabBarHeaderButtonPressedWeight = FontWeight.SemiLight;

    #endregion

    #region Editor

    public IBrush? EditorBackground
    {
        get => _editorBackground;
        set => this.RaiseAndSetIfChanged(ref _editorBackground, value);
    }

    public IBrush? EditorForeground
    {
        get => _editorForeground;
        set => this.RaiseAndSetIfChanged(ref _editorForeground, value);
    }

    public bool EditorShowLineNumbers
    {
        get => _editorShowLineNumbers;
        set => this.RaiseAndSetIfChanged(ref _editorShowLineNumbers, value);
    }

    #region EditorFont

    public FontFamily EditorFontFamily
    {
        get => _editorFontFamily;
        set => this.RaiseAndSetIfChanged(ref _editorFontFamily, value);
    }

    public double EditorFontSize
    {
        get => _editorFontSize;
        set => this.RaiseAndSetIfChanged(ref _editorFontSize, value);
    }

    public FontStyle EditorFontStyle
    {
        get => _editorFontStyle;
        set => this.RaiseAndSetIfChanged(ref _editorFontStyle, value);
    }

    public FontWeight EditorFontWeight
    {
        get => _editorFontWeight;
        set => this.RaiseAndSetIfChanged(ref _editorFontWeight, value);
    }

    #endregion

    #endregion

    #region Compile

    public IBrush? CompileForeground
    {
        get => _compileForeground;
        set => this.RaiseAndSetIfChanged(ref _compileForeground, value);
    }

    #region CompileBackground

    public IBrush? CompileBackground
    {
        get => _compileBackground;
        set => this.RaiseAndSetIfChanged(ref _compileBackground, value);
    }

    #endregion

    #region CompileAccent

    public IBrush? CompileAccent
    {
        get => _compileAccent;
        set => this.RaiseAndSetIfChanged(ref _compileAccent, value);
    }

    #endregion

    #region CompileFont

    public FontFamily CompileFontFamily
    {
        get => _compileFontFamily;
        set => this.RaiseAndSetIfChanged(ref _compileFontFamily, value);
    }

    public double CompileFontSize
    {
        get => _compileFontSize;
        set => this.RaiseAndSetIfChanged(ref _compileFontSize, value);
    }

    public FontStyle CompileFontStyle
    {
        get => _compileFontStyle;
        set => this.RaiseAndSetIfChanged(ref _compileFontStyle, value);
    }

    public FontWeight CompileFontWeight
    {
        get => _compileFontWeight;
        set => this.RaiseAndSetIfChanged(ref _compileFontWeight, value);
    }

    #endregion

    #endregion

    #region Run

    public IBrush? RunForeground
    {
        get => _runForeground;
        set => this.RaiseAndSetIfChanged(ref _runForeground, value);
    }

    #region RunBackground

    public IBrush? RunBackground
    {
        get => _runBackground;
        set => this.RaiseAndSetIfChanged(ref _runBackground, value);
    }

    #endregion

    #region RunAccent

    public IBrush? RunAccent
    {
        get => _runAccent;
        set => this.RaiseAndSetIfChanged(ref _runAccent, value);
    }

    #endregion

    #region RunFont

    public FontFamily RunFontFamily
    {
        get => _runFontFamily;
        set => this.RaiseAndSetIfChanged(ref _runFontFamily, value);
    }

    public double RunFontSize
    {
        get => _runFontSize;
        set => this.RaiseAndSetIfChanged(ref _runFontSize, value);
    }

    public FontStyle RunFontStyle
    {
        get => _runFontStyle;
        set => this.RaiseAndSetIfChanged(ref _runFontStyle, value);
    }

    public FontWeight RunFontWeight
    {
        get => _runFontWeight;
        set => this.RaiseAndSetIfChanged(ref _runFontWeight, value);
    }

    #endregion

    #endregion

    #region FileExplorer

    #region FileExplorerButton

    public IBrush? FileExplorerButtonBackground
    {
        get => _fileExplorerButtonBackground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonBackground, value);
    }

    public IBrush? FileExplorerButtonForeground
    {
        get => _fileExplorerButtonForeground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonForeground, value);
    }

    public IBrush? FileExplorerButtonAccent
    {
        get => _fileExplorerButtonAccent;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonAccent, value);
    }

    #endregion

    #region FileExplorerButtonHover

    public IBrush? FileExplorerButtonHoverBackground
    {
        get => _fileExplorerButtonHoverBackground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonHoverBackground, value);
    }

    public IBrush? FileExplorerButtonHoverAccent
    {
        get => _fileExplorerButtonHoverAccent;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonHoverAccent, value);
    }

    #endregion

    #region FileExplorerButtonPressed

    public IBrush? FileExplorerButtonPressedBackground
    {
        get => _fileExplorerButtonPressedBackground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonPressedBackground, value);
    }

    public IBrush? FileExplorerButtonPressedForeground
    {
        get => _fileExplorerButtonPressedForeground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonPressedForeground, value);
    }

    public IBrush? FileExplorerButtonPressedAccent
    {
        get => _fileExplorerButtonPressedAccent;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonPressedAccent, value);
    }

    #endregion

    #region FileExplorerItem

    public IBrush? FileExplorerItemBackground
    {
        get => _fileExplorerItemBackground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemBackground, value);
    }

    public IBrush? FileExplorerItemForeground
    {
        get => _fileExplorerItemForeground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemForeground, value);
    }

    #region FileExplorerItemFont

    public FontFamily FileExplorerItemFontFamily
    {
        get => _fileExplorerItemFontFamily;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemFontFamily, value);
    }

    public double FileExplorerItemFontSize
    {
        get => _fileExplorerItemFontSize;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemFontSize, value);
    }

    public FontStyle FileExplorerItemFontStyle
    {
        get => _fileExplorerItemFontStyle;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemFontStyle, value);
    }

    public FontWeight FileExplorerItemFontWeight
    {
        get => _fileExplorerItemFontWeight;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemFontWeight, value);
    }

    #endregion


    #region FileExplorerItemHover

    public IBrush? FileExplorerItemHoverForeground
    {
        get => _fileExplorerItemHoverForeground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemHoverForeground, value);
    }

    #region FileExplorerItemHoverFont

    public FontFamily FileExplorerItemHoverFontFamily
    {
        get => _fileExplorerItemHoverFontFamily;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemHoverFontFamily, value);
    }

    public double FileExplorerItemHoverFontSize
    {
        get => _fileExplorerItemHoverFontSize;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemHoverFontSize, value);
    }

    public FontStyle FileExplorerItemHoverFontStyle
    {
        get => _fileExplorerItemHoverFontStyle;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemHoverFontStyle, value);
    }

    public FontWeight FileExplorerItemHoverFontWeight
    {
        get => _fileExplorerItemHoverFontWeight;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemHoverFontWeight, value);
    }

    #endregion

    #endregion


    #region FileExplorerItemSelected

    public IBrush? FileExplorerItemSelectedBackground
    {
        get => _fileExplorerItemSelectedBackground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemSelectedBackground, value);
    }

    public IBrush? FileExplorerItemSelectedForeground
    {
        get => _fileExplorerItemSelectedForeground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemSelectedForeground, value);
    }

    #region FileExplorerItemSelectedFont

    public FontFamily FileExplorerItemSelectedFontFamily
    {
        get => _fileExplorerItemSelectedFontFamily;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemSelectedFontFamily, value);
    }

    public double FileExplorerItemSelectedFontSize
    {
        get => _fileExplorerItemSelectedFontSize;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemSelectedFontSize, value);
    }

    public FontStyle FileExplorerItemSelectedFontStyle
    {
        get => _fileExplorerItemSelectedFontStyle;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemSelectedFontStyle, value);
    }

    public FontWeight FileExplorerItemSelectedFontWeight
    {
        get => _fileExplorerItemSelectedFontWeight;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemSelectedFontWeight, value);
    }

    #endregion

    #endregion

    #endregion

    #endregion

    #region TabBar

    public IBrush? TabBarBackground
    {
        get => _tabBarBackground;
        set => this.RaiseAndSetIfChanged(ref _tabBarBackground, value);
    }

    #region TabBarHeader

    public IBrush? TabBarHeaderBackground
    {
        get => _tabBarHeaderBackground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderBackground, value);
    }

    #region TabBarHeaderFont

    public IBrush? TabBarHeaderFontBackground
    {
        get => _tabBarHeaderFontBackground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderFontBackground, value);
    }

    public IBrush? TabBarHeaderFontForeground
    {
        get => _tabBarHeaderFontForeground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderFontForeground, value);
    }

    public FontFamily TabBarHeaderFontFamily
    {
        get => _tabBarHeaderFontFamily;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderFontFamily, value);
    }

    public double TabBarHeaderFontSize
    {
        get => _tabBarHeaderFontSize;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderFontSize, value);
    }

    public FontStyle TabBarHeaderFontStyle
    {
        get => _tabBarHeaderFontStyle;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderFontStyle, value);
    }

    public FontWeight TabBarHeaderFontWeight
    {
        get => _tabBarHeaderFontWeight;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderFontWeight, value);
    }

    #endregion

    #region TabBarHeaderButton

    public IBrush? TabBarHeaderButtonBackground
    {
        get => _tabBarHeaderButtonBackground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonBackground, value);
    }

    public IBrush? TabBarHeaderButtonForeground
    {
        get => _tabBarHeaderButtonForeground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonForeground, value);
    }

    public double TabBarHeaderButtonSize
    {
        get => _tabBarHeaderButtonSize;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonSize, value);
    }

    public FontWeight TabBarHeaderButtonWeight
    {
        get => _tabBarHeaderButtonWeight;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonWeight, value);
    }

    #region TabBarHeaderButtonHover

    public IBrush? TabBarHeaderButtonHoverBackground
    {
        get => _tabBarHeaderButtonHoverBackground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonHoverBackground, value);
    }

    public IBrush? TabBarHeaderButtonHoverForeground
    {
        get => _tabBarHeaderButtonHoverForeground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonHoverForeground, value);
    }

    public double TabBarHeaderButtonHoverSize
    {
        get => _tabBarHeaderButtonHoverSize;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonHoverSize, value);
    }

    public FontWeight TabBarHeaderButtonHoverWeight
    {
        get => _tabBarHeaderButtonHoverWeight;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonHoverWeight, value);
    }

    #endregion

    #region TabBarHeaderButtonPressed

    public IBrush? TabBarHeaderButtonPressedBackground
    {
        get => _tabBarHeaderButtonPressedBackground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonPressedBackground, value);
    }

    public IBrush? TabBarHeaderButtonPressedForeground
    {
        get => _tabBarHeaderButtonPressedForeground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonPressedForeground, value);
    }

    public double TabBarHeaderButtonPressedSize
    {
        get => _tabBarHeaderButtonPressedSize;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonPressedSize, value);
    }

    public FontWeight TabBarHeaderButtonPressedWeight
    {
        get => _tabBarHeaderButtonPressedWeight;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonPressedWeight, value);
    }

    #endregion

    #endregion

    #endregion

    #endregion
}