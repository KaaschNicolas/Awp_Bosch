using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.DTOs;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace App.ViewModels;

public partial class PcbTypeI_OEvaluationViewModel : ObservableRecipient, INavigationAware
{
    private readonly ICrudService<PcbType> _pcbTypeCrudService;
    private readonly IInfoBarService _infoBarService;
    private readonly IPcbTypeEvaluationService _evaluationService;

    public PcbTypeI_OEvaluationViewModel(
        IInfoBarService infoBarService,
        ICrudService<PcbType> pcbTypeCrudService,
        IPcbTypeEvaluationService evaluationService)
    {
        FilterOptions = PcbFilterOptions.None;
        _infoBarService = infoBarService;
        _pcbTypeCrudService = pcbTypeCrudService;
        _evaluationService = evaluationService;
        //_table = new ObservableCollection<List<Dictionary<string, object>>>();

        AllPcbTypes = new List<string>();

        StartDate = DateTime.Parse("01/01/0001");
        EndDate = DateTime.Now;

        FillTable();
    }


    public IAsyncRelayCommand FilterItems { get; }

    [ObservableProperty]
    private PcbFilterOptions _filterOptions;

    [ObservableProperty]
    private StorageLocation _selectedComboBox;

    [ObservableProperty]
    public ObservableCollection<PcbType> selectedPcbTypes = new();

    [ObservableProperty]
    public List<string> _allPcbTypes;

    [ObservableProperty]
    public List<PcbType> _ptList;

    [ObservableProperty]
    public DateTime _startDate;

    [ObservableProperty]
    public DateTime _endDate;

    public List<string> Header;

    public List<List<object>> Rows = new();

    [ObservableProperty]
    public ObservableCollection<Dictionary<string, object>> _table = new();

    [ObservableProperty]
    public ObservableCollection<EvaluationPcbTypeI_ODTO> _content = new();

    private async Task GetPcbTypes()
    {

        var response = await _pcbTypeCrudService.GetAll();
        if (response != null && response.Code == ResponseCode.Success)
        {
            PtList = new List<PcbType>(response.Data.OrderBy(x => x.PcbPartNumber));
            PtList.ForEach(x => AllPcbTypes.Add(x.PcbPartNumber));
        }
        else
        {
            _infoBarService.showError("Fehler beim Laden der Sachnnummern", "Error");
        }
    }

    private async Task GetTable()
    {
        var response = await _evaluationService.GetPcbTypePosition(AllPcbTypes, StartDate, EndDate);
        if (response != null && response.Code == ResponseCode.Success)
        {

            Table = new ObservableCollection<Dictionary<string, object>>(response.Data);
            if (Table.Count > 0)
            {
                Header = new List<string>(Table[0].Keys);
                foreach (var item in Table)
                {
                    List<object> row = item.Values.ToList();
                    Rows.Add(row);
                }

            }
            Debug.WriteLine(Rows);
        }
        else
        {
            _infoBarService.showError("Fehler beim Laden des Tables", "Error");
        }
    }

    private async Task FillTable()
    {
        var response = await _evaluationService.GetPcbTypeI_O("1688400308", StartDate, EndDate);
        if (response != null && response.Code == ResponseCode.Success)
        {
            Content = new ObservableCollection<EvaluationPcbTypeI_ODTO>(response.Data);
        }
        else
        {
            _infoBarService.showError("Fehler beim Laden des Tables", "Error");
        }
    }



    private async Task GetPcbs(int pageIndex, int pageSize, bool isAscending)
    {
        Response<List<PcbDTO>> pcbs;
        Response<int> maxEntries;

        if (FilterOptions != PcbFilterOptions.None)
        {

        }
        FilterItems.NotifyCanExecuteChanged();
    }

    public async void OnNavigatedTo(object parameter)
    {
        await GetPcbTypes();
        await GetTable();
        //await FillTable();
    }

    public void OnNavigatedFrom()
    {

    }
}

