using App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class UpdateUserPage : Page
{
    public UpdateUserViewModel ViewModel
    {
        get;
    }

    public UpdateUserPage()
    {
        ViewModel = App.GetService<UpdateUserViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }
}
