// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
        public ReconnectDialog()
        {
            ViewModel = App.GetService<ReconnectDialogViewModel>();
            InitializeComponent();
        }

        private void Click_Refresh(object sender, RoutedEventArgs e)
        {
            ViewModel.CheckConnectionCommand.ExecuteAsync(null);
        }
    }
}
