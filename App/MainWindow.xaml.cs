﻿using App.Core.Services;
using App.Helpers;

namespace App;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/PfeilIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();

    }

}
