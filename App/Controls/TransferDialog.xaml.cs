// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using App.ViewModels;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App.Controls
{
    public sealed partial class TransferDialog : UserControl
    {
        public TransferDialogViewModel ViewModel
        {
            get;
        }

        public TransferDialog()
        {
            ViewModel = App.GetService<TransferDialogViewModel>();
            InitializeComponent();
        }
    }
}
