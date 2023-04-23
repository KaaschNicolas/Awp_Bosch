using App.ViewModels;

using Microsoft.UI.Xaml.Controls;

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
    }
}
