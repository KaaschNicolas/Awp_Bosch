using App.Behaviors;
using App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System.Diagnostics;

namespace App.Views;

public sealed partial class CreateStorageLocationPage : Page
{
    public CreateStorageLocationViewModel ViewModel
    {
        get;
    }

    public CreateStorageLocationPage()
    {
        ViewModel = App.GetService<CreateStorageLocationViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
        DataContext = ViewModel;
    }

    public void checkFinalDestination(object sender, RoutedEventArgs e)
    {

    }

    public void SaveStorageLocationButton_Click(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Create_StorageLocation");
        //Frame.Navigate(typeof(StorageLocationPage));
    }

    public void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(StorageLocationPage));
    }
}
