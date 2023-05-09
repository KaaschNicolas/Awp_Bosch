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

    public StorageLocationsViewPage1()
    {
        ViewModel = App.GetService<StorageLocationPaginationViewModel>();
        InitializeComponent();
        Loaded += Page_Loaded;
        Unloaded += Page_Unload;
        ViewModel.FilterOptions = StorageLocationFilterOptions.None;
        //DataGrid.SelectionChanged += DataGrid_SelectionChanged;  <--- richtiges Event
    }

    private DataGridDisplayMode _displayMode = DataGridDisplayMode.Default;
    private long _token;
    private string _grouping;

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
        DataGrid.ItemsSource =  ViewModel.StorageLocations; //nötig? weil schon in Xaml gebunden
        DataGrid.Columns[0].SortDirection = ctWinUI.DataGridSortDirection.Ascending;
        DataGrid.SelectionChanged += DataGrid_SelectionChanged;
    }

    private void Page_Unload(object sender, RoutedEventArgs e)
    {
        DataGrid.SelectionChanged -= DataGrid_SelectionChanged;
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        DataGrid.RowDetailsVisibilityMode = ctWinUI.DataGridRowDetailsVisibilityMode.Collapsed;
    }

    private async void DataGrid_Sorting(object sender, ctWinUI.DataGridColumnEventArgs e)
    {
        _displayMode = DataGridDisplayMode.UserSorted;
        ViewModel.SortedByDwellTimeYellowFlag = true;
        await ViewModel.SortByDwellTime.ExecuteAsync(null); //hier nochmal schauen
        bool isAscending = e.Column.SortDirection is null or (ctWinUI.DataGridSortDirection?)ctWinUI.DataGridSortDirection.Descending;
        e.Column.SortDirection = isAscending
            ? ctWinUI.DataGridSortDirection.Ascending
            : ctWinUI.DataGridSortDirection.Descending;
    }

    private async void FilterDTYHigh_Click(object Sender, RoutedEventArgs e)
    {
        _displayMode = DataGridDisplayMode.Filtered;
        ViewModel.FilterOptions = StorageLocationFilterOptions.DwellTimeYellowHigh;
        await ViewModel.FilterItems.ExecuteAsync(null);
    }

    private async void FilterDTYLow_Click(object Sender, RoutedEventArgs e)
    {
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

    private void DataGridItemsSourceChangedCallback(DependencyObject sender, DependencyProperty dp)
    {
        // Binding could do most of this ...

        // Remove Display Mode Indicators;
        FilterIndicator.Visibility = Visibility.Collapsed;
       

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

    }

    public StorageLocationPaginationViewModel ViewModel { get; }
}
