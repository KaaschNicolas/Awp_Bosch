// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using App.Helpers;
using App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DwellTimeEvalutionPage : Page
    {
        public DwellTimeEvaluationViewModel ViewModel { get; }
        public DwellTimeEvalutionPage()
        {
            ViewModel = App.GetService<DwellTimeEvaluationViewModel>();
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ApplyTheme(ActualTheme);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ActualThemeChanged += Page_ActualThemeChanged;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ActualThemeChanged += Page_ActualThemeChanged;
            base.OnNavigatedFrom(e);
        }

        private void Page_ActualThemeChanged(FrameworkElement sender, object args)
        {
            ApplyTheme(sender.ActualTheme);
        }

        private void ApplyTheme(ElementTheme theme)
        {
            ViewModel.DwellTimeBarPlot.ApplyTheme(theme);
        }

    }
}
