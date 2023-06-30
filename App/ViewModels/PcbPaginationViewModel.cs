using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.DTOs;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services;
using App.Core.Services.Interfaces;
using App.Helpers;
using App.Messages;
using App.Models;
using App.Services.PrintService.impl;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace App.ViewModels
{
    public partial class PcbPaginationViewModel : ObservableRecipient, INavigationAware
    {

        // Konstruktor der Klasse, der alle erforderlichen Abhängigkeiten über Parameterübergabe(Dependency Injection) erhält
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
            // Initialisierung der AsyncRelayCommand-Objekte für verschiedene Aktionen
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
            // Initialisierung der abhängigen Services
            _dialogService = dialogService;
            _infoBarService = infoBarService;
            _navigationService = navigationService;
            _transferDataService = transferDataService;
            _storageLocationCrudService = storageLocationDataService;
            _pcbTypeCrudService = pcbTypeCrudService;
            Refresh();
        }

        // Private Felder zur Speicherung der abhängigen Services und anderer Variablen
        private readonly IPcbDataService<Pcb> _pcbDataService;
        private readonly ICrudService<StorageLocation> _storageLocationCrudService;
        private readonly ICrudService<PcbType> _pcbTypeCrudService;
        private readonly IInfoBarService _infoBarService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly ITransferDataService<Transfer> _transferDataService;

        // Deklaration der Befehle (Commands)
        public IAsyncRelayCommand FirstAsyncCommand { get; }
        public IAsyncRelayCommand PreviousAsyncCommand { get; }
        public IAsyncRelayCommand NextAsyncCommand { get; }
        public IAsyncRelayCommand LastAsyncCommand { get; }
        public IAsyncRelayCommand SortByCommand { get; }
        public IAsyncRelayCommand FilterItems { get; }

        // Deklaration der privaten Eigenschaften
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

        // Deklaration der öffentlichen Eigenschaften
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
        public ObservableCollection<PcbType> selectedPcbTypes = new();

        [ObservableProperty]
        private List<PcbType> _allPcbTypes;


        //Ruft die Liste der PCB-Typen vom PCB-Typ CRUD-Service ab und aktualisiert die Liste aller PCB-Typen.
        private async Task GetPcbTypes()
        {
            var response = await _pcbTypeCrudService.GetAll(); // Ruft alle Leiterplattentypen ab
            if (response != null && response.Code == ResponseCode.Success)
            {
                AllPcbTypes = new List<PcbType>(response.Data.OrderBy(x => x.PcbPartNumber)); // Ordnet die Leiterplattentypen nach ihrer PcbPartNumber und speichert sie in der Liste AllPcbTypes
            }
            else
            {
                _infoBarService.showError("Fehler beim Laden der Sachnnummern", "Error");
            }
        }

        // Erstellt eine Liste von PCB-Objekten basierend auf den ausgewählten PCB-Typen.
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


        // Ruft PCB-Daten basierend auf den angegebenen Filteroptionen und -parametern ab und erstellt eine PCB-Liste.
        private async Task GetPcbs(int pageIndex, int pageSize, bool isAscending)
        {

            Response<List<PcbDTO>> pcbs;
            Response<int> maxEntries;


            // Überprüfen der Lagerorte für das Filtern

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

                // Je nach Filteroption die entsprechende Logik ausführen
                switch (FilterOptions)
                {
                    // Filteroption: Suche
                    case PcbFilterOptions.Search:
                        maxEntries = await _pcbDataService.MaxEntriesSearch(QueryText);
                        pcbs = await _pcbDataService.Like(pageIndex, pageSize, QueryText);
                        break;
                    // Filteroption: Filter1
                    case PcbFilterOptions.Filter1:
                        Expression<Func<Pcb, bool>> where1 = x => x.Finalized == true;
                        maxEntries = await _pcbDataService.MaxEntriesFiltered(where1);
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, "Finalized = 1", SortBy, isAscending, PcbFilterOptions.Filter1);
                        break;
                    // Filteroption: Filter2
                    case PcbFilterOptions.Filter2:
                        Expression<Func<Pcb, bool>> where2 = x => x.CreatedDate.Date == DateTime.UtcNow.Date;
                        maxEntries = await _pcbDataService.MaxEntriesFiltered(where2);
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, "DATEDIFF(DAY, CreatedDate, GETDATE()) = 0", SortBy, isAscending, PcbFilterOptions.Filter2);
                        break;
                    // Filteroption: FilterPcbTypes
                    case PcbFilterOptions.FilterPcbTypes:
                        if (SelectedPcbTypes.Count > 0)
                        {
                            var l = new List<PcbType>(SelectedPcbTypes);
                            var pcbTypeIds = l.Select(x => x.Id);
                            string pcbTypeIdsString = string.Join(", ", pcbTypeIds);
                            maxEntries = await _pcbDataService.MaxEntriesPcbTypes(pcbTypeIdsString);
                            pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, pcbTypeIdsString, SortBy, isAscending, PcbFilterOptions.FilterPcbTypes);

                        }
                        else
                        {
                            // Keine ausgewählten Pcb-Typen
                            maxEntries = new Response<int>(ResponseCode.Success, 0);
                            pcbs = new Response<List<PcbDTO>>(ResponseCode.Success, new List<PcbDTO>());
                        }
                        break;
                    // Filteroption: FilterStorageLocation
                    case PcbFilterOptions.FilterStorageLocation:
                        maxEntries = await _pcbDataService.MaxEntriesByStorageLocation(SelectedComboBox.Id);
                        pcbs = await _pcbDataService.GetWithFilter(pageIndex, pageSize, SelectedComboBox.Id.ToString(), SortBy, isAscending, PcbFilterOptions.FilterStorageLocation);
                        break;
                    // Kein spezifischer Filter angewendet
                    default:
                        maxEntries = await _pcbDataService.MaxEntries();
                        pcbs = await _pcbDataService.GetAllQueryable(pageSize, pageIndex, SortBy, isAscending);
                        break;
                }
                // Erstellen der PCB-Liste
                if (pcbs.Code == ResponseCode.Success)
                {
                    await CreatePcbList(pageIndex, pageSize, pcbs.Data, isAscending, maxEntries.Data);
                }
            }

            // Keine Filteroptionen angegeben
            else
            {
                maxEntries = await _pcbDataService.MaxEntries();
                pcbs = await _pcbDataService.GetAllQueryable(pageIndex, pageSize, _sortyBy, isAscending);

                if (pcbs.Code == ResponseCode.Success)
                {
                    await CreatePcbList(pageIndex, pageSize, pcbs.Data, isAscending, maxEntries.Data);
                }
            }
            // Aktualisierung der Befehle zur Navigation und Filterung
            FirstAsyncCommand.NotifyCanExecuteChanged();
            PreviousAsyncCommand.NotifyCanExecuteChanged();
            NextAsyncCommand.NotifyCanExecuteChanged();
            LastAsyncCommand.NotifyCanExecuteChanged();
            FilterItems.NotifyCanExecuteChanged();

        }

        // Löscht eine ausgewählte Leiterplatte: nach Bestätigung des Löschvorgangs wird eine Erfolg- oder Fehlersmeldung angezeigt.
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


        // Zeigt einen Dialog zur Erstellung einer Weitergabe/Umbuchung an und zeigt entsprechende Erfolgs- oder Fehlermeldungen an.
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

        // Erstellt eine Druckseite mit den Informationen der ausgewählten Leiterplatte und druckt sie aus.
        [RelayCommand]
        public async void Print()
        {
            var res = await _pcbDataService.GetByIdEager(_selectedItem.PcbId);
            var pcbRes = res.Data;
            IDataMatrixService _dmService = new DataMatrixService();
            var dmImage = _dmService.GetDataMatrix(_selectedItem.SerialNumber);
            var dmImageConverted = BitmapToBitmapImageConverter.Convert(dmImage);
            var ErrorTypes = pcbRes.ErrorTypes;
            String FirstErrorCode;
            String FirstErrorDescription;
            String SecondErrorCode;
            String SecondErrorDescription;
            if (ErrorTypes[0].Code != null && ErrorTypes[0].ErrorDescription != null && ErrorTypes != null)
            {
                FirstErrorCode = ErrorTypes[0].Code;
                FirstErrorDescription = ErrorTypes[0].ErrorDescription;

                if (ErrorTypes[1].Code != null && ErrorTypes[1].ErrorDescription != null)
                {
                    SecondErrorCode = ErrorTypes[1].Code;
                    SecondErrorDescription = ErrorTypes[1].ErrorDescription;
                }
                else
                {
                    SecondErrorCode = " nicht vorhanden";
                    SecondErrorDescription = " nicht vorhanden";
                }
            }
            else
            {
                FirstErrorCode = " nicht vorhanden";
                FirstErrorDescription = " nicht vorhanden";
                SecondErrorCode = " nicht vorhanden";
                SecondErrorDescription = " nicht vorhanden";
            }
            var pcbPrintPageDto = new PcbPrintPageDTO()
            {
                Seriennummer = _selectedItem.SerialNumber,
                Sachnummer = _selectedItem.PcbPartNumber,
                Datamatrix = dmImageConverted,
                Einschraenkung = pcbRes.Restriction.Name,
                Panel = pcbRes.Comment,
                Status = _selectedItem.IsFinalized ? "abgeschlossen" : "offen",
                UmlaufTage = (int)Math.Round((DateTime.Now - pcbRes.CreatedDate).TotalDays),
                AktuellerStandort = _selectedItem.StorageName,
                Verweildauer = _selectedItem.DwellTime,
                LetzteBearbeitung = pcbRes.Transfers.Last().NotedBy.Name,
                Oberfehler = FirstErrorCode,
                OberfehlerBeschreibung = FirstErrorDescription,
                Unterfehler = SecondErrorCode,
                UnterfehlerBeschreibung = SecondErrorDescription,
            };
            var printPageModel = new PrintPageModel(pcbPrintPageDto);
            var _printService = new PrintService();
            _printService.Print(printPageModel);
        }

        // Navigiert zur Detailansicht einer Leiterplatte anhand der übergebenen PCB-ID.
        [RelayCommand]
        public void NavigateToDetails(int pcbId)
        {
            _navigationService.NavigateTo("App.ViewModels.PcbSingleViewModel", pcbId);
        }

        // Navigiert zur Aktualisierungsansicht einer Leiterplatte anhand der übergebenen PCB-ID.
        [RelayCommand]
        public void NavigateToUpdate(int pcbId)
        {
            _navigationService.NavigateTo("App.ViewModels.UpdatePcbViewModel", pcbId);
        }

        // Navigiert zur Erstellungsansicht, um ein PCB zu erstellen.
        [RelayCommand]
        public void NavigateToCreate()
        {
            _navigationService.NavigateTo("App.ViewModels.CreatePcbViewModel");
        }

        // Aktualisiert die Ansicht der Leiterplattenliste durch Setzen der Seitennummer auf 0 und Ausführen des FirstAsyncCommands Befehlsaktion.
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



        // Wird aufgerufen, wenn zur Seite navigiert wird, setzt IsActive auf true und ruft die Methode GetPcbTypes asynchron auf.
        public async void OnNavigatedTo(object parameter)
        {
            IsActive = true; // invokes onActivated
            await GetPcbTypes();
        }

        // Wird aufgerufen, wenn von der Seite weg navigiert wird, setzt IsActive auf false und ruft OnDeactivated auf.
        public void OnNavigatedFrom()
        {
            IsActive = false; // invokes onDeactivated
        }
    }
}
