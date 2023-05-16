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
        dateOfFailure.Date = new DateTime(2023, 04, 23);
    }
}
