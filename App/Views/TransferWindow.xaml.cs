// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using App.ViewModels;
using Microsoft.UI.Windowing;
using Windows.UI.WindowManagement;

namespace App.Views;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TransferWindow : WindowEx
{

    public TransferDialogViewModel ViewModel
    {
        get;
    }
    public TransferWindow()
    {
        ViewModel = App.GetService<TransferDialogViewModel>();
        InitializeComponent();
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Title = "Weitergabe erstellen";
        AppWindow.SetPresenter(AppWindowPresenterKind.CompactOverlay);

    }
}
