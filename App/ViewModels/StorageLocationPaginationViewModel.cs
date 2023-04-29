using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;
public class StorageLocationPaginationViewModel : ObservableRecipient
{
    public StorageLocationPaginationViewModel(ICrudService<StorageLocation> crudService)
    {
        _crudService = crudService;
        FirstAsyncCommand = new AsyncRelayCommand(
            async () => await GetStorageLocations(1, _pageSize),
            () => _pageNumber != 1
        );

        PreviousAsyncCommand = new AsyncRelayCommand(
            async () => await GetStorageLocations(_pageNumber -1, _pageSize),
            () => _pageNumber > 1
        );

        NextAsyncCommand = new AsyncRelayCommand(
            async () => await GetStorageLocations(_pageNumber + 1, _pageSize),
            () => _pageNumber < _pageCount
        );

        LastAsyncCommand = new AsyncRelayCommand(
            async () => await GetStorageLocations(_pageCount, _pageSize),
            () => _pageNumber != _pageCount
        );

        Refresh();
    }

    private readonly ICrudService<StorageLocation> _crudService;

    private int _pageSize = 10;
    private int _pageNumber;
    private int _pageCount;
    private List<StorageLocation> _storageLocations;

    public List<int> PageSizes => new() { 5, 10, 15, 20 };

    public IAsyncRelayCommand FirstAsyncCommand { get; }
    public IAsyncRelayCommand PreviousAsyncCommand { get; }
    public IAsyncRelayCommand NextAsyncCommand { get; }
    public IAsyncRelayCommand LastAsyncCommand { get; }

    public int PageNumber
    {
        get => _pageNumber;
        private set => SetProperty(ref _pageNumber, value);
    }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            SetProperty(ref _pageSize, value);
            Refresh();
        }
    }

    public int PageCount
    {
        get => _pageCount;
        private set => SetProperty(ref _pageCount, value);
    }

    public List<StorageLocation> StorageLocations
    {
        get => _storageLocations;
        private set => SetProperty(ref _storageLocations, value);
    }

    private async Task GetStorageLocations(int pageIndex, int pageSize)
    {
        var storageLocations = await _crudService.GetAll();
        if (storageLocations.Code == ResponseCode.Success)
        {
            PaginatedList<StorageLocation> storageLocationsPaginated = await PaginatedList<StorageLocation>.CreateAsync(
                storageLocations,
                pageIndex,
                pageSize
            );
            PageNumber = storageLocationsPaginated.PageIndex;
            PageCount = storageLocationsPaginated.PageCount;
            StorageLocations = storageLocationsPaginated;
            
            FirstAsyncCommand.NotifyCanExecuteChanged();
            PreviousAsyncCommand.NotifyCanExecuteChanged();
            NextAsyncCommand.NotifyCanExecuteChanged();
            LastAsyncCommand.NotifyCanExecuteChanged();
        }
    }

    private void Refresh()
    {
        _pageNumber = 0;
        FirstAsyncCommand.Execute(null);
    }
}
