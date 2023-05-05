using App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class PcbPage : Page
{
    public PcbViewModel ViewModel
    {
        get;
    }

    public PcbPage()
    {
        ViewModel = App.GetService<PcbViewModel>();
        InitializeComponent();
    }
}
