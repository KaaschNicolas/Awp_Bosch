using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class CreatePcbViewModel : ObservableRecipient, INavigationAware
{
    private readonly ICrudService<Pcb> _pcbCrudService;
    private readonly ICrudService<PcbType> _pcbTypeCrudService;
    private readonly ICrudService<StorageLocation> _storageLocationCrudService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authenticationService;

    [ObservableProperty]
    private DateTime createdAt = DateTime.Now;

    [ObservableProperty]
    private User _user;

    [ObservableProperty]
    private PcbType _selectedPcbType;

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

    [ObservableProperty]
    private ObservableCollection<StorageLocation> _storageLocations;

    [ObservableProperty]
    private ObservableCollection<PcbType> _pcbTypes;
    public CreatePcbViewModel(ICrudService<Pcb> pcbCrudService, ICrudService<StorageLocation> storageLocationCrudService, ICrudService<PcbType> pcbTypesCrudService, IInfoBarService infoBarService, INavigationService navigationService, IAuthenticationService authenticationService)
    {
        _pcbCrudService = pcbCrudService;
        _storageLocationCrudService = storageLocationCrudService;
        _pcbTypeCrudService = pcbTypesCrudService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
        _authenticationService = authenticationService;

        _storageLocations = new ObservableCollection<StorageLocation>();
        _pcbTypes = new ObservableCollection<PcbType>();
    }

    [RelayCommand]
    public async Task Save()
    {
        Transfer transfer = new Transfer { StorageLocationId = _selectedStorageLocation.Id, Comment = _comment, NotedById = User.Id };
        ErrorType errorType1 = new ErrorType { Code = _errorCode1, ErrorDescription = _errorDescription1 };
        ErrorType errorType2 = new ErrorType { Code = _errorCode2, ErrorDescription = _errorDescription2 };
        Device restriction = new Device { Name = _restriction };
        var errorTypes = new List<ErrorType> { errorType1, errorType2 };

        var transfers = new List<Transfer>() { transfer };

        
        Pcb pcb = new Pcb
        {
            SerialNumber = _serialNumber,
            Finalized = false,
            PcbTypeId = _selectedPcbType.Id,
            Transfers = transfers,
            Restriction = restriction,
            ErrorTypes = errorTypes,
        };
        var response = await _pcbCrudService.Create(pcb);


        if (response != null)
        {
            if (response.Code == ResponseCode.Success)
            {
                _infoBarService.showMessage("Leiterplatte erfolgreich erstellt", "Erfolg");
                // TODO: when List View exists, navigate to it after successfull creation of Pcb
                // _navigationService.NavigateTo("App.ViewModels.ListPcbViewModel");
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
            foreach (var item in pcbResponse.Data)
            {
                _pcbTypes.Add(item);
            }
        }

        var storageLocationResponse = await _storageLocationCrudService.GetAll();
        if (storageLocationResponse != null)
        {
            foreach (var item in storageLocationResponse.Data)
            {
                _storageLocations.Add(item);
            }

        }

        User = _authenticationService.currentUser();

    }

    public void OnNavigatedFrom()
    {

    }
}
