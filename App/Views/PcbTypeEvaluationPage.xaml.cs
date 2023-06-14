using App.Core.Models.Enums;
using App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class PcbTypeEvaluationPage : Page
{
    public PcbTypeEvaluationViewModel ViewModel
    {
        get;
    }

    public PcbTypeEvaluationPage()
    {
        ViewModel = App.GetService<PcbTypeEvaluationViewModel>();
        InitializeComponent();
    }

    /*private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs e)
    {
        _displayMode = DataGridDisplayMode.Search;
        ViewModel.FilterOptions = PcbFilterOptions.Search;
        ViewModel.QueryText = e.QueryText;
        ComboBoxStorageLocation.SelectedItem = null;
        await ViewModel.FilterItems.ExecuteAsync(null);
    }

    private async void SearchBox_QueryClick(object sender, RoutedEventArgs e)
    {
        _displayMode = DataGridDisplayMode.Search;
        ViewModel.FilterOptions = PcbFilterOptions.Search;
        ViewModel.QueryText = SearchBox.Text;
        ComboBoxStorageLocation.SelectedItem = null;
        await ViewModel.FilterItems.ExecuteAsync(null);
    }*/
}
