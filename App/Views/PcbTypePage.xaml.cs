using App.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class PcbTypePage : Page
{

    public PcbTypeViewModel ViewModel
    {
        get;
    }

    public PcbTypePage()
    {
        ViewModel = App.GetService<PcbTypeViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void CreatPartNumberButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.Navigate(typeof(CreatePcbTypePage));
    }

    void deleteClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.DeleteCommand.Execute(null);
    }

    void NavigateToUpdate(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.NavigateToUpdateCommand.Execute(ViewModel.SelectedItem);
    }

    void RefreshPartNumberButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) 
    {
        ViewModel.RefreshPartNumberCommand.Execute(null);
    }
}
