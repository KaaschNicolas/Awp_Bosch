using System.Xml.Linq;
using App.Behaviors;
using App.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Windows.Devices.Enumeration;

namespace App.Views;

public sealed partial class MDPartNumberPage : Page
{


    public MDPartNumberViewModel ViewModel
    {
        get;
    }

    public MDPartNumberPage()
    {
        ViewModel = App.GetService<MDPartNumberViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void CreatPartNumberButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.Navigate(typeof(MD_CreatePartNumberPage));
    }

    void deleteClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.DeletePN.Execute(null);
    }

}
