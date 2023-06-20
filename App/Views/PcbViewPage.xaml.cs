// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using App.Core.Models;
using App.Core.Models.Enums;
using App.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ctWinUI = CommunityToolkit.WinUI.UI.Controls;

namespace App.Views
{
    public sealed partial class PcbViewPage : Page
    {
        private List<CheckBox> _listCheckBox = new();

        private bool _wasChecked = true;
        private bool _canExecute = true;
        private enum DataGridDisplayMode
        {
            Default,
            UserSorted,
            Filtered,
            Grouped,
            Search
        }

        public PcbPaginationViewModel ViewModel { get; }

        public PcbViewPage()
        {
            ViewModel = App.GetService<PcbPaginationViewModel>();
            InitializeComponent();
            DataContext = ViewModel;
            Loaded += Page_Loaded;
            Unloaded += Page_Unload;
            ViewModel.FilterOptions = PcbFilterOptions.None;
            ViewModel.SortBy = DataGrid.Columns[0].Tag.ToString();
            DataGrid.SelectionChanged += DataGrid_SelectionChanged;
        }

        private DataGridDisplayMode _displayMode = DataGridDisplayMode.Default;
        private long _token;
        private DataGridColumn _actualSortedColumn;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _token = DataGrid.RegisterPropertyChangedCallback(ctWinUI.DataGrid.ItemsSourceProperty, DataGridItemsSourceChangedCallback);
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            DataGrid.UnregisterPropertyChangedCallback(ctWinUI.DataGrid.ItemsSourceProperty, _token);
            base.OnNavigatingFrom(e);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _displayMode = DataGridDisplayMode.Default;
            DataGrid.ItemsSource = ViewModel.Pcbs; //nötig? weil schon in Xaml gebunden
            DataGrid.Columns[0].SortDirection = ctWinUI.DataGridSortDirection.Descending;
            DataGrid.SelectionChanged += DataGrid_SelectionChanged;
            ViewModel.FilterOptions = PcbFilterOptions.None;


        }


        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {

            CheckBox cb = sender as CheckBox;
            if (!_listCheckBox.Contains(cb))
            {
                _listCheckBox.Add(cb);
            }

            if ((_listCheckBox.Count - 1) == ViewModel.AllPcbTypes.Count)
            {
                SelectAll_Checked(sender, e);
            }

        }

