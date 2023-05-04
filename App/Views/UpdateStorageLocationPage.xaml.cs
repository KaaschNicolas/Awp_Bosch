using System.Diagnostics;
using App.Behaviors;
using App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace App.Views;

public sealed partial class UpdateStorageLocationPage : Page
{
    public UpdateStorageLocationViewModel ViewModel
    {
        get;
    }

    public UpdateStorageLocationPage()
    {
        ViewModel = App.GetService<UpdateStorageLocationViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
        DataContext = ViewModel;
    }

    public void UpdateStorageLocationButton_Click(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Update_StorageLocation");
        //Frame.Navigate(typeof(StorageLocationPage));
    }

    public void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(StorageLocationPage));
    }
}
