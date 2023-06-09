// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using App.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ctWinUI = CommunityToolkit.WinUI.UI.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using App.Core.Models;
using App.Core.Models.Enums;
using CommunityToolkit.WinUI.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class StorageLocationsViewPage1 : Page
{
    private enum DataGridDisplayMode
    {
        Default,
        UserSorted,
        Filtered,
        Grouped,
        Search
    }

    public StorageLocationPaginationViewModel ViewModel { get; }

    public StorageLocationsViewPage1()
    {
        ViewModel = App.GetService<StorageLocationPaginationViewModel>();
        InitializeComponent();
        Loaded += Page_Loaded;
        Unloaded += Page_Unload;
        ViewModel.FilterOptions = StorageLocationFilterOptions.None;
        ViewModel.SortBy = DataGrid.Columns[0].Tag.ToString();
        DataGrid.SelectionChanged += DataGrid_SelectionChanged;
    }

    private DataGridDisplayMode _displayMode = DataGridDisplayMode.Default;
    private long _token;

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
        DataGrid.ItemsSource =  ViewModel.StorageLocations; //n�tig? weil schon in Xaml gebunden
        DataGrid.Columns[0].SortDirection = ctWinUI.DataGridSortDirection.Ascending;
        DataGrid.SelectionChanged += DataGrid_SelectionChanged;
        ViewModel.FilterOptions = StorageLocationFilterOptions.None;
    }

    private void Page_Unload(object sender, RoutedEventArgs e)
    {
        DataGrid.SelectionChanged -= DataGrid_SelectionChanged;
        ViewModel.FilterOptions = StorageLocationFilterOptions.None;
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        DataGrid.RowDetailsVisibilityMode = ctWinUI.DataGridRowDetailsVisibilityMode.Collapsed;
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

        ViewModel.SortBy = e.Column.Tag.ToString();
        bool isAscending = e.Column.SortDirection is null or (ctWinUI.DataGridSortDirection?)ctWinUI.DataGridSortDirection.Descending;
        
        await ViewModel.SortByDwellTime.ExecuteAsync(null); //hier nochmal schauen
    }

    private async void FilterDTYHigh_Click(object Sender, RoutedEventArgs e)
    {
        _displayMode = DataGridDisplayMode.Filtered;
        ViewModel.FilterOptions = StorageLocationFilterOptions.DwellTimeYellowHigh;
        await ViewModel.FilterItems.ExecuteAsync(null);
    }

    private async void FilterDTYLow_Click(object Sender, RoutedEventArgs e)
    {
        ViewModel.PageNumber = 1;
        _displayMode = DataGridDisplayMode.Filtered;
        ViewModel.FilterOptions = StorageLocationFilterOptions.DwellTimeYellowLow;
        await ViewModel.FilterItems.ExecuteAsync(null);
    }

    private async void FilterDTRHigh_Click(object Sender, RoutedEventArgs e)
    {
        _displayMode = DataGridDisplayMode.Filtered;
        ViewModel.FilterOptions = StorageLocationFilterOptions.DwellTimeRedHigh;
        await ViewModel.FilterItems.ExecuteAsync(null);
    }

    private async void FilterDTRLow_Click(object Sender, RoutedEventArgs e)
    {
        _displayMode = DataGridDisplayMode.Filtered;
        ViewModel.FilterOptions = StorageLocationFilterOptions.DwellTimeRedLow;
        await ViewModel.FilterItems.ExecuteAsync(null);
    }

    private async void FilterClear_Click(object sender, RoutedEventArgs e)
    {
        _displayMode = DataGridDisplayMode.Default;
        ViewModel.FilterOptions = StorageLocationFilterOptions.None;
        await ViewModel.FirstAsyncCommand.ExecuteAsync(null);
    }

    private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs e)
    {
        _displayMode = DataGridDisplayMode.Search;
        ViewModel.FilterOptions = StorageLocationFilterOptions.Search;
        ViewModel.QueryText = e.QueryText;
        await ViewModel.FilterItems.ExecuteAsync(null);
        
    }

    private async void SearchBox_QueryClick(object sender, RoutedEventArgs e)
    {
        _displayMode = DataGridDisplayMode.Search;
        ViewModel.FilterOptions = StorageLocationFilterOptions.Search;
        ViewModel.QueryText = SearchBox.Text;
        await ViewModel.FilterItems.ExecuteAsync(null);

    }

    private void DeleteClick(object sender, RoutedEventArgs e)
    {
        ViewModel.DeleteCommand.Execute(null);
    }

    void NavigateToUpdate(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.NavigateToUpdateCommand.Execute(ViewModel.SelectedItem);
    }

    private void CreatStorageLocationButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.Navigate(typeof(CreateStorageLocationPage));
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
    private DataGridColumn _actualSortedColumn;

}
