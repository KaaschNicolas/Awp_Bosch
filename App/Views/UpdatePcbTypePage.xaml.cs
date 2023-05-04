using App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class UpdatePcbTypePage : Page
{
    public UpdatePcbTypeViewModel ViewModel
    {
        get;
    }

    public UpdatePcbTypePage()
    {
        ViewModel = App.GetService<UpdatePcbTypeViewModel>();
        InitializeComponent();
    }
}
