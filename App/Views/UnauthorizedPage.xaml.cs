using App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class UnauthorizedPage : Page
{
    public UnauthorizedViewModel ViewModel
    {
        get;
    }

    public UnauthorizedPage()
    {
        ViewModel = App.GetService<UnauthorizedViewModel>();
        InitializeComponent();
    }
}