        private void Page_Unload(object sender, RoutedEventArgs e)
        {
            DataGrid.SelectionChanged -= DataGrid_SelectionChanged;
            ViewModel.FilterOptions = PcbFilterOptions.None;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid.RowDetailsVisibilityMode = ctWinUI.DataGridRowDetailsVisibilityMode.Collapsed;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems.First() is not null)
                {
                    _displayMode = DataGridDisplayMode.Default;
                    ViewModel.FilterOptions = PcbFilterOptions.FilterStorageLocation;
                    ViewModel.SelectedComboBox = ComboBoxStorageLocation.SelectedItem as StorageLocation;
                    ViewModel.FilterItems.ExecuteAsync(null);
                }
            }
        }

        private async void DataGrid_Sorting(object sender, ctWinUI.DataGridColumnEventArgs e)
        {
            _displayMode = DataGridDisplayMode.UserSorted;

            _ = ViewModel.IsSortingAscending
                ? ViewModel.IsSortingAscending = false
                : ViewModel.IsSortingAscending = true;
            _ = ViewModel.IsSortingAscending
                ? e.Column.SortDirection = ctWinUI.DataGridSortDirection.Ascending
                : e.Column.SortDirection = ctWinUI.DataGridSortDirection.Descending;
            _actualSortedColumn = e.Column;

            if (e.Column.Tag is not null)
            {
                ViewModel.SortBy = e.Column.Tag.ToString();
            }
            bool isAscending = e.Column.SortDirection is null or (ctWinUI.DataGridSortDirection?)ctWinUI.DataGridSortDirection.Descending;
            await ViewModel.SortByCommand.ExecuteAsync(null); //hier nochmal schauen
        }

        private void Option_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Content is not null)
            {
                PcbType checkedPcbType = ViewModel.AllPcbTypes.Where(i => i.Description == (string)cb.Content).Single();
                ViewModel.SelectedPcbTypes.Add(checkedPcbType);

            }
            if (cb != _listCheckBox[0] && (ViewModel.SelectedPcbTypes.Count == (_listCheckBox.Count - 1)))
            {

                _listCheckBox[0].Checked -= SelectAll_Checked;
                _listCheckBox[0].IsChecked = true;
                _listCheckBox[0].Checked += SelectAll_Checked;
                _canExecute = false;
            }

        }

        private void Option_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ViewModel.SelectedPcbTypes.Remove(ViewModel.SelectedPcbTypes.Where(i => i.Description == (string)cb.Content).Single());
            if (cb != _listCheckBox[0])
            {
                _listCheckBox[0].Indeterminate -= SelectAll_Indeterminate;
                _listCheckBox[0].IsChecked = null;
                _listCheckBox[0].Indeterminate += SelectAll_Indeterminate;
            }

            if (ViewModel.SelectedPcbTypes.Count == 0)
            {
                _listCheckBox[0].IsChecked = false;

                _canExecute = true;
            }
        }

        private void SelectAll_Checked(object sender, RoutedEventArgs e)
        {
            _wasChecked = true;

            foreach (CheckBox cb in _listCheckBox)
            {
                cb.IsChecked = true;
            }



        }

        private void SelectAll_Unchecked(object sender, RoutedEventArgs e)
        {

            foreach (CheckBox cb in _listCheckBox)
            {
                cb.IsChecked = false;
            }
            ViewModel.SelectedPcbTypes.Clear();
            _wasChecked = false;
        }


        private void SelectAll_Indeterminate(object sender, RoutedEventArgs e)
        {
            if (_wasChecked)
            {
                SelectAll_Unchecked(sender, e);
                _listCheckBox[0].Unchecked -= SelectAll_Unchecked;
                _listCheckBox[0].IsChecked = false;
                _listCheckBox[0].Unchecked += SelectAll_Unchecked;
            }
            else
            {
                SelectAll_Checked(sender, e);
                _listCheckBox[0].Checked -= SelectAll_Checked;
                _listCheckBox[0].IsChecked = true;
                _listCheckBox[0].Checked += SelectAll_Checked;
            }

        }



        private async void Filter1_Click(object Sender, RoutedEventArgs e)
        {
            _displayMode = DataGridDisplayMode.Filtered;
            ViewModel.FilterOptions = PcbFilterOptions.Filter1;
            ComboBoxStorageLocation.SelectedItem = null;
            await ViewModel.FilterItems.ExecuteAsync(null);
        }

        private async void Filter2_Click(object Sender, RoutedEventArgs e)
        {
            ViewModel.PageNumber = 1;
            _displayMode = DataGridDisplayMode.Filtered;
            ViewModel.FilterOptions = PcbFilterOptions.Filter2;
            ComboBoxStorageLocation.SelectedItem = null;
            await ViewModel.FilterItems.ExecuteAsync(null);
        }

        private async void Filter3_Click(object Sender, RoutedEventArgs e)
        {
            _displayMode = DataGridDisplayMode.Filtered;
            ViewModel.FilterOptions = PcbFilterOptions.Filter3;
            ComboBoxStorageLocation.SelectedItem = null;
            await ViewModel.FilterItems.ExecuteAsync(null);
        }

        private async void FilterPcbTypes_Click(object Sender, RoutedEventArgs e)
        {
            _displayMode = DataGridDisplayMode.Filtered;
            ViewModel.FilterOptions = PcbFilterOptions.FilterPcbTypes;
            ComboBoxStorageLocation.SelectedItem = null;
            await ViewModel.FilterItems.ExecuteAsync(null);

        }

        private async void FilterClear_Click(object sender, RoutedEventArgs e)
        {
            _displayMode = DataGridDisplayMode.Default;
            ViewModel.FilterOptions = PcbFilterOptions.None;
            ComboBoxStorageLocation.SelectedItem = null;
            SelectAll_Checked(sender, e);
            await ViewModel.FirstAsyncCommand.ExecuteAsync(null);
        }

        private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs e)
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
        }

        private void DeleteClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.DeleteCommand.Execute(null);
        }

        private void PrintClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.PrintCommand.Execute(null);
        }

        void EditClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.NavigateToUpdateCommand.Execute(ViewModel.SelectedItem.PcbId);
        }

        private void NavigateToDetails(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.NavigateToDetailsCommand.Execute(ViewModel.SelectedItem.PcbId);
        }

        private void CreatePcbButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.NavigateToCreateCommand.Execute(null);
        }

        private void DataGridItemsSourceChangedCallback(DependencyObject sender, DependencyProperty dp)
        {
            if (_actualSortedColumn != null)
            {
                _ = ViewModel.IsSortingAscending
                    ? DataGrid.Columns[_actualSortedColumn.DisplayIndex].SortDirection = ctWinUI.DataGridSortDirection.Ascending
                    : DataGrid.Columns[_actualSortedColumn.DisplayIndex].SortDirection = ctWinUI.DataGridSortDirection.Descending;
            }

            // Remove Display Mode Indicators;
            FilterIndicator.Visibility = Visibility.Collapsed;
            SearchIndicator.Visibility = Visibility.Collapsed;

            // Remove Sort Indicators.
            if (dp == ctWinUI.DataGrid.ItemsSourceProperty)
            {
                foreach (var column in (sender as ctWinUI.DataGrid).Columns)
                {
                    column.SortDirection = null;
                }
            }

            if (_displayMode == DataGridDisplayMode.Filtered)
            {
                FilterIndicator.Visibility = Visibility.Visible;
            }

            if (_displayMode == DataGridDisplayMode.Search)
            {
                SearchIndicator.Visibility = Visibility.Visible;
            }

        }

        void TransferClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowTransferCommand.Execute(null);
        }
    }
}
