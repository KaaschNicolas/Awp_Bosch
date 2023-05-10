using App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class PcbSinglePage : Page
{
    public PcbSingleViewModel ViewModel
    {
        get;
    }

    public PcbSinglePage()
    {
        ViewModel = App.GetService<PcbSingleViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }
}
