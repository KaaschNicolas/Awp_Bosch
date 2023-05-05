using App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class CreatePcbPage : Page
{
    public CreatePcbViewModel ViewModel
    {
        get;
    }

    public CreatePcbPage()
    {
        ViewModel = App.GetService<CreatePcbViewModel>();
        InitializeComponent();
    }
}
