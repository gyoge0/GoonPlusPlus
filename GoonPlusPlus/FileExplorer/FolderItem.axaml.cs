﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GoonPlusPlus.FileExplorer;

public partial class FolderItem : UserControl
{
    public FolderItem()
    {
        InitializeComponent();
        TextBlock = this.FindControl<TextBlock>("Block");
    }

    public TextBlock TextBlock { get; }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}