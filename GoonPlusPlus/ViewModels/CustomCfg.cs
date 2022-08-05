using Avalonia.Media;

namespace GoonPlusPlus.ViewModels;

public class CustomCfg
{
    public static CustomCfg Instance { get; } = new();

    private CustomCfg()
    {
    }

    #region Editor

    public IBrush? EditorBackground { get; } = Brushes.Transparent;
    public IBrush? EditorForeground { get; } = SolidColorBrush.Parse("#bbbbbb");
    public bool EditorShowLineNumbers { get; } = true;

    #region EditorFont

    public FontFamily EditorFontFamily { get; } = "Jetbrains Mono,Courier New,Monospace";
    public double EditorFontSize { get; } = 14;
    public FontStyle EditorFontStyle { get; } = FontStyle.Normal;
    public FontWeight EditorFontWeight { get; } = FontWeight.Light;

    #endregion

    #endregion

    #region Compile

    public IBrush? CompileForeground { get; } = Brushes.White;

    #region CompileBackground

    public IBrush? CompileBackground { get; } = Brushes.Transparent;
    public IBrush? CompileHoverBackground { get; } = Brushes.Transparent;
    public IBrush? CompileFocusBackground { get; } = Brushes.Transparent;

    #endregion

    #region CompileAccent

    public IBrush? CompileAccent { get; } = SolidColorBrush.Parse("#0078d7");
    public IBrush? CompileHoverAccent { get; } = SolidColorBrush.Parse("#0078d7");
    public IBrush? CompileFocusAccent { get; } = SolidColorBrush.Parse("#0078d7");

    #endregion

    #region CompileFont

    public FontFamily CompileFontFamily { get; } = "Consolas";
    public double CompileFontSize { get; } = 14;
    public FontStyle CompileFontStyle { get; } = FontStyle.Normal;
    public FontWeight CompileFontWeight { get; } = FontWeight.Normal;

    #endregion

    #endregion

    #region Run

    public IBrush? RunForeground { get; } = Brushes.White;

    #region RunBackground

    public IBrush? RunBackground { get; } = Brushes.Transparent;
    public IBrush? RunHoverBackground { get; } = Brushes.Transparent;
    public IBrush? RunFocusBackground { get; } = Brushes.Transparent;

    #endregion

    #region RunAccent

    public IBrush? RunAccent { get; } = SolidColorBrush.Parse("#0078d7");
    public IBrush? RunHoverAccent { get; } = SolidColorBrush.Parse("#0078d7");
    public IBrush? RunFocusAccent { get; } = SolidColorBrush.Parse("#0078d7");

    #endregion

    #region RunFont

    public FontFamily RunFontFamily { get; } = "Consolas";
    public double RunFontSize { get; } = 14;
    public FontStyle RunFontStyle { get; } = FontStyle.Normal;
    public FontWeight RunFontWeight { get; } = FontWeight.Normal;

    #endregion

    #endregion

    #region FileExplorer

    #region FileExplorerButton

    public IBrush? FileExplorerButtonBackground { get; } = Brushes.Transparent;
    public IBrush? FileExplorerButtonForeground { get; } = Brushes.White;
    public IBrush? FileExplorerButtonAccent { get; } = SolidColorBrush.Parse("#0078d7");

    #endregion

    #region FileExplorerButtonHover

    public IBrush? FileExplorerButtonHoverBackground { get; } = Brushes.DimGray;
    public IBrush? FileExplorerButtonHoverAccent { get; } = SolidColorBrush.Parse("#0078d7");

    #endregion

    #region FileExplorerButtonPressed

    public IBrush? FileExplorerButtonPressedBackground { get; } = Brushes.Gray;
    public IBrush? FileExplorerButtonPressedForeground { get; } = Brushes.White;
    public IBrush? FileExplorerButtonPressedAccent { get; } = SolidColorBrush.Parse("#0078d7");

    #endregion

    #region FileExplorerItem

