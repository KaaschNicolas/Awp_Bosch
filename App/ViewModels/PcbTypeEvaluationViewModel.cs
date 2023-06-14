using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.DataAccess;
using App.Core.DTOs;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services;
using App.Core.Services.Interfaces;
using App.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace App.ViewModels;

public partial class PcbTypeEvaluationViewModel : ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private PcbType _selectedPcbType;

    [ObservableProperty]
    private PcbType _selectedPcbType2;

    [ObservableProperty]
    private ObservableCollection<PcbType> _pcbTypes;

    private int _pcbId = 1007;

    [ObservableProperty]
    private ObservableCollection<Pcb> _pcbs;

    [ObservableProperty]
    private ObservableCollection<EvaluationStorageLocationDTO> _locations;

    [ObservableProperty]
    private int _countAtStorageLocation;

    [ObservableProperty]
    private PlotModel _pcbTypeFinalizedModel;

    [ObservableProperty]
    private int _countFinalized;

    [ObservableProperty]
    private int _countOpen;

    private readonly IPcbDataService<Pcb> _pcbDataService;
    private readonly ICrudService<StorageLocation> _storageLocationCrudService;
    private readonly ICrudService<PcbType> _pcbTypeCrudService;
    private readonly IDialogService _dialogService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;

    public PcbTypeEvaluationViewModel(
        IPcbDataService<Pcb> pcbDataService, 
        IStorageLocationDataService<StorageLocation> storageLocationDataService,
        ICrudService<PcbType> pcbTypesCrudService,
        IInfoBarService infoBarService, 
        IDialogService dialogService, 
        INavigationService navigationService)
    {
        _pcbDataService = pcbDataService;
        _storageLocationCrudService = storageLocationDataService;
        _pcbTypeCrudService = pcbTypesCrudService;
        _dialogService = dialogService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
        _locations = new ObservableCollection<EvaluationStorageLocationDTO>();
        _pcbTypes = new ObservableCollection<PcbType>();
        LoadStorageLocations();

        //_selectedPcbType = selectedPcbType1;

        _pcbTypeFinalizedModel = new PlotModel { Title = "Bearbeitungsstatus für Sachnummer: "  };
        CreatePieChart();
    }

    private async void CalcStatus()
    {
        if(_pcbId  != null)
        {
            var res = await _pcbTypeCrudService.GetById(_pcbId);
            if(res != null && res.Code == ResponseCode.Success)
            {
                    
                
            }
        }
    }

    private PlotModel CreatePieChart()
    {
        var seriesP1 = new PieSeries { StrokeThickness = 1.5, InsideLabelFormat=" ", OutsideLabelFormat = "{0}\n{1}", AngleSpan = 360, StartAngle = 270, InnerDiameter = 0.5 };


        seriesP1.Slices.Add(new PieSlice("abgeschlossen", 1030) { IsExploded = false, Fill = OxyColors.Green });
        seriesP1.Slices.Add(new PieSlice("offen", 929) { IsExploded = true, Fill = OxyColors.Blue });


        _pcbTypeFinalizedModel.Series.Add(seriesP1);

        // Gesamtzahl berechnen
        double total = seriesP1.Slices.Sum(slice => slice.Value);

        // Text in die Mitte schreiben
        _pcbTypeFinalizedModel.Subtitle = $"Insgesamt: {total}";

        return _pcbTypeFinalizedModel;
    }

    [RelayCommand]
    public async void LoadStorageLocations()
    {
        var storage = new List<StorageLocation>();
        var response = await _storageLocationCrudService.GetAll();
        if (response != null && response.Code == ResponseCode.Success)
        {
            foreach (var item in response.Data)
            {
                storage.Add(item);
            }
            foreach(var item in storage)
            {
                
            }
            //_infoBarService.showMessage("Laden erfolgreich", "Erfolg");
        }
        else if ((response != null && response.Code == ResponseCode.Error) || response == null)
        {
            _infoBarService.showError("Fehler bei Laden der Lagerorte", "Error");
        }
    }
    //public async Task LoadData(PcbType pcbType)
    //{
    //    using (var dbContext = BoschContext)
    //    {
    //        var query = "SELECT Count(Pcb) FROM Pcbs WHERE PcbType == pcbType GROUP BY StorageLocation";
    //        var result = await dbContext.Database.SqlQuery<MyModel>(query).ToListAsync();
    //        Data = new ObservableCollection<MyModel>(result);
    //    }
    //}

    /*private async Task GetPcbs(int pageIndex, int pageSize, bool isAscending)
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
                default:
                    maxEntries = await _pcbDataService.MaxEntries();
                    pcbs = await _pcbDataService.GetAllQueryable(pageSize, pageIndex, _sortyBy, isAscending);
                    break;
            }
            
            if (pcbs.Code == ResponseCode.Success && maxEntries.Code == ResponseCode.Success)
            {
                List<PaginatedPcb> convertedPcbs = new();

                // TODO: Error handling
                var resEager = await _pcbDataService.GetAllEager(pageIndex, pageSize, _sortyBy, isAscending);
                var newPcbs = new List<Pcb>();
                foreach (var item in resEager.Data)
                {
                    foreach (var pcb in pcbs.Data)
                    {
                        if (item.Id.Equals(pcb.Id))
                        {
                            newPcbs.Add(item);
                        }
                    }
                }

            }
        }
        else if (_filterOptions != PcbFilterOptions.None && _filterOptions == PcbFilterOptions.FilterStorageLocation)
        {
            if (_selectedComboBox.StorageName == "Alles")
            {
                maxEntries = await _pcbDataService.MaxEntries();
                pcbs = await _pcbDataService.GetAllQueryable(pageSize, pageIndex, _sortyBy, isAscending);
            }
            else
            {
                switch (_filterOptions)
                {
                    case PcbFilterOptions.Search:
                        maxEntries = await _pcbDataService.MaxEntriesSearch(QueryText);
                        pcbs = await _pcbDataService.Like(pageIndex, pageSize, QueryText);
                        break;
                    default:
                        maxEntries = await _pcbDataService.MaxEntries();
                        pcbs = await _pcbDataService.GetAllQueryable(pageSize, pageIndex, _sortyBy, isAscending);
                        break;
                }
            }

            if (pcbs.Code == ResponseCode.Success && maxEntries.Code == ResponseCode.Success)
            {
                List<PaginatedPcb> convertedPcbs = new();
                // TODO: Error handling
                var resEager = await _pcbDataService.GetAllEager(pageIndex, pageSize, _sortyBy, isAscending);
                var newPcbs = new List<Pcb>();
                foreach (var item in resEager.Data)
                {
                    foreach (var pcb in pcbs.Data)
                    {
                        if (item.Id.Equals(pcb.Id))
                        {
                            newPcbs.Add(item);
                        }
                    }
                }
            }
        }
        else
        {


            FirstAsyncCommand.NotifyCanExecuteChanged();
            PreviousAsyncCommand.NotifyCanExecuteChanged();
            NextAsyncCommand.NotifyCanExecuteChanged();
            LastAsyncCommand.NotifyCanExecuteChanged();
            FilterItems.NotifyCanExecuteChanged();
        }
    }*/
    public async void OnNavigatedTo(object parameter)
    {
        var pcbResponse = await _pcbTypeCrudService.GetAll();
        if (pcbResponse != null)
        {
            pcbResponse.Data.ForEach(_pcbTypes.Add);
        }


    }

    public void OnNavigatedFrom()
    {
        
    }
}
