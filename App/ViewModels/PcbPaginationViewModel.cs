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
            ITransferDataService<Transfer> transferDataService
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
            Refresh();
        }

        private readonly IPcbDataService<Pcb> _pcbDataService;
        private readonly ICrudService<StorageLocation> _storageLocationCrudService;
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
        private ObservableCollection<PaginatedPcb> _pcbs;

        [ObservableProperty]
        private ObservableCollection<StorageLocation> _storageLocations = new();

        [ObservableProperty]
        private bool _isSortingAscending;

        [ObservableProperty]
        private PcbFilterOptions _filterOptions;

        [ObservableProperty]
        private StorageLocation _selectedComboBox;

        [ObservableProperty]
        private PaginatedPcb _selectedItem;

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

        private async Task CreatePcbList(int pageIndex, int pageSize, List<Pcb> pcbs, bool isAscending, int maxEntries)
        {
            List<PaginatedPcb> convertedPcbs = new();

            // TODO: Error handling
            var resEager = await _pcbDataService.GetAllEager(pageIndex, pageSize, _sortyBy, isAscending);
            var newPcbs = new List<Pcb>();
            foreach (var item in resEager.Data)
            {
                foreach (var pcb in pcbs)
                {
                    if (item.Id.Equals(pcb.Id))
                    {
                        newPcbs.Add(item);
                    }
                }
            }
            newPcbs.ForEach(pcbItem => convertedPcbs.Add(PaginatedPcb.ToPaginatedPcb(pcbItem)));

            PaginatedList<PaginatedPcb> pcbsPaginated = await PaginatedList<PaginatedPcb>.CreateAsync(
                convertedPcbs,
            pageIndex,
                pageSize,
                maxEntries
            );

            PageNumber = pcbsPaginated.PageIndex;
            PageCount = pcbsPaginated.PageCount;

            Pcbs = new ObservableCollection<PaginatedPcb>(pcbsPaginated);

        }

        private async Task GetPcbs(int pageIndex, int pageSize, bool isAscending)
        {
            Response<List<Pcb>> pcbs;
            int maxEntries;


            await _pcbDataService.GetAllEagerTest(0, 10, "Status", false);

            // Get storage locations for filter

            if (_storageLocations.Count == 0)
            {
                var storageLocations = await _storageLocationCrudService.GetAll();

                if (storageLocations.Code == ResponseCode.Success)
                {
                    storageLocations.Data.ForEach(x => _storageLocations.Add(x));
                }
            }

            // Check if new filter is set
            if (_filterOptions != PcbFilterOptions.None && _filterOptions != PcbFilterOptions.FilterStorageLocation)
            {
                switch (_filterOptions)
                {
                    case PcbFilterOptions.Search:
                        pcbs = await _pcbDataService.Like(pageIndex, pageSize, QueryText);
                        break;
                    case PcbFilterOptions.Filter1:
                        Expression<Func<Pcb, bool>> where1 = x => x.Finalized == true;
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, where1);
                        break;
                    case PcbFilterOptions.Filter2:
                        Expression<Func<Pcb, bool>> where2 = x => x.CreatedDate.Date == DateTime.Now.Date;
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, where2);
                        break;
                    case PcbFilterOptions.Filter3:
                        Expression<Func<Pcb, bool>> where3 = x => x.Transfers.Count < 0;
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, where3);
                        break;
                    default:
                        pcbs = await _pcbDataService.GetAllQueryable(pageSize, pageIndex, _sortyBy, isAscending);
                        break;
                }

                if (pcbs.Code == ResponseCode.Success)
                {
                    await CreatePcbList(pageIndex, pageSize, pcbs.Data, isAscending, maxEntries.Data);
                }
            }
            else if (_filterOptions != PcbFilterOptions.None && _filterOptions == PcbFilterOptions.FilterStorageLocation)
            {
                if (_selectedComboBox.StorageName == "Alles")
                {

                    pcbs = await _pcbDataService.GetAllQueryable(pageSize, pageIndex, _sortyBy, isAscending);
                }
                else
                {
                    switch (_filterOptions)
                    {
                        case PcbFilterOptions.Search:

                            pcbs = await _pcbDataService.Like(pageIndex, pageSize, QueryText);
                            break;
                        case PcbFilterOptions.Filter1:
                            Expression<Func<Pcb, bool>> where1 = x => x.Finalized == true;
                            pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, where1);
                            break;
                        case PcbFilterOptions.Filter2:
                            Expression<Func<Pcb, bool>> where2 = x => x.CreatedDate.Date == DateTime.UtcNow.Date;
                            pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, where2);
                            break;
                        case PcbFilterOptions.Filter3:
                            Expression<Func<Pcb, bool>> where3 = x => x.Transfers.Count < 0;
                            pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, where3);
                            break;
                        case PcbFilterOptions.FilterStorageLocation:
                            pcbs = await _pcbDataService.GetWithFilterStorageLocation(pageIndex, pageSize, SelectedComboBox.Id);
                            break;
                        default:
                            pcbs = await _pcbDataService.GetAllQueryable(pageSize, pageIndex, _sortyBy, isAscending);
                            break;
                    }
                }

                if (pcbs.Code == ResponseCode.Success)
                {
                    await CreatePcbList(pageIndex, pageSize, pcbs.Data, isAscending, maxEntries.Data);
                }
            }
            else
            {
                pcbs = await _pcbDataService.GetAllQueryable(pageIndex, pageSize, _sortyBy, isAscending);

                if (pcbs.Code == ResponseCode.Success)
                {
                    await CreatePcbList(pageIndex, pageSize, pcbs.Data, isAscending, maxEntries.Data);
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
                PaginatedPcb pcbToRemove = _selectedItem;
                _pcbs.Remove(pcbToRemove);
                await _pcbDataService.Delete(PaginatedPcb.ToPcb(pcbToRemove));
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
        public void NavigateToDetails(PaginatedPcb paginatedPcb)
        {
            var pcb = PaginatedPcb.ToPcb(paginatedPcb);
            _navigationService.NavigateTo("App.ViewModels.PcbSingleViewModel", pcb);
        }

        [RelayCommand]
        public void NavigateToUpdate(PaginatedPcb paginatedPcb)
        {
            var pcb = PaginatedPcb.ToPcb(paginatedPcb);
            _navigationService.NavigateTo("App.ViewModels.UpdatePcbViewModel", pcb);
        }

        private void Refresh()
        {
            _pageNumber = 0;
            FirstAsyncCommand.ExecuteAsync(null);
        }
        // Register Messenger on Page load
        protected override void OnActivated()
        {

            Messenger.Register<PcbPaginationViewModel, CurrentPcbRequestMessage>(this, (r, m) =>
            {

                m.Reply(PaginatedPcb.ToPcb(r.SelectedItem));

            }
            );

        }
        // Unregister Messneger when Page is navigated away from
        protected override void OnDeactivated()
        {
            Messenger.UnregisterAll(this);
        }



        public void OnNavigatedTo(object parameter)
        {
            IsActive = true; // invokes onActivated
        }


        public void OnNavigatedFrom()
        {
            IsActive = false; // invokes onDeactivated
        }
    }
}
