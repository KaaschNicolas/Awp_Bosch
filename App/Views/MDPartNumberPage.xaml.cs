using System.Xml.Linq;
using App.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Windows.Devices.Enumeration;

namespace App.Views;

public sealed partial class MDPartNumberPage : Page
{


    public MDPartNumberPage()
    {
        ViewModel = App.GetService<MDPartNumberViewModel>();
        InitializeComponent();
    }

    public MDPartNumberViewModel ViewModel
    {
        get;
    }

    private void CreatPartNumberButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.Navigate(typeof(MD_CreatePartNumberPage));
    }
}
