using System.Diagnostics;
using App.Behaviors;
using App.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace App.Views;

public sealed partial class MD_CreatePartNumberPage : Page
{
    public MDPartNumberViewModel ViewModel
    {
        get;
    }

    public MD_CreatePartNumberPage()
    {
        
        ViewModel = App.GetService<MDPartNumberViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
        DataContext = ViewModel;


    }

    private void Create_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Debug.WriteLine("Create_Item");
    }

    private void Cancel_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.Navigate(typeof(MDPartNumberPage));
    }


}
