using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Contracts.Services;
using App.Core.Helpers;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using App.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace App.ViewModels;
public partial class StorageLocationPaginationViewModel : ObservableRecipient
{   
    public StorageLocationPaginationViewModel(IStorageLocationDataService<StorageLocation> crudService, IInfoBarService infoBarService, IDialogService dialogService, INavigationService navigationService)
    {
        _crudService = crudService;
        FirstAsyncCommand = new AsyncRelayCommand(
            async () => await GetStorageLocations(1, _pageSize, false),
            () => _pageNumber != 1
        );

        PreviousAsyncCommand = new AsyncRelayCommand(
            async () => await GetStorageLocations(_pageNumber -1, _pageSize, false),
            () => _pageNumber > 1
        );

        NextAsyncCommand = new AsyncRelayCommand(
            async () => await GetStorageLocations(_pageNumber + 1, _pageSize, false),
            () => _pageNumber < _pageCount
        );

        LastAsyncCommand = new AsyncRelayCommand(
            async () => await GetStorageLocations(_pageCount, _pageSize, false),
            () => _pageNumber != _pageCount
        );

        SortByDwellTime = new AsyncRelayCommand(
            async () => await GetStorageLocations(_pageCount, _pageSize, true),
            () => _pageNumber != _pageCount
        );

        FilterItems = new AsyncRelayCommand(
            async () => await GetStorageLocations(_pageNumber, _pageSize, false),
            () => _pageNumber != _pageCount && _filterOptions != StorageLocationFilterOptions.None
        );

        _dialogService = dialogService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;

        Refresh();
    }

    private readonly IStorageLocationDataService<StorageLocation> _crudService;
    private readonly IDialogService _dialogService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;

    public IAsyncRelayCommand FirstAsyncCommand { get; }
    public IAsyncRelayCommand PreviousAsyncCommand { get; }
    public IAsyncRelayCommand NextAsyncCommand { get; }
    public IAsyncRelayCommand LastAsyncCommand { get; }
    public IAsyncRelayCommand SortByDwellTime { get; }
    public IAsyncRelayCommand FilterItems { get; }

    private int _pageSize = 10;
    private int _pageNumber;
    private int _pageCount;
    private bool _sortedByDwellTimeYellow;
    private string _queryText;
    private List<StorageLocation> _storageLocations;
    private StorageLocationFilterOptions _filterOptions;
    public List<int> PageSizes => new() { 5, 10, 15, 20 };

    public string QueryText { get => _queryText; set => SetProperty(ref _queryText, value); }

