using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public partial class CreatePcbViewModel : ObservableRecipient, INavigationAware
{
    private readonly IPcbDataService<Pcb> _pcbDataService;
    private readonly ICrudService<PcbType> _pcbTypeCrudService;
    private readonly ICrudService<StorageLocation> _storageLocationCrudService;
    private readonly IInfoBarService _infoBarService;

    [ObservableProperty]
    private DateTime createdAt = DateTime.Now;

    [ObservableProperty]
    private User? _user = null;

    [ObservableProperty]
    private PcbType _selectedPcbTypes;

    [ObservableProperty]
    private string _serialNumber;

    [ObservableProperty]
    private string _errorCode1;

    [ObservableProperty]
    private string _errorCode2;

    [ObservableProperty]
    private string _errorDescription1;

    [ObservableProperty]
    private string _errorDescription2;

    [ObservableProperty]
    private string _restriction;

    [ObservableProperty]
    private StorageLocation _selectedStorageLocation;

    [ObservableProperty]
    private string _comment;

    private List<StorageLocation> _storageLocations = new();

    private List<PcbType> _pcbTypes = new();
    public CreatePcbViewModel(IPcbDataService<Pcb> pcbDataService, ICrudService<StorageLocation> storageLocationCrudService, ICrudService<PcbType> pcbTypesCrudService, IInfoBarService infoBarService)
    {
        _pcbDataService = pcbDataService;
        _storageLocationCrudService = storageLocationCrudService;
        _pcbTypeCrudService = pcbTypesCrudService;
        _infoBarService = infoBarService;
    }





    [RelayCommand]
    public async void Save()
    {
        Comment comment = new Comment { Content = _comment };
        Transfer transfer = new Transfer { StorageLocation = _selectedStorageLocation };
        ErrorType errorType1 = new ErrorType { Code = _errorCode1, ErrorDescription = _errorDescription1 };
        ErrorType errorType2 = new ErrorType { Code = _errorCode2, ErrorDescription = _errorDescription2 };

        var errorTypes = new List<ErrorType> { errorType1, errorType2 };

        Device device = new Device();
        Pcb pcb = new Pcb { SerialNumber = _serialNumber, Finalized = false, PcbType = _selectedPcbTypes };
        var response = await _pcbDataService.Create(transfer, pcb, errorTypes, device, comment);


        if (response != null)
        {
            if (response.Code == ResponseCode.Success)
            {
                _infoBarService.showMessage("Leiterplatte erfolgreich erstellt", "Erfolg");
            }
            else
            {
                _infoBarService.showError("Leiterplatte konnte nicht erstellt werden", "Error");
            }
        }
        else
        {
            _infoBarService.showError("Leiterplatte konnte nicht erstellt werden", "Error");
        }
    }

    public async void OnNavigatedTo(object parameter)
    {
        var pcbResponse = await _pcbTypeCrudService.GetAll();
        if (pcbResponse != null)
        {

            _pcbTypes = pcbResponse.Data;
        }

        var storageLocationResponse = await _storageLocationCrudService.GetAll();
        if (storageLocationResponse != null)
        {
            _storageLocations = storageLocationResponse.Data;
        }

    }

    public void OnNavigatedFrom()
    {

    }
}
