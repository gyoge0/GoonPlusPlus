using Avalonia.Logging;
using Avalonia.Media;
using GoonPlusPlus.Util.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReactiveUI;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace GoonPlusPlus.ViewModels;

[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
[SuppressMessage("ReSharper", "StringLiteralTypo")]
public class CustomCfg : ViewModelBase
{
    private CustomCfg()
    {
        MakePaths();
        LoadConfig();
        SaveConfig();
    }

    [JsonIgnore] public static CustomCfg Instance { get; set; } = new();

    [JsonIgnore] public string GppAppDataPath { get; private set; } = null!;
    [JsonIgnore] public string ConfigFilePath { get; private set; } = null!;

    public void MakePaths()
    {
        GppAppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Goon++");
        ConfigFilePath = Path.Combine(GppAppDataPath, "config.json");

        if (!Directory.Exists(GppAppDataPath)) Directory.CreateDirectory(GppAppDataPath);
        // ReSharper disable once InvertIf
        if (!File.Exists(ConfigFilePath))
        {
            using var writer = File.CreateText(ConfigFilePath);
            writer.WriteLine("{}");
        }
    }

    public void LoadConfig()
    {
        try
        {
            JsonConvert.PopulateObject(File.ReadAllText(ConfigFilePath), this);
        }
        catch (JsonSerializationException)
        {
            Logger.TryGet(LogEventLevel.Warning, LogArea.Binding)?.Log(this, "Invalid JSON config found");
        }
    }

    public void SaveConfig() => File.WriteAllText(ConfigFilePath,
        JsonConvert.SerializeObject(this, Formatting.Indented, new CustomCfgSerializer()));

    #region BackingFields

    private IBrush? _editorBackground = Brushes.Transparent;
    private IBrush? _editorForeground = SolidColorBrush.Parse("#ffbbbbbb");
    private bool _editorShowLineNumbers = true;
    private FontFamily _editorFontFamily = "Jetbrains Mono,Courier New,Monospace";
    private double _editorFontSize = 14;
    private FontStyle _editorFontStyle = FontStyle.Normal;
    private FontWeight _editorFontWeight = FontWeight.Light;
    private IBrush? _compileForeground = Brushes.White;
    private IBrush? _compileBackground = Brushes.Transparent;
    private IBrush? _compileAccent = SolidColorBrush.Parse("#ff0078d7");
    private FontFamily _compileFontFamily = "Consolas";
    private double _compileFontSize = 14;
    private FontStyle _compileFontStyle = FontStyle.Normal;
    private FontWeight _compileFontWeight = FontWeight.Normal;
    private IBrush? _runForeground = Brushes.White;
    private IBrush? _runBackground = Brushes.Transparent;
    private IBrush? _runAccent = SolidColorBrush.Parse("#ff0078d7");
    private FontFamily _runFontFamily = "Consolas";
    private double _runFontSize = 14;
    private FontStyle _runFontStyle = FontStyle.Normal;
    private FontWeight _runFontWeight = FontWeight.Normal;
    private IBrush? _fileExplorerButtonBackground = Brushes.Transparent;
    private IBrush? _fileExplorerButtonForeground = Brushes.White;
    private IBrush? _fileExplorerButtonAccent = SolidColorBrush.Parse("#ff0078d7");
    private IBrush? _fileExplorerButtonHoverBackground = Brushes.DimGray;
    private IBrush? _fileExplorerButtonHoverAccent = SolidColorBrush.Parse("#ff0078d7");
    private IBrush? _fileExplorerButtonPressedBackground = Brushes.Gray;
    private IBrush? _fileExplorerButtonPressedForeground = Brushes.White;
    private IBrush? _fileExplorerButtonPressedAccent = SolidColorBrush.Parse("#ff0078d7");
    private IBrush? _fileExplorerItemBackground = Brushes.Transparent;
    private IBrush? _fileExplorerItemForeground = Brushes.White;
    private FontFamily _fileExplorerItemFontFamily = "Segoe UI";
    private double _fileExplorerItemFontSize = 18;
    private FontStyle _fileExplorerItemFontStyle = FontStyle.Normal;
    private FontWeight _fileExplorerItemFontWeight = FontWeight.ExtraLight;
    private IBrush? _fileExplorerItemHoverBackground = Brushes.DimGray;
    private IBrush? _fileExplorerItemHoverForeground = Brushes.White;
    private FontFamily _fileExplorerItemHoverFontFamily = "Segoe UI";
    private double _fileExplorerItemHoverFontSize = 18;
    private FontStyle _fileExplorerItemHoverFontStyle = FontStyle.Normal;
    private FontWeight _fileExplorerItemHoverFontWeight = FontWeight.ExtraLight;
    private IBrush? _fileExplorerItemSelectedBackground = SolidColorBrush.Parse("#ff0078d7");
    private IBrush? _fileExplorerItemSelectedForeground = Brushes.White;
    private FontFamily _fileExplorerItemSelectedFontFamily = "Segoe UI";
    private double _fileExplorerItemSelectedFontSize = 18;
    private FontStyle _fileExplorerItemSelectedFontStyle = FontStyle.Normal;
    private FontWeight _fileExplorerItemSelectedFontWeight = FontWeight.ExtraLight;
    private IBrush? _tabBarBackground = Brushes.Transparent;
    private IBrush? _tabBarHeaderBackground = Brushes.Transparent;
    private IBrush? _tabBarHeaderFontBackground = Brushes.Transparent;
    private IBrush? _tabBarHeaderFontForeground = Brushes.White;
    private FontFamily _tabBarHeaderFontFamily = "Segoe UI";
    private double _tabBarHeaderFontSize = 28;
    private FontStyle _tabBarHeaderFontStyle = FontStyle.Normal;
    private FontWeight _tabBarHeaderFontWeight = FontWeight.ExtraLight;
    private IBrush? _tabBarHeaderButtonBackground = Brushes.Transparent;
    private IBrush? _tabBarHeaderButtonForeground = Brushes.White;
    private double _tabBarHeaderButtonSize = 28;
    private FontWeight _tabBarHeaderButtonWeight = FontWeight.ExtraLight;
    private IBrush? _tabBarHeaderButtonHoverBackground = Brushes.DimGray;
    private IBrush? _tabBarHeaderButtonHoverForeground = Brushes.White;
    private double _tabBarHeaderButtonHoverSize = 28;
    private FontWeight _tabBarHeaderButtonHoverWeight = FontWeight.ExtraLight;
    private IBrush? _tabBarHeaderButtonPressedBackground = Brushes.LightGray;
    private IBrush? _tabBarHeaderButtonPressedForeground = Brushes.White;
    private double _tabBarHeaderButtonPressedSize = 28;
    private FontWeight _tabBarHeaderButtonPressedWeight = FontWeight.ExtraLight;

    #endregion

    #region Editor

    [CustomCfgDefault("\"Transparent\"")]
    public IBrush? EditorBackground
    {
        get => _editorBackground;
        set => this.RaiseAndSetIfChanged(ref _editorBackground, value);
    }

    [CustomCfgDefault("\"#ffbbbbbb\"")]
    public IBrush? EditorForeground
    {
        get => _editorForeground;
        set => this.RaiseAndSetIfChanged(ref _editorForeground, value);
    }

    [CustomCfgDefault("true")]
    public bool EditorShowLineNumbers
    {
        get => _editorShowLineNumbers;
        set => this.RaiseAndSetIfChanged(ref _editorShowLineNumbers, value);
    }

    #region EditorFont

    [CustomCfgDefault("\"Jetbrains Mono,Courier New,Monospace\"")]
    public FontFamily EditorFontFamily
    {
        get => _editorFontFamily;
        set => this.RaiseAndSetIfChanged(ref _editorFontFamily, value);
    }

    [CustomCfgDefault("14")]
    public double EditorFontSize
    {
        get => _editorFontSize;
        set => this.RaiseAndSetIfChanged(ref _editorFontSize, value);
    }

    [CustomCfgDefault("\"Normal\"")]
    public FontStyle EditorFontStyle
    {
        get => _editorFontStyle;
        set => this.RaiseAndSetIfChanged(ref _editorFontStyle, value);
    }

    [CustomCfgDefault("\"Light\"")]
    public FontWeight EditorFontWeight
    {
        get => _editorFontWeight;
        set => this.RaiseAndSetIfChanged(ref _editorFontWeight, value);
    }

    #endregion

    #endregion

    #region Compile

    [CustomCfgDefault("\"White\"")]
    public IBrush? CompileForeground
    {
        get => _compileForeground;
        set => this.RaiseAndSetIfChanged(ref _compileForeground, value);
    }

    #region CompileBackground

    [CustomCfgDefault("\"Transparent\"")]
    public IBrush? CompileBackground
    {
        get => _compileBackground;
        set => this.RaiseAndSetIfChanged(ref _compileBackground, value);
    }

    #endregion

    #region CompileAccent

    [CustomCfgDefault("\"#ff0078d7\"")]
    public IBrush? CompileAccent
    {
        get => _compileAccent;
        set => this.RaiseAndSetIfChanged(ref _compileAccent, value);
    }

    #endregion

    #region CompileFont

    [CustomCfgDefault("\"Consolas\"")]
    public FontFamily CompileFontFamily
    {
        get => _compileFontFamily;
        set => this.RaiseAndSetIfChanged(ref _compileFontFamily, value);
    }

    [CustomCfgDefault("14")]
    public double CompileFontSize
    {
        get => _compileFontSize;
        set => this.RaiseAndSetIfChanged(ref _compileFontSize, value);
    }

    [CustomCfgDefault("\"Normal\"")]
    public FontStyle CompileFontStyle
    {
        get => _compileFontStyle;
        set => this.RaiseAndSetIfChanged(ref _compileFontStyle, value);
    }

    [CustomCfgDefault("\"Normal\"")]
    public FontWeight CompileFontWeight
    {
        get => _compileFontWeight;
        set => this.RaiseAndSetIfChanged(ref _compileFontWeight, value);
    }

    #endregion

    #endregion

    #region Run

    [CustomCfgDefault("\"White\"")]
    public IBrush? RunForeground
    {
        get => _runForeground;
        set => this.RaiseAndSetIfChanged(ref _runForeground, value);
    }

    #region RunBackground

    [CustomCfgDefault("\"Transparent\"")]
    public IBrush? RunBackground
    {
        get => _runBackground;
        set => this.RaiseAndSetIfChanged(ref _runBackground, value);
    }

    #endregion

    #region RunAccent

    [CustomCfgDefault("\"#ff0078d7\"")]
    public IBrush? RunAccent
    {
        get => _runAccent;
        set => this.RaiseAndSetIfChanged(ref _runAccent, value);
    }

    #endregion

    #region RunFont

    [CustomCfgDefault("\"Consolas\"")]
    public FontFamily RunFontFamily
    {
        get => _runFontFamily;
        set => this.RaiseAndSetIfChanged(ref _runFontFamily, value);
    }

    [CustomCfgDefault("14")]
    public double RunFontSize
    {
        get => _runFontSize;
        set => this.RaiseAndSetIfChanged(ref _runFontSize, value);
    }

    [CustomCfgDefault("\"Normal\"")]
    public FontStyle RunFontStyle
    {
        get => _runFontStyle;
        set => this.RaiseAndSetIfChanged(ref _runFontStyle, value);
    }

    [CustomCfgDefault("\"Normal\"")]
    public FontWeight RunFontWeight
    {
        get => _runFontWeight;
        set => this.RaiseAndSetIfChanged(ref _runFontWeight, value);
    }

    #endregion

    #endregion

    #region FileExplorer

    #region FileExplorerButton

    [CustomCfgDefault("\"Transparent\"")]
    public IBrush? FileExplorerButtonBackground
    {
        get => _fileExplorerButtonBackground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonBackground, value);
    }

    [CustomCfgDefault("\"White\"")]
    public IBrush? FileExplorerButtonForeground
    {
        get => _fileExplorerButtonForeground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonForeground, value);
    }

    [CustomCfgDefault("\"#ff0078d7\"")]
    public IBrush? FileExplorerButtonAccent
    {
        get => _fileExplorerButtonAccent;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonAccent, value);
    }

    #endregion

    #region FileExplorerButtonHover

    [CustomCfgDefault("\"DimGray\"")]
    public IBrush? FileExplorerButtonHoverBackground
    {
        get => _fileExplorerButtonHoverBackground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonHoverBackground, value);
    }

    [CustomCfgDefault("\"#ff0078d7\"")]
    public IBrush? FileExplorerButtonHoverAccent
    {
        get => _fileExplorerButtonHoverAccent;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonHoverAccent, value);
    }

    #endregion

    #region FileExplorerButtonPressed

    [CustomCfgDefault("\"Gray\"")]
    public IBrush? FileExplorerButtonPressedBackground
    {
        get => _fileExplorerButtonPressedBackground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonPressedBackground, value);
    }

    [CustomCfgDefault("\"White\"")]
    public IBrush? FileExplorerButtonPressedForeground
    {
        get => _fileExplorerButtonPressedForeground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonPressedForeground, value);
    }

    [CustomCfgDefault("\"#ff0078d7\"")]
    public IBrush? FileExplorerButtonPressedAccent
    {
        get => _fileExplorerButtonPressedAccent;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerButtonPressedAccent, value);
    }

    #endregion

    #region FileExplorerItem

    [CustomCfgDefault("\"Transparent\"")]
    public IBrush? FileExplorerItemBackground
    {
        get => _fileExplorerItemBackground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemBackground, value);
    }

    [CustomCfgDefault("\"White\"")]
    public IBrush? FileExplorerItemForeground
    {
        get => _fileExplorerItemForeground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemForeground, value);
    }

    #region FileExplorerItemFont

    [CustomCfgDefault("\"Segoe UI\"")]
    public FontFamily FileExplorerItemFontFamily
    {
        get => _fileExplorerItemFontFamily;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemFontFamily, value);
    }

    [CustomCfgDefault("18")]
    public double FileExplorerItemFontSize
    {
        get => _fileExplorerItemFontSize;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemFontSize, value);
    }

    [CustomCfgDefault("\"Normal\"")]
    public FontStyle FileExplorerItemFontStyle
    {
        get => _fileExplorerItemFontStyle;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemFontStyle, value);
    }

    [CustomCfgDefault("\"ExtraLight\"")]
    public FontWeight FileExplorerItemFontWeight
    {
        get => _fileExplorerItemFontWeight;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemFontWeight, value);
    }

    #endregion


    #region FileExplorerItemHover

    [CustomCfgDefault("\"DimGray\"")]
    public IBrush? FileExplorerItemHoverBackground
    {
        get => _fileExplorerItemHoverBackground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemHoverBackground, value);
    }

    [CustomCfgDefault("\"White\"")]
    public IBrush? FileExplorerItemHoverForeground
    {
        get => _fileExplorerItemHoverForeground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemHoverForeground, value);
    }

    #region FileExplorerItemHoverFont

    [CustomCfgDefault("\"Segoe UI\"")]
    public FontFamily FileExplorerItemHoverFontFamily
    {
        get => _fileExplorerItemHoverFontFamily;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemHoverFontFamily, value);
    }

    [CustomCfgDefault("18")]
    public double FileExplorerItemHoverFontSize
    {
        get => _fileExplorerItemHoverFontSize;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemHoverFontSize, value);
    }

    [CustomCfgDefault("\"Normal\"")]
    public FontStyle FileExplorerItemHoverFontStyle
    {
        get => _fileExplorerItemHoverFontStyle;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemHoverFontStyle, value);
    }

    [CustomCfgDefault("\"ExtraLight\"")]
    public FontWeight FileExplorerItemHoverFontWeight
    {
        get => _fileExplorerItemHoverFontWeight;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemHoverFontWeight, value);
    }

    #endregion

    #endregion


    #region FileExplorerItemSelected

    [CustomCfgDefault("\"#ff0078d7\"")]
    public IBrush? FileExplorerItemSelectedBackground
    {
        get => _fileExplorerItemSelectedBackground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemSelectedBackground, value);
    }

    [CustomCfgDefault("\"White\"")]
    public IBrush? FileExplorerItemSelectedForeground
    {
        get => _fileExplorerItemSelectedForeground;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemSelectedForeground, value);
    }

    #region FileExplorerItemSelectedFont

    [CustomCfgDefault("\"Segoe UI\"")]
    public FontFamily FileExplorerItemSelectedFontFamily
    {
        get => _fileExplorerItemSelectedFontFamily;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemSelectedFontFamily, value);
    }

    [CustomCfgDefault("18")]
    public double FileExplorerItemSelectedFontSize
    {
        get => _fileExplorerItemSelectedFontSize;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemSelectedFontSize, value);
    }

    [CustomCfgDefault("\"Normal\"")]
    public FontStyle FileExplorerItemSelectedFontStyle
    {
        get => _fileExplorerItemSelectedFontStyle;
        set => this.RaiseAndSetIfChanged(ref _fileExplorerItemSelectedFontStyle, value);
    }

    [CustomCfgDefault("\"ExtraLight\"")]
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

    [CustomCfgDefault("\"Transparent\"")]
    public IBrush? TabBarBackground
    {
        get => _tabBarBackground;
        set => this.RaiseAndSetIfChanged(ref _tabBarBackground, value);
    }

    #region TabBarHeader

    [CustomCfgDefault("\"Transparent\"")]
    public IBrush? TabBarHeaderBackground
    {
        get => _tabBarHeaderBackground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderBackground, value);
    }

    #region TabBarHeaderFont

    [CustomCfgDefault("\"Transparent\"")]
    public IBrush? TabBarHeaderFontBackground
    {
        get => _tabBarHeaderFontBackground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderFontBackground, value);
    }

    [CustomCfgDefault("\"White\"")]
    public IBrush? TabBarHeaderFontForeground
    {
        get => _tabBarHeaderFontForeground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderFontForeground, value);
    }

    [CustomCfgDefault("\"Segoe UI\"")]
    public FontFamily TabBarHeaderFontFamily
    {
        get => _tabBarHeaderFontFamily;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderFontFamily, value);
    }

    [CustomCfgDefault("28")]
    public double TabBarHeaderFontSize
    {
        get => _tabBarHeaderFontSize;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderFontSize, value);
    }

    [CustomCfgDefault("\"Normal\"")]
    public FontStyle TabBarHeaderFontStyle
    {
        get => _tabBarHeaderFontStyle;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderFontStyle, value);
    }

    [CustomCfgDefault("\"ExtraLight\"")]
    public FontWeight TabBarHeaderFontWeight
    {
        get => _tabBarHeaderFontWeight;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderFontWeight, value);
    }

    #endregion

    #region TabBarHeaderButton

    [CustomCfgDefault("\"Transparent\"")]
    public IBrush? TabBarHeaderButtonBackground
    {
        get => _tabBarHeaderButtonBackground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonBackground, value);
    }

    [CustomCfgDefault("\"White\"")]
    public IBrush? TabBarHeaderButtonForeground
    {
        get => _tabBarHeaderButtonForeground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonForeground, value);
    }

    [CustomCfgDefault("28")]
    public double TabBarHeaderButtonSize
    {
        get => _tabBarHeaderButtonSize;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonSize, value);
    }

    [CustomCfgDefault("\"ExtraLight\"")]
    public FontWeight TabBarHeaderButtonWeight
    {
        get => _tabBarHeaderButtonWeight;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonWeight, value);
    }

    #region TabBarHeaderButtonHover

    [CustomCfgDefault("\"DimGray\"")]
    public IBrush? TabBarHeaderButtonHoverBackground
    {
        get => _tabBarHeaderButtonHoverBackground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonHoverBackground, value);
    }

    [CustomCfgDefault("\"White\"")]
    public IBrush? TabBarHeaderButtonHoverForeground
    {
        get => _tabBarHeaderButtonHoverForeground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonHoverForeground, value);
    }

    [CustomCfgDefault("28")]
    public double TabBarHeaderButtonHoverSize
    {
        get => _tabBarHeaderButtonHoverSize;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonHoverSize, value);
    }

    [CustomCfgDefault("\"ExtraLight\"")]
    public FontWeight TabBarHeaderButtonHoverWeight
    {
        get => _tabBarHeaderButtonHoverWeight;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonHoverWeight, value);
    }

    #endregion

    #region TabBarHeaderButtonPressed

    [CustomCfgDefault("\"LightGray\"")]
    public IBrush? TabBarHeaderButtonPressedBackground
    {
        get => _tabBarHeaderButtonPressedBackground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonPressedBackground, value);
    }

    [CustomCfgDefault("\"White\"")]
    public IBrush? TabBarHeaderButtonPressedForeground
    {
        get => _tabBarHeaderButtonPressedForeground;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonPressedForeground, value);
    }

    [CustomCfgDefault("28")]
    public double TabBarHeaderButtonPressedSize
    {
        get => _tabBarHeaderButtonPressedSize;
        set => this.RaiseAndSetIfChanged(ref _tabBarHeaderButtonPressedSize, value);
    }

    [CustomCfgDefault("\"ExtraLight\"")]
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
