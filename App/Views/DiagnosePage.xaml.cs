using App.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class DiagnosePage : Page
{
    public DiagnoseViewModel ViewModel
    {
        get;
    }
    public DiagnosePage()
    {
        ViewModel = App.GetService<DiagnoseViewModel>();
        InitializeComponent();
    }

    private void NavigateCreateDiagnose(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.Navigate(typeof(CreateDiagnosePage));
    }

    // Fix for Command not working in MenuFlyOut -> Bug WinUI
    void deleteClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.DeleteCommand.Execute(null);
    }

    void NavigateToUpdateClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.NavigateToUpdateCommand.Execute(ViewModel.SelectedItem);
    }

}
