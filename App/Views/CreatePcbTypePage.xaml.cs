using App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class CreatePcbTypePage : Page
{
    public CreatePcbTypeViewModel ViewModel
    {
        get;
    }

    public CreatePcbTypePage()
    {
        ViewModel = App.GetService<CreatePcbTypeViewModel>();
        InitializeComponent();
    }
}
