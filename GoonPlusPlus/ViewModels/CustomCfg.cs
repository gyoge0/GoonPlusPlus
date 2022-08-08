using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Avalonia.Logging;
using Avalonia.Media;
using GoonPlusPlus.Util.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GoonPlusPlus.ViewModels;

[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
[SuppressMessage("ReSharper", "StringLiteralTypo")]
public class CustomCfg
{
    [JsonIgnore] public static CustomCfg Instance { get; set; } = new();

    [JsonIgnore] public string GppAppDataPath { get; private set; } = null!;
    [JsonIgnore] public string ConfigFilePath { get; private set; } = null!;

    public void MakePaths()
    {
        GppAppDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Goon++"
        );
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

    public void SaveConfig() => File.WriteAllText(
        ConfigFilePath,
        JsonConvert.SerializeObject(
            this,
            Formatting.Indented,
            new CustomCfgSerializer()
        )
    );

    private CustomCfg()
    {
        MakePaths();
        LoadConfig();
        SaveConfig();
    }

    #region Editor

    [CustomCfgDefault("\"Transparent\"")] public IBrush? EditorBackground { get; set; } = Brushes.Transparent;

    [CustomCfgDefault("\"#ffbbbbbb\"")]
    public IBrush? EditorForeground { get; set; } = SolidColorBrush.Parse("#ffbbbbbb");

    [CustomCfgDefault("true")] public bool EditorShowLineNumbers { get; set; } = true;

    #region EditorFont

    [CustomCfgDefault("\"Jetbrains Mono,Courier New,Monospace\"")]
    public FontFamily EditorFontFamily { get; set; } = "Jetbrains Mono,Courier New,Monospace";

    [CustomCfgDefault("14")] public double EditorFontSize { get; set; } = 14;
    [CustomCfgDefault("\"Normal\"")] public FontStyle EditorFontStyle { get; set; } = FontStyle.Normal;
    [CustomCfgDefault("\"Light\"")] public FontWeight EditorFontWeight { get; set; } = FontWeight.Light;

    #endregion

    #endregion

    #region Compile

    [CustomCfgDefault("\"White\"")] public IBrush? CompileForeground { get; set; } = Brushes.White;

    #region CompileBackground

    [CustomCfgDefault("\"Transparent\"")] public IBrush? CompileBackground { get; set; } = Brushes.Transparent;

    #endregion

    #region CompileAccent

    [CustomCfgDefault("\"#ff0078d7\"")] public IBrush? CompileAccent { get; set; } = SolidColorBrush.Parse("#ff0078d7");

    #endregion

    #region CompileFont

    [CustomCfgDefault("\"Consolas\"")] public FontFamily CompileFontFamily { get; set; } = "Consolas";
    [CustomCfgDefault("14")] public double CompileFontSize { get; set; } = 14;
    [CustomCfgDefault("\"Normal\"")] public FontStyle CompileFontStyle { get; set; } = FontStyle.Normal;
    [CustomCfgDefault("\"Normal\"")] public FontWeight CompileFontWeight { get; set; } = FontWeight.Normal;

    #endregion

    #endregion

    #region Run

    [CustomCfgDefault("\"White\"")] public IBrush? RunForeground { get; set; } = Brushes.White;

    #region RunBackground

    [CustomCfgDefault("\"Transparent\"")] public IBrush? RunBackground { get; set; } = Brushes.Transparent;

    #endregion

    #region RunAccent

    [CustomCfgDefault("\"#ff0078d7\"")] public IBrush? RunAccent { get; set; } = SolidColorBrush.Parse("#ff0078d7");

    #endregion

    #region RunFont

    [CustomCfgDefault("\"Consolas\"")] public FontFamily RunFontFamily { get; set; } = "Consolas";
    [CustomCfgDefault("14")] public double RunFontSize { get; set; } = 14;
    [CustomCfgDefault("\"Normal\"")] public FontStyle RunFontStyle { get; set; } = FontStyle.Normal;
    [CustomCfgDefault("\"Normal\"")] public FontWeight RunFontWeight { get; set; } = FontWeight.Normal;

    #endregion

    #endregion

    #region FileExplorer

    #region FileExplorerButton

    [CustomCfgDefault("\"Transparent\"")]
    public IBrush? FileExplorerButtonBackground { get; set; } = Brushes.Transparent;

    [CustomCfgDefault("\"White\"")] public IBrush? FileExplorerButtonForeground { get; set; } = Brushes.White;

    [CustomCfgDefault("\"#ff0078d7\"")]
    public IBrush? FileExplorerButtonAccent { get; set; } = SolidColorBrush.Parse("#ff0078d7");

    #endregion

    #region FileExplorerButtonHover

    [CustomCfgDefault("\"DimGray\"")] public IBrush? FileExplorerButtonHoverBackground { get; set; } = Brushes.DimGray;

    [CustomCfgDefault("\"#ff0078d7\"")]
    public IBrush? FileExplorerButtonHoverAccent { get; set; } = SolidColorBrush.Parse("#ff0078d7");

    #endregion

    #region FileExplorerButtonPressed

    [CustomCfgDefault("\"Gray\"")] public IBrush? FileExplorerButtonPressedBackground { get; set; } = Brushes.Gray;
    [CustomCfgDefault("\"White\"")] public IBrush? FileExplorerButtonPressedForeground { get; set; } = Brushes.White;

    [CustomCfgDefault("\"#ff0078d7\"")]
    public IBrush? FileExplorerButtonPressedAccent { get; set; } = SolidColorBrush.Parse("#ff0078d7");

    #endregion

    #region FileExplorerItem

    [CustomCfgDefault("\"Transparent\"")] public IBrush? FileExplorerItemBackground { get; set; } = Brushes.Transparent;
    [CustomCfgDefault("\"White\"")] public IBrush? FileExplorerItemForeground { get; set; } = Brushes.White;

    #region FileExplorerItemFont

    [CustomCfgDefault("\"Segoe UI\"")] public FontFamily FileExplorerItemFontFamily { get; set; } = "Segoe UI";
    [CustomCfgDefault("18")] public double FileExplorerItemFontSize { get; set; } = 18;
    [CustomCfgDefault("\"Normal\"")] public FontStyle FileExplorerItemFontStyle { get; set; } = FontStyle.Normal;

    [CustomCfgDefault("\"ExtraLight\"")]
    public FontWeight FileExplorerItemFontWeight { get; set; } = FontWeight.ExtraLight;

    #endregion


    #region FileExplorerItemHover

    [CustomCfgDefault("\"DimGray\"")] public IBrush? FileExplorerItemHoverBackground { get; set; } = Brushes.DimGray;

    [CustomCfgDefault("\"White\"")] public IBrush? FileExplorerItemHoverForeground { get; set; } = Brushes.White;

    #region FileExplorerItemHoverFont

    [CustomCfgDefault("\"Segoe UI\"")] public FontFamily FileExplorerItemHoverFontFamily { get; set; } = "Segoe UI";
    [CustomCfgDefault("18")] public double FileExplorerItemHoverFontSize { get; set; } = 18;
    [CustomCfgDefault("\"Normal\"")] public FontStyle FileExplorerItemHoverFontStyle { get; set; } = FontStyle.Normal;

    [CustomCfgDefault("\"ExtraLight\"")]
    public FontWeight FileExplorerItemHoverFontWeight { get; set; } = FontWeight.ExtraLight;

    #endregion

    #endregion


    #region FileExplorerItemSelected

    [CustomCfgDefault("\"#ff0078d7\"")]
    public IBrush? FileExplorerItemSelectedBackground { get; set; } = SolidColorBrush.Parse("#ff0078d7");

    [CustomCfgDefault("\"White\"")] public IBrush? FileExplorerItemSelectedForeground { get; set; } = Brushes.White;

    #region FileExplorerItemSelectedFont

    [CustomCfgDefault("\"Segoe UI\"")] public FontFamily FileExplorerItemSelectedFontFamily { get; set; } = "Segoe UI";
    [CustomCfgDefault("18")] public double FileExplorerItemSelectedFontSize { get; set; } = 18;

    [CustomCfgDefault("\"Normal\"")]
    public FontStyle FileExplorerItemSelectedFontStyle { get; set; } = FontStyle.Normal;

    [CustomCfgDefault("\"ExtraLight\"")]
    public FontWeight FileExplorerItemSelectedFontWeight { get; set; } = FontWeight.ExtraLight;

    #endregion

    #endregion

    #endregion

    #endregion

    #region TabBar

    [CustomCfgDefault("\"Transparent\"")] public IBrush? TabBarBackground { get; set; } = Brushes.Transparent;

    #region TabBarHeader

    [CustomCfgDefault("\"Transparent\"")] public IBrush? TabBarHeaderBackground { get; set; } = Brushes.Transparent;

    #region TabBarHeaderFont

    [CustomCfgDefault("\"Transparent\"")] public IBrush? TabBarHeaderFontBackground { get; set; } = Brushes.Transparent;
    [CustomCfgDefault("\"White\"")] public IBrush? TabBarHeaderFontForeground { get; set; } = Brushes.White;

    [CustomCfgDefault("\"Segoe UI\"")] public FontFamily TabBarHeaderFontFamily { get; set; } = "Segoe UI";
    [CustomCfgDefault("28")] public double TabBarHeaderFontSize { get; set; } = 28;
    [CustomCfgDefault("\"Normal\"")] public FontStyle TabBarHeaderFontStyle { get; set; } = FontStyle.Normal;

    [CustomCfgDefault("\"ExtraLight\"")] public FontWeight TabBarHeaderFontWeight { get; set; } = FontWeight.ExtraLight;

    #endregion

    #region TabBarHeaderButton

    [CustomCfgDefault("\"Transparent\"")]
    public IBrush? TabBarHeaderButtonBackground { get; set; } = Brushes.Transparent;

    [CustomCfgDefault("\"White\"")] public IBrush? TabBarHeaderButtonForeground { get; set; } = Brushes.White;
    [CustomCfgDefault("28")] public double TabBarHeaderButtonSize { get; set; } = 28;

    [CustomCfgDefault("\"ExtraLight\"")]
    public FontWeight TabBarHeaderButtonWeight { get; set; } = FontWeight.ExtraLight;

    #region TabBarHeaderButtonHover

    [CustomCfgDefault("\"DimGray\"")] public IBrush? TabBarHeaderButtonHoverBackground { get; set; } = Brushes.DimGray;
    [CustomCfgDefault("\"White\"")] public IBrush? TabBarHeaderButtonHoverForeground { get; set; } = Brushes.White;
    [CustomCfgDefault("28")] public double TabBarHeaderButtonHoverSize { get; set; } = 28;

    [CustomCfgDefault("\"ExtraLight\"")]
    public FontWeight TabBarHeaderButtonHoverWeight { get; set; } = FontWeight.ExtraLight;

    #endregion

    #region TabBarHeaderButtonPressed

    [CustomCfgDefault("\"LightGray\"")]
    public IBrush? TabBarHeaderButtonPressedBackground { get; set; } = Brushes.LightGray;

    [CustomCfgDefault("\"White\"")] public IBrush? TabBarHeaderButtonPressedForeground { get; set; } = Brushes.White;
    [CustomCfgDefault("28")] public double TabBarHeaderButtonPressedSize { get; set; } = 28;

    [CustomCfgDefault("\"ExtraLight\"")]
    public FontWeight TabBarHeaderButtonPressedWeight { get; set; } = FontWeight.ExtraLight;

    #endregion

    #endregion

    #endregion

    #endregion
}