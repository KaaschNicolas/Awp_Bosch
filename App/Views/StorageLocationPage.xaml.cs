using App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class StorageLocationPage : Page
{
    public StorageLocationViewModel ViewModel
    {
        get;
    }

    public StorageLocationPage()
    {
        ViewModel = App.GetService<StorageLocationViewModel>();
        InitializeComponent();
    }
}
