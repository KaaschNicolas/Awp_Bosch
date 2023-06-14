using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.DTOs;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using App.Messages;
using App.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace App.ViewModels
{
    public partial class PcbPaginationViewModel : ObservableRecipient, INavigationAware
    {

        public PcbPaginationViewModel(
            IPcbDataService<Pcb> pcbDataService,
            IStorageLocationDataService<StorageLocation> storageLocationDataService,
            IInfoBarService infoBarService,
            IDialogService dialogService,
            INavigationService navigationService,
            ITransferDataService<Transfer> transferDataService,
            ICrudService<PcbType> pcbTypeCrudService
        )
        {

            _pcbDataService = pcbDataService;
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
            _transferDataService = transferDataService;
            _storageLocationCrudService = storageLocationDataService;
            _pcbTypeCrudService = pcbTypeCrudService;
            Refresh();
        }

        private readonly IPcbDataService<Pcb> _pcbDataService;
        private readonly ICrudService<StorageLocation> _storageLocationCrudService;
        private readonly ICrudService<PcbType> _pcbTypeCrudService;
        private readonly IInfoBarService _infoBarService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
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
        private ObservableCollection<PcbDTO> _pcbs;

        [ObservableProperty]
        private ObservableCollection<StorageLocation> _storageLocations = new();

        [ObservableProperty]
        private bool _isSortingAscending;

        [ObservableProperty]
        private PcbFilterOptions _filterOptions;

        [ObservableProperty]
        private StorageLocation _selectedComboBox;

        [ObservableProperty]
        private PcbDTO _selectedItem;

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

        [ObservableProperty]
        public ObservableCollection<PcbType> selectedPcbTypes;

        [ObservableProperty]
        private List<PcbType> _allPcbTypes;


        private async Task GetPcbTypes()
        {
            var response = await _pcbTypeCrudService.GetAll();
            if (response != null && response.Code == ResponseCode.Success)
            {
                AllPcbTypes = response.Data;
            }
            else
            {
                _infoBarService.showError("Fehler beim Laden der Sachnnummern", "Error");
            }
        }

        private async Task CreatePcbList(int pageIndex, int pageSize, List<PcbDTO> pcbs, bool isAscending, int maxEntries)
        {

            PaginatedList<PcbDTO> pcbsPaginated = await PaginatedList<PcbDTO>.CreateAsync(
                pcbs,
                pageIndex,
                pageSize,
                maxEntries
            );

            PageNumber = pcbsPaginated.PageIndex;
            PageCount = pcbsPaginated.PageCount;

            Pcbs = new ObservableCollection<PcbDTO>(pcbsPaginated);

        }


        private async Task GetPcbs(int pageIndex, int pageSize, bool isAscending)
        {

            Response<List<PcbDTO>> pcbs;
            Response<int> maxEntries;


            // Get storage locations for filter

            if (StorageLocations.Count == 0)
            {
                var storageLocations = await _storageLocationCrudService.GetAll();

                if (storageLocations.Code == ResponseCode.Success)
                {
                    storageLocations.Data.ForEach(x => StorageLocations.Add(x));
                }
            }


            if (FilterOptions != PcbFilterOptions.None)
            {

                switch (FilterOptions)
                {
                    case PcbFilterOptions.Search:
                        maxEntries = await _pcbDataService.MaxEntriesSearch(QueryText);
                        pcbs = await _pcbDataService.Like(pageIndex, pageSize, QueryText);
                        break;
                    case PcbFilterOptions.Filter1:
                        Expression<Func<Pcb, bool>> where1 = x => x.Finalized == true;
                        maxEntries = await _pcbDataService.MaxEntriesFiltered(where1);
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, "Finalized = 1", SortBy, isAscending, PcbFilterOptions.Filter1);
                        break;
                    case PcbFilterOptions.Filter2:
                        Expression<Func<Pcb, bool>> where2 = x => x.CreatedDate.Date == DateTime.UtcNow.Date;
                        maxEntries = await _pcbDataService.MaxEntriesFiltered(where2);
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, "DATEDIFF(DAY, CreatedDate, GETDATE()) = 0", SortBy, isAscending, PcbFilterOptions.Filter2);
                        break;
                    case PcbFilterOptions.FilterPcbTypes:
                        var l = new List<PcbType>(SelectedPcbTypes);
                        var pcbTypeIds = l.Select(x => x.Id);
                        string pcbTypeIdsString = string.Join(", ", pcbTypeIds);
                        maxEntries = await _pcbDataService.MaxEntriesPcbTypes(pcbTypeIdsString);
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, pcbTypeIdsString, SortBy, isAscending, PcbFilterOptions.FilterPcbTypes);
                        break;
                    case PcbFilterOptions.FilterStorageLocation:
                        maxEntries = await _pcbDataService.MaxEntriesByStorageLocation(SelectedComboBox.Id);
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, SelectedComboBox.Id.ToString(), SortBy, isAscending, PcbFilterOptions.FilterStorageLocation);
                        break;
                    default:
                        maxEntries = await _pcbDataService.MaxEntries();
                        pcbs = await _pcbDataService.GetAllQueryable(pageSize, pageIndex, SortBy, isAscending);
                        break;
                }
                if (pcbs.Code == ResponseCode.Success)
                {
                    await CreatePcbList(pageIndex, pageSize, pcbs.Data, isAscending, maxEntries.Data);
                }
            }

            else
            {
                maxEntries = await _pcbDataService.MaxEntries();
                pcbs = await _pcbDataService.GetAllQueryable(pageIndex, pageSize, _sortyBy, isAscending);

                if (pcbs.Code == ResponseCode.Success)
                {

                    await CreatePcbList(pageIndex, pageSize, pcbs.Data, isAscending, maxEntries.Data);
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
            var result = await _dialogService.ConfirmDeleteDialogAsync("Leiterplatte Löschen", "Sind Sie sicher, dass Sie diesen Eintrag löschen wollen?");
            if (result != null && result == true)
            {
                PcbDTO pcbToRemove = SelectedItem;
                Pcbs.Remove(pcbToRemove);
                await _pcbDataService.Delete(pcbToRemove.PcbId);
                _infoBarService.showMessage("Erfolgreich Leiterplatte gelöscht", "Erfolg");
            }
        }


        [RelayCommand]
        public async void ShowTransfer()
        {
            var response = await _dialogService.ShowCreateTransferDialog();
            if (response != null && response.Code == ResponseCode.Success)
            {
                _infoBarService.showMessage("Weitergabe erfolgreich", "Erfolg");
                Refresh();
            }
            else if (response != null && response.Code == ResponseCode.Error)
            {
                _infoBarService.showError("Fehler bei der Weitergabe", "Erfolg");
            }
        }


        [RelayCommand]
        public void NavigateToDetails(int pcbId)
        {
            _navigationService.NavigateTo("App.ViewModels.PcbSingleViewModel", pcbId);
        }

        [RelayCommand]
        public void NavigateToUpdate(int pcbId)
        {
            _navigationService.NavigateTo("App.ViewModels.UpdatePcbViewModel", pcbId);
        }

        [RelayCommand]
        public void NavigateToCreate()
        {
            _navigationService.NavigateTo("App.ViewModels.CreatePcbViewModel");
        }

        private void Refresh()
        {
            _pageNumber = 0;
            FirstAsyncCommand.ExecuteAsync(null);
        }
        // Register Messenger on Page load send Pcb Id on request
        protected override void OnActivated()
        {

            Messenger.Register<PcbPaginationViewModel, CurrentPcbRequestMessage>(this, (r, m) =>
            {

                m.Reply(r.SelectedItem.PcbId);

            }
            );

        }
        // Unregister Messenger when Page is navigated away from
        protected override void OnDeactivated()
        {
            Messenger.UnregisterAll(this);
        }



        public async void OnNavigatedTo(object parameter)
        {
            IsActive = true; // invokes onActivated
            await GetPcbTypes();
        }


        public void OnNavigatedFrom()
        {
            IsActive = false; // invokes onDeactivated
        }
    }
}
