using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Core.Helpers;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using App.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace App.ViewModels;
public class StorageLocationPaginationViewModel<T> : ObservableRecipient where T : StorageLocation
{   
    public StorageLocationPaginationViewModel(IStorageLocationDataService<StorageLocation> crudService)
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
            () => _pageNumber != _pageCount && _sortedByDwellTimeYellow);
        FilterItems = new AsyncRelayCommand(
            async () => await GetStorageLocations(_pageCount, _pageSize, false),
            () => _pageNumber != _pageCount && _filterOptions != StorageLocationFilterOptions.None);

        Refresh();
    }

    private readonly IStorageLocationDataService<StorageLocation> _crudService;

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
    private bool _filterDwellTimeHigh;
    private List<StorageLocation> _storageLocations;
    private StorageLocationFilterOptions _filterOptions;
    public List<int> PageSizes => new() { 5, 10, 15, 20 };

    public int PageNumber { get => _pageNumber; private set => SetProperty(ref _pageNumber, value); }

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
    public bool FilterDwellTimeHighFlag { get => _filterDwellTimeHigh; set => SetProperty(ref _filterDwellTimeHigh, value); }
    public StorageLocationFilterOptions FilterOptions { get => _filterOptions; set => SetProperty(ref _filterOptions, value); }

    public List<StorageLocation> StorageLocations { get => _storageLocations; private set => SetProperty(ref _storageLocations, value); }

    private async Task GetStorageLocations(int pageIndex, int pageSize, bool isAscending)
    {
        Response<List<StorageLocation>> storageLocations;
        Response<int> maxEntries;

        if (SortedByDwellTimeYellowFlag is true)
        {
            storageLocations = await _crudService.GetAllSortedBy(pageIndex, pageSize, "DwellTimeYellow", isAscending);
        }
        else
        {
            storageLocations = await _crudService.GetAllQueryable(pageIndex, pageSize);
        }

        switch (_filterOptions)
        {
            case StorageLocationFilterOptions.DwellTimeYellowLow:
                Expression<Func<StorageLocation, bool>> whereDTYLow = x => x.DwellTimeYellow < 10;
                maxEntries = await _crudService.MaxEntriesFiltered(whereDTYLow);
                storageLocations = await _crudService.GetWithFilter(pageIndex, pageSize, _filterOptions, whereDTYLow);
                break;
            case StorageLocationFilterOptions.DwellTimeYellowHigh:
                Expression<Func<StorageLocation, bool>> whereDTYHigh = x => x.DwellTimeYellow < 10;
                maxEntries = await _crudService.MaxEntriesFiltered(whereDTYHigh);
                storageLocations = await _crudService.GetWithFilter(pageIndex, pageSize, _filterOptions, whereDTYHigh);
                break;
            case StorageLocationFilterOptions.DwellTimeRedLow:
                Expression<Func<StorageLocation, bool>> whereDTRLow = x => x.DwellTimeRed < 10;
                maxEntries = await _crudService.MaxEntriesFiltered(whereDTRLow);
                storageLocations = await _crudService.GetWithFilter(pageIndex, pageSize, _filterOptions, whereDTRLow);
                break;
            case StorageLocationFilterOptions.DwellTimeRedHigh:
                Expression<Func<StorageLocation, bool>> whereDTRHigh = x => x.DwellTimeRed > 10;
                maxEntries = await _crudService.MaxEntriesFiltered(whereDTRHigh);
                storageLocations = await _crudService.GetWithFilter(pageIndex, pageSize, _filterOptions, whereDTRHigh);
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
                pageIndex,
                pageSize,
                maxEntries.Data
            );
            PageNumber = storageLocationsPaginated.PageIndex;
            PageCount = storageLocationsPaginated.PageCount;
            StorageLocations = storageLocationsPaginated;
            
        }
            
        FirstAsyncCommand.NotifyCanExecuteChanged();
        PreviousAsyncCommand.NotifyCanExecuteChanged();
        NextAsyncCommand.NotifyCanExecuteChanged();
        LastAsyncCommand.NotifyCanExecuteChanged();
    }

    private void Refresh()
    {
        _pageNumber = 0;
        FirstAsyncCommand.ExecuteAsync(null);
    }
}
