﻿using App.Contracts.Services;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services;
using App.Core.Services.Interfaces;
using App.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace App.ViewModels
{
    public partial class PcbPaginationViewModel : ObservableRecipient
    {
        public PcbPaginationViewModel(
            IPcbDataService<Pcb> pcbDataService,
            IStorageLocationDataService<StorageLocation> storageLocationDataService,
            IInfoBarService infoBarService,
            IDialogService dialogService,
            INavigationService navigationService,
                IAuthenticationService authenticationService,
            ITransferDataService<Transfer> transferDataService,
            ICrudService<Diagnose> diagnoseCrudService
        )
        {
            _pcbDataService = pcbDataService;
            _authenticationService = authenticationService;
            FirstAsyncCommand = new AsyncRelayCommand(
                async () => await GetPcbs(1, _pageSize, _isSortingAscending),
                () => _pageNumber != 1
            );

            PreviousAsyncCommand = new AsyncRelayCommand(
                async () => await GetPcbs(_pageNumber - 1, _pageSize, _isSortingAscending),
                () => _pageNumber > 1
            );

            NextAsyncCommand = new AsyncRelayCommand(
                async () => await GetPcbs(_pageNumber + 1, _pageSize, _isSortingAscending),
                () => _pageNumber < _pageCount
            );

            LastAsyncCommand = new AsyncRelayCommand(
                async () => await GetPcbs(_pageCount, _pageSize, _isSortingAscending),
                () => _pageNumber != _pageCount
            );

            SortByCommand = new AsyncRelayCommand(
                async () => await GetPcbs(_pageNumber, _pageSize, _isSortingAscending),
                () => _pageNumber != _pageCount
            );

            FilterItems = new AsyncRelayCommand(
                async () => await GetPcbs(_pageNumber, _pageSize, _isSortingAscending),
                () => _pageNumber != _pageCount && _filterOptions != PcbFilterOptions.None
            );

            FilterOptions = PcbFilterOptions.None;
            _dialogService = dialogService;
            _infoBarService = infoBarService;
            _navigationService = navigationService;
            _storageLocationCrudService = storageLocationDataService;
            _diagnoseCrudService = diagnoseCrudService;
            _transferDataService = transferDataService;
            Refresh();
        }

        private readonly IPcbDataService<Pcb> _pcbDataService;
        private readonly ICrudService<StorageLocation> _storageLocationCrudService;
        private readonly ICrudService<Diagnose> _diagnoseCrudService;
        private readonly IInfoBarService _infoBarService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ITransferDataService<Transfer> _transferDataService;

        public IAsyncRelayCommand FirstAsyncCommand { get; }
        public IAsyncRelayCommand PreviousAsyncCommand { get; }
        public IAsyncRelayCommand NextAsyncCommand { get; }
        public IAsyncRelayCommand LastAsyncCommand { get; }
        public IAsyncRelayCommand SortByCommand { get; }
        public IAsyncRelayCommand FilterItems { get; }

        private int _pageSize = 10;
        private int _pageNumber;
        private int _pageCount;
        private string _queryText;
        private string _sortyBy;

        [ObservableProperty]
        private ObservableCollection<Pcb> _pcbs;

        [ObservableProperty]
        private ObservableCollection<StorageLocation> _storageLocations = new();

        [ObservableProperty]
        private bool _isSortingAscending;

        [ObservableProperty]
        private PcbFilterOptions _filterOptions;

        [ObservableProperty]
        private StorageLocation _selectedComboBox;

        [ObservableProperty]
        private Pcb _selectedItem;

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

        public int PageCount { get => _pageCount; set => SetProperty(ref _pageCount, value); }

        public string SortBy { get => _sortyBy; set => SetProperty(ref _sortyBy, value); }

        private async Task GetPcbs(int pageIndex, int pageSize, bool isAscending)
        {
            Response<List<Pcb>> pcbs;
            Response<int> maxEntries;

            if (_storageLocations.Count == 0)
            {
                var storageLocations = await _storageLocationCrudService.GetAll();

                if (storageLocations.Code == ResponseCode.Success)
                {
                    storageLocations.Data.ForEach(x => _storageLocations.Add(x));
                }
            }

            if (_filterOptions != PcbFilterOptions.None && _filterOptions != PcbFilterOptions.FilterStorageLocation)
            {
                switch (_filterOptions)
                {
                    case PcbFilterOptions.Search:
                        maxEntries = await _pcbDataService.MaxEntriesSearch(QueryText);
                        pcbs = await _pcbDataService.Like(pageIndex, pageSize, QueryText);
                        break;
                    case PcbFilterOptions.Filter1:
                        Expression<Func<Pcb, bool>> where1 = x => x.Finalized == true;
                        maxEntries = await _pcbDataService.MaxEntriesFiltered(where1);
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, where1);
                        break;
                    case PcbFilterOptions.Filter2:
                        Expression<Func<Pcb, bool>> where2 = x => x.CreatedDate.Date == DateTime.Now.Date;
                        maxEntries = await _pcbDataService.MaxEntriesFiltered(where2);
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, where2);
                        break;
                    case PcbFilterOptions.Filter3:
                        Expression<Func<Pcb, bool>> where3 = x => x.Transfers.Count < 0;
                        maxEntries = await _pcbDataService.MaxEntriesFiltered(where3);
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, where3);
                        break;
                    default:
                        maxEntries = await _pcbDataService.MaxEntries();
                        pcbs = await _pcbDataService.GetAllQueryable(pageSize, pageIndex, _sortyBy, isAscending);
                        break;
                }

                if (pcbs.Code == ResponseCode.Success && maxEntries.Code == ResponseCode.Success)
                {
                    PaginatedList<Pcb> pcbsPaginated = await PaginatedList<Pcb>.CreateAsync(
                        pcbs.Data,
                        pageIndex,
                        pageSize,
                        maxEntries.Data
                    );

                    PageNumber = pcbsPaginated.PageIndex;
                    PageCount = pcbsPaginated.PageCount;

                    ObservableCollection<Pcb> copy = new();
                    pcbsPaginated.ForEach(x => copy.Add(x));
                    Pcbs = copy;
                }
            }
            else if (_filterOptions != PcbFilterOptions.None && _filterOptions == PcbFilterOptions.FilterStorageLocation)
            {
                switch (_filterOptions)
                {
                    case PcbFilterOptions.Search:
                        maxEntries = await _pcbDataService.MaxEntriesSearch(QueryText);
                        pcbs = await _pcbDataService.Like(pageIndex, pageSize, QueryText);
                        break;
                    case PcbFilterOptions.Filter1:
                        Expression<Func<Pcb, bool>> where1 = x => x.Finalized == true && x.Transfers.LastOrDefault().StorageLocation.Id == _selectedComboBox.Id;
                        maxEntries = await _pcbDataService.MaxEntriesFiltered(where1);
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, where1);
                        break;
                    case PcbFilterOptions.Filter2:
                        Expression<Func<Pcb, bool>> where2 = x => x.CreatedDate.Date == DateTime.Now.Date && x.Transfers.LastOrDefault().StorageLocation.Id == _selectedComboBox.Id;
                        maxEntries = await _pcbDataService.MaxEntriesFiltered(where2);
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, where2);
                        break;
                    case PcbFilterOptions.Filter3:
                        Expression<Func<Pcb, bool>> where3 = x => x.Transfers.Count < 0 && x.Transfers.LastOrDefault().StorageLocation.Id == _selectedComboBox.Id;
                        maxEntries = await _pcbDataService.MaxEntriesFiltered(where3);
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, where3);
                        break;
                    case PcbFilterOptions.FilterStorageLocation:
                        maxEntries = await _pcbDataService.MaxEntriesByStorageLocation(SelectedComboBox.Id);
                        pcbs = await _pcbDataService.GetWithFilterStorageLocation(pageIndex, pageSize, SelectedComboBox.Id);
                        break;
                    default:
                        maxEntries = await _pcbDataService.MaxEntries();
                        pcbs = await _pcbDataService.GetAllQueryable(pageSize, pageIndex, _sortyBy, isAscending);
                        break;
                }

                if (pcbs.Code == ResponseCode.Success && maxEntries.Code == ResponseCode.Success)
                {
                    PaginatedList<Pcb> pcbsPaginated = await PaginatedList<Pcb>.CreateAsync(
                        pcbs.Data,
                        pageIndex,
                        pageSize,
                        maxEntries.Data
                    );

                    PageNumber = pcbsPaginated.PageIndex;
                    PageCount = pcbsPaginated.PageCount;

                    ObservableCollection<Pcb> copy = new();
                    pcbsPaginated.ForEach(x => copy.Add(x));
                    Pcbs = copy;
                }
            }
            else
            {
                maxEntries = await _pcbDataService.MaxEntries();
                pcbs = await _pcbDataService.GetAllQueryable(pageIndex, pageSize, _sortyBy, isAscending);

                if (pcbs.Code == ResponseCode.Success && maxEntries.Code == ResponseCode.Success)
                {
                    PaginatedList<Pcb> pcbsPaginated = await PaginatedList<Pcb>.CreateAsync(
                        pcbs.Data,
                        pageIndex,
                        pageSize,
                        maxEntries.Data
                    );

                    PageNumber = pcbsPaginated.PageIndex;
                    PageCount = pcbsPaginated.PageCount;

                    ObservableCollection<Pcb> copy = new();
                    pcbsPaginated.ForEach(x => copy.Add(x));
                    Pcbs = copy;
                }

                FirstAsyncCommand.NotifyCanExecuteChanged();
                PreviousAsyncCommand.NotifyCanExecuteChanged();
                NextAsyncCommand.NotifyCanExecuteChanged();
                LastAsyncCommand.NotifyCanExecuteChanged();
                FilterItems.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand]
        public async void Delete()
        {
            var result = await _dialogService.ConfirmDeleteDialogAsync("Leiterplatte Löschen", "Sind Sie sicher, dass Sie diesen Eintrag löschen wollen?");
            if (result != null && result == true)
            {
                Pcb pcbToRemove = _selectedItem;
                _pcbs.Remove(pcbToRemove);
                await _pcbDataService.Delete(pcbToRemove);
                _infoBarService.showMessage("Erfolgreich Leiterplatte gelöscht", "Erfolg");
            }
        }

        [RelayCommand]
        public async void ShowTransfer()
        {
            User currentUser = _authenticationService.currentUser();
            var storageLocationsResponse = await _storageLocationCrudService.GetAll();
            var storageLocations = new List<StorageLocation>();
            if (storageLocationsResponse.Code == ResponseCode.Success)
            {
                storageLocations = storageLocationsResponse.Data;
            }

            var diagnoseResponse = await _diagnoseCrudService.GetAll();
            var diagnoses = new List<Diagnose>();
            if (diagnoseResponse.Code == ResponseCode.Success)
            {
                diagnoses = diagnoseResponse.Data;
            }
            var result = await _dialogService.ShowCreateTransferDialog("Weitergabe", currentUser, storageLocations, diagnoses);
            if (result != null)
            {
                Transfer transfer = result.Item1;
                int diagnoseId = result.Item2;

                transfer.PcbId = _selectedItem.Id;

                var response = await _transferDataService.CreateTransfer(transfer, diagnoseId);
                if (response == ResponseCode.Success)
                {
                    _infoBarService.showMessage("Weitergabe erfolgreich", "Erfolg");
                }
                else
                {
                    _infoBarService.showError("Fehler bei der Weitergabe", "Error");

                }


            }
        }

        [RelayCommand]
        public void NavigateToDetails(Pcb pcb)
        {
            var test = pcb;
            var str = pcb.SerialNumber;
            _navigationService.NavigateTo("App.ViewModels.PcbSingleViewModel", pcb);
        }

        [RelayCommand]
        public void NavigateToUpdate(Pcb pcb)
        {
            _navigationService.NavigateTo("App.ViewModels.PcbSingleViewModel", pcb);
        }

        private void Refresh()
        {
            _pageNumber = 0;
            FirstAsyncCommand.ExecuteAsync(null);
        }

    }
}
