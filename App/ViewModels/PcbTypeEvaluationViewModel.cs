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

    [ObservableProperty]
    private string _pcbNumber;

    [ObservableProperty]
    private DateTime _deadline;

    [ObservableProperty]
    private ObservableCollection<Pcb> _pcbs;

    [ObservableProperty]
    private ObservableCollection<EvaluationStorageLocationDTO> _locations;

    [ObservableProperty]
    private PlotModel _pcbTypeFinalizedModel;

    [ObservableProperty]
    private int _total;

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
    private readonly IPcbTypeEvaluationService _pcbTypeEvaluationService;

    public PcbTypeEvaluationViewModel(
        IPcbDataService<Pcb> pcbDataService, 
        IStorageLocationDataService<StorageLocation> storageLocationDataService,
        ICrudService<PcbType> pcbTypesCrudService,
        IInfoBarService infoBarService, 
        IDialogService dialogService, 
        INavigationService navigationService,
        IPcbTypeEvaluationService pcbTypeEvaluationService)
    {
        _pcbDataService = pcbDataService;
        _storageLocationCrudService = storageLocationDataService;
        _pcbTypeCrudService = pcbTypesCrudService;
        _dialogService = dialogService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
        _pcbTypeEvaluationService = pcbTypeEvaluationService;
        _locations = new ObservableCollection<EvaluationStorageLocationDTO>();
        _pcbTypes = new ObservableCollection<PcbType>();
        //LoadStorageLocations();

        GeneratePlotCommand = new AsyncRelayCommand(
             GeneratePlot
        );

        LoadStorageLocationsCommand = new AsyncRelayCommand(
            LoadStorageLocations
        );    

        Total = 0;
        PcbNumber = "1688400320";
        Deadline = DateTime.Now;

        //_pcbTypeFinalizedModel = new PlotModel { Title = "Bearbeitungsstatus für Sachnummer: "  };
        //CreatePieChart();
    }

    private async Task CalcStatus()
    {
        var response = await _pcbTypeEvaluationService.GetFinalizedByPcbType(_selectedPcbType.PcbPartNumber, Deadline);
        if (response != null && response.Code == ResponseCode.Success)
        {
            CountFinalized = response.Data[0].TotalFinalized;
            CountOpen = response.Data[0].TotalInProgress;
        }
        else if ((response != null && response.Code == ResponseCode.Error) || response == null)
        {
            _infoBarService.showError("Fehler bei Laden der Lagerorte", "Error");
        }
    }

    public IAsyncRelayCommand GeneratePlotCommand { get; }
    public IAsyncRelayCommand LoadStorageLocationsCommand { get; }

    public async Task GeneratePlot() 
    {
        PcbTypeFinalizedModel = new PlotModel();
        
        PcbTypeFinalizedModel = await CreatePieChart();
        
    }

    private async Task<PlotModel> CreatePieChart()
    {
        await CalcStatus();

        //var model = new PlotModel();

        var seriesP1 = new PieSeries { StrokeThickness = 1.5, InsideLabelFormat=" ", OutsideLabelFormat = "{0}\n{1}", AngleSpan = 360, StartAngle = 270, InnerDiameter = 0.5 };


        seriesP1.Slices.Add(new PieSlice("abgeschlossen", CountFinalized) { IsExploded = false, Fill = OxyColors.Green });
        seriesP1.Slices.Add(new PieSlice("offen", CountOpen) { IsExploded = true, Fill = OxyColors.Blue });

        PcbTypeFinalizedModel.Series.Add(seriesP1);
        PcbTypeFinalizedModel.Title = "Bearbeitungsstatus für Sachnummer: ";

        // Gesamtzahl berechnen
        double total = seriesP1.Slices.Sum(slice => slice.Value);

        // Text in die Mitte schreiben
        PcbTypeFinalizedModel.Subtitle = $"Insgesamt: {total}";

        return PcbTypeFinalizedModel;
    }

  
    private async Task LoadStorageLocations()
    {
        Locations.Clear();
        Total = 0;

        var storage = new List<StorageLocation>();
        var response = await _pcbTypeEvaluationService.GetAllByPcbType(SelectedPcbType.PcbPartNumber, Deadline);
        if (response != null && response.Code == ResponseCode.Success)
        {
            foreach (var item in response.Data)
            {
                _locations.Add(item);
                Total += item.SumCount;
            }
        }
        else if ((response != null && response.Code == ResponseCode.Error) || response == null)
        {
            _infoBarService.showError("Fehler bei Laden der Lagerorte", "Error");
        }

        await GeneratePlot();
    }
    
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
