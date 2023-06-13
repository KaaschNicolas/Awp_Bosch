using App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class CreateUserPage : Page
{
    public CreateUserViewModel ViewModel
    {
        get;
    }

    public CreateUserPage()
    {
        ViewModel = App.GetService<CreateUserViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }
}
