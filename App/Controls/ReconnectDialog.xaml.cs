// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using App.ViewModels;
using Microsoft.UI.Windowing;
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

namespace App.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReconnectDialog : ContentDialog
    {
        public ReconnectDialogViewModel ViewModel { get; }
        public AppWindow ReconnectAppWindow { get; set; }
        public ReconnectDialog()
        {
            ViewModel = App.GetService<ReconnectDialogViewModel>();
            InitializeComponent();
            PrimaryButtonCommand = ViewModel.CheckConCommand;
        }

        //private void Click_Refresh(object sender, RoutedEventArgs e)
        //{
        //    ViewModel.CheckConCommand.ExecuteAsync(null);
            
        //    if (ViewModel.ConnectionAvailable == true)
        //    {
        //        Hide();
        //    }
        //}


    }
}
