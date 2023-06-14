using App.Core.Models;
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
        DataContext = ViewModel;
    }

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        // Only get results when it was a user typing, 
        // otherwise assume the value got filled in by TextMemberPath 
        // or the handler for SuggestionChosen.
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            //Set the ItemsSource to be your filtered dataset
            var dataset = ViewModel.PcbTypes.Where(w => w.PcbPartNumber.Contains(sender.Text));
            sender.ItemsSource = dataset;
        }
    }

    private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        // Set sender.Text. You can use args.SelectedItem to build your text string.
        ViewModel.SelectedPcbType = (PcbType)args.SelectedItem;
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