    public int PageNumber { get => _pageNumber; set => SetProperty(ref _pageNumber, value); }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            SetProperty(ref _pageSize, value);
            Refresh();
        }
    }

    public int PageCount { get => _pageCount; private set => SetProperty(ref _pageCount, value); }
    public bool SortedByDwellTimeYellowFlag { get => _sortedByDwellTimeYellow; set => SetProperty(ref _sortedByDwellTimeYellow, value); }
    public StorageLocationFilterOptions FilterOptions { get => _filterOptions; set => SetProperty(ref _filterOptions, value); }

    public List<StorageLocation> StorageLocations { get => _storageLocations; private set => SetProperty(ref _storageLocations, value); }

    [ObservableProperty]
    private StorageLocation _selectedItem;

    private async Task GetStorageLocations(int pageIndex, int pageSize, bool isAscending)
    {
        Response<List<StorageLocation>> storageLocations;
        Response<int> maxEntries;

        _ = SortedByDwellTimeYellowFlag is true 
            ? storageLocations = await _crudService.GetAllSortedBy(pageIndex, pageSize, "DwellTimeYellow", isAscending) 
            : storageLocations = await _crudService.GetAllQueryable(pageIndex, pageSize);

        if (FilterOptions != StorageLocationFilterOptions.None)
        {
            switch (_filterOptions)
            {
                case StorageLocationFilterOptions.DwellTimeYellowLow:
                    Expression<Func<StorageLocation, bool>> whereDTYLow = x => x.DwellTimeYellow < 5;
                    maxEntries = await _crudService.MaxEntriesFiltered(whereDTYLow);
                    storageLocations = await _crudService.GetWithFilter(pageIndex, pageSize, whereDTYLow);
                    break;
                case StorageLocationFilterOptions.DwellTimeYellowHigh:
                    Expression<Func<StorageLocation, bool>> whereDTYHigh = x => x.DwellTimeYellow >= 5;
                    maxEntries = await _crudService.MaxEntriesFiltered(whereDTYHigh);
                    storageLocations = await _crudService.GetWithFilter(pageIndex, pageSize, whereDTYHigh);
                    break;
                case StorageLocationFilterOptions.DwellTimeRedLow:
                    Expression<Func<StorageLocation, bool>> whereDTRLow = x => x.DwellTimeRed < 5;
                    maxEntries = await _crudService.MaxEntriesFiltered(whereDTRLow);
                    storageLocations = await _crudService.GetWithFilter(pageIndex, pageSize, whereDTRLow);
                    break;
                case StorageLocationFilterOptions.DwellTimeRedHigh:
                    Expression<Func<StorageLocation, bool>> whereDTRHigh = x => x.DwellTimeRed >= 5;
                    maxEntries = await _crudService.MaxEntriesFiltered(whereDTRHigh);
                    storageLocations = await _crudService.GetWithFilter(pageIndex, pageSize, whereDTRHigh);
                    break;
                case StorageLocationFilterOptions.Search:
                    maxEntries = await _crudService.MaxEntriesSearch(QueryText);
                    storageLocations = await _crudService.Like(pageIndex, pageSize, QueryText);
                    break;
                default:
                    maxEntries = await _crudService.MaxEntries();
                    storageLocations = await _crudService.GetAllQueryable(pageIndex, pageSize);
                    break;
            }

            if (storageLocations.Code == ResponseCode.Success && maxEntries.Code == ResponseCode.Success)
            {
                PaginatedList<StorageLocation> storageLocationsPaginated = await PaginatedList<StorageLocation>.CreateAsync(
                    storageLocations.Data,
                    pageIndex, //muss neu berechnet werden
                    pageSize,
                    maxEntries.Data
                );
                PageNumber = storageLocationsPaginated.PageIndex;
                PageCount = storageLocationsPaginated.PageCount;
                StorageLocations = storageLocationsPaginated;
            }
        }
        else
        {
            maxEntries = await _crudService.MaxEntries();
            storageLocations = await _crudService.GetAllQueryable(pageIndex, pageSize);

            if (storageLocations.Code == ResponseCode.Success && maxEntries.Code == ResponseCode.Success)
            {
                PaginatedList<StorageLocation> storageLocationsPaginated = await PaginatedList<StorageLocation>.CreateAsync(
                    storageLocations.Data,
                    pageIndex, //muss neu berechnet werden
                    pageSize,
                    maxEntries.Data
                );
                PageNumber = storageLocationsPaginated.PageIndex;
                PageCount = storageLocationsPaginated.PageCount;
                StorageLocations = storageLocationsPaginated;
            }
        }

        FirstAsyncCommand.NotifyCanExecuteChanged();
        PreviousAsyncCommand.NotifyCanExecuteChanged();
        NextAsyncCommand.NotifyCanExecuteChanged();
        LastAsyncCommand.NotifyCanExecuteChanged();
        FilterItems.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    public async void Delete()
    {
        var result = await _dialogService.ConfirmDeleteDialogAsync("Sachnummer Löschen", "Sind Sie sicher, dass Sie diesen Eintrag löschen wollen?");
        if (result != null && result == true)
        {
            StorageLocation pcbToRemove = _selectedItem;
            _storageLocations.Remove(pcbToRemove);
            await _crudService.Delete(pcbToRemove);
            _infoBarService.showMessage("Erfolgreich Leiterplatte gelöscht", "Erfolg");

        }
    }

    [RelayCommand]
    public void NavigateToUpdate(StorageLocation storageLocation)
    {
        _navigationService.NavigateTo("App.ViewModels.UpdateStorageLocationViewModel", storageLocation);

    }

    private void Refresh()
    {
        _pageNumber = 0;
        FirstAsyncCommand.ExecuteAsync(null);
    }
}
