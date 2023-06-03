using App.ViewModels;
using Microsoft.UI.Xaml;
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

    public void Create(object sender, RoutedEventArgs e)
    {
        ViewModel.NavigateToCreateCommand.Execute(null);
    }

    public void Refresh(object sender, RoutedEventArgs e)
    {
        ViewModel.RefreshCommand.Execute(null);
    }
    public void Delete(object sender, RoutedEventArgs e)
    {
        ViewModel.DeleteCommand.Execute(null);
    }
    public void Edit(object sender, RoutedEventArgs e)
    {
        ViewModel.NavigateToUpdateCommand.Execute(null);
    }

    //private void Verbleib_Checked()
    //{

    //}

    //private void Verbleib_Unchecked()
    //{

    //}
}
