using App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class AddStorageLocationPage : Page
{
    public AddStorageLocationViewModel ViewModel
    {
        get;
    }

    public AddStorageLocationPage()
    {
        ViewModel = App.GetService<AddStorageLocationViewModel>();
        InitializeComponent();
    }
}
