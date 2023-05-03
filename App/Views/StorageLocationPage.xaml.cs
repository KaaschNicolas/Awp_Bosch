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
        DataContext = ViewModel;
    }

    private void CreateStorageLocationButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.Navigate(typeof(CreateStorageLocationPage));
    }

    private void UpdateStorageLocationButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.NavigateToUpdateStorageLocationCommand.Execute(ViewModel.SelectedItem);
    }

    private void DeleteStorageLocation_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.DeleteSL.Execute(null);
    }

    //private void Verbleib_Checked()
    //{
    
    //}

    //private void Verbleib_Unchecked()
    //{

    //}
}
