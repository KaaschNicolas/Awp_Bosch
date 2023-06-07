// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using App.ViewModels;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;

namespace App.Controls;
public sealed partial class TransferDialog : ContentDialog
{
    public TransferDialogViewModel ViewModel
    {
        get;
    }

    public AppWindow TransferAppWindow { get; set; }

    public TransferDialog()
    {
        ViewModel = App.GetService<TransferDialogViewModel>();
        InitializeComponent();
    }
}

