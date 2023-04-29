using System.Xml.Linq;
using App.Behaviors;
using App.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Windows.Devices.Enumeration;

namespace App.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
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
        ViewModel.DeleteDiagnoseCommand.Execute(null);
    }

}