    public IBrush? FileExplorerItemBackground { get; } = Brushes.Transparent;
    public IBrush? FileExplorerItemForeground { get; } = Brushes.White;

    #region FileExplorerItemFont

    public FontFamily FileExplorerItemFontFamily { get; } = FontFamily.Default;
    public double FileExplorerItemFontSize { get; } = 18;
    public FontStyle FileExplorerItemFontStyle { get; } = FontStyle.Normal;
    public FontWeight FileExplorerItemFontWeight { get; } = FontWeight.ExtraLight;

    #endregion


    #region FileExplorerItemHover

    public IBrush? FileExplorerItemHoverForeground { get; } = Brushes.White;

    #region FileExplorerItemHoverFont

    public FontFamily FileExplorerItemHoverFontFamily { get; } = FontFamily.Default;
    public double FileExplorerItemHoverFontSize { get; } = 18;
    public FontStyle FileExplorerItemHoverFontStyle { get; } = FontStyle.Normal;
    public FontWeight FileExplorerItemHoverFontWeight { get; } = FontWeight.ExtraLight;

    #endregion

    #endregion


    #region FileExplorerItemSelected

    public IBrush? FileExplorerItemSelectedBackground { get; } = SolidColorBrush.Parse("#0078d7");
    public IBrush? FileExplorerItemSelectedForeground { get; } = Brushes.White;

    #region FileExplorerItemSelectedFont

    public FontFamily FileExplorerItemSelectedFontFamily { get; } = FontFamily.Default;
    public double FileExplorerItemSelectedFontSize { get; } = 18;
    public FontStyle FileExplorerItemSelectedFontStyle { get; } = FontStyle.Normal;
    public FontWeight FileExplorerItemSelectedFontWeight { get; } = FontWeight.ExtraLight;

    #endregion

    #endregion

    #endregion

    #endregion

    #region TabBar

    public IBrush? TabBarBackground { get; } = Brushes.Transparent;

    #region TabBarHeader

    public IBrush? TabBarHeaderBackground { get; } = Brushes.Transparent;

    #region TabBarHeaderFont

    public IBrush? TabBarHeaderFontBackground { get; } = Brushes.Transparent;
    public IBrush? TabBarHeaderFontForeground { get; } = Brushes.White;

    public FontFamily TabBarHeaderFontFamily { get; } = FontFamily.Default;
    public double TabBarHeaderFontSize { get; } = 28;
    public FontStyle TabBarHeaderFontStyle { get; } = FontStyle.Normal;
    public FontWeight TabBarHeaderFontWeight { get; } = FontWeight.SemiLight;

    #endregion

    #region TabBarHeaderButton

    public IBrush? TabBarHeaderButtonBackground { get; } = Brushes.Transparent;
    public IBrush? TabBarHeaderButtonForeground { get; } = Brushes.White;
    public double TabBarHeaderButtonSize { get; } = 28;
    public FontWeight TabBarHeaderButtonWeight { get; } = FontWeight.SemiLight;
    #region TabBarHeaderButtonHover

    public IBrush? TabBarHeaderButtonHoverBackground { get; } = Brushes.DimGray;
    public IBrush? TabBarHeaderButtonHoverForeground { get; } = Brushes.White;
    public double TabBarHeaderButtonHoverSize { get; } = 28;
    public FontWeight TabBarHeaderButtonHoverWeight { get; } = FontWeight.SemiLight;
    

    #endregion
    #region TabBarHeaderButtonPressed

    public IBrush? TabBarHeaderButtonPressedBackground { get; } = Brushes.LightGray;
    public IBrush? TabBarHeaderButtonPressedForeground { get; } = Brushes.White;
    public double TabBarHeaderButtonPressedSize { get; } = 28;
    public FontWeight TabBarHeaderButtonPressedWeight { get; } = FontWeight.SemiLight;
    

    #endregion

    #endregion

    #endregion

    #endregion
}