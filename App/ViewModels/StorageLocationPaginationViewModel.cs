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
public class StorageLocationPaginationViewModel : ObservableObject
{
    public StorageLocationPaginationViewModel(ICrudService<StorageLocation> crudService)
    {
        _crudService = crudService;
    }

    private readonly ICrudService<StorageLocation> _crudService;

    private int _pageSize = 10;
    private int _pageNumber;
    private int _pageCount;
    private List<StorageLocation> _storageLocations;

    public IAsyncRelayCommand FirstAsyncCommand { get; }
    public IAsyncRelayCommand PreviousAsyncCommand { get; }
    public IAsyncRelayCommand NextAsyncCommand { get; }
    public IAsyncRelayCommand LastAsyncCommand { get; }

    public int PageNumber
    {
        get => _pageNumber;
        private set => SetProperty(ref _pageNumber, value);
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
        }
    }
}
