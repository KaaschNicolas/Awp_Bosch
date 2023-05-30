using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace App.ViewModels;

public partial class UpdatePcbViewModel : ObservableRecipient, INavigationAware
{
    private readonly IPcbDataService<Pcb> _pcbDataService;
    private readonly ICrudService<PcbType> _pcbTypeCrudService;
    private readonly ICrudService<StorageLocation> _storageLocationCrudService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authenticationService;

    private int _pcbId;
    private Pcb _pcbToEdit;

    [ObservableProperty]
    private DateTime _createdAt;

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

    [ObservableProperty]
    private ObservableCollection<ErrorType> _errorTypes;

    [ObservableProperty]
    private ObservableCollection<Transfer> _transfers;

    public UpdatePcbViewModel(IPcbDataService<Pcb> pcbDataService, ICrudService<StorageLocation> storageLocationCrudService, ICrudService<PcbType> pcbTypesCrudService, IInfoBarService infoBarService, INavigationService navigationService, IAuthenticationService authenticationService)
    {
        try
        {
            _storageLocationCrudService = storageLocationCrudService;
            _pcbTypeCrudService = pcbTypesCrudService;
            _infoBarService = infoBarService;
            _navigationService = navigationService;
            _authenticationService = authenticationService;
            _pcbDataService = pcbDataService;


            _storageLocations = new ObservableCollection<StorageLocation>();
            _pcbTypes = new ObservableCollection<PcbType>();
            _errorTypes = new ObservableCollection<ErrorType>();
            _transfers = new ObservableCollection<Transfer>();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    [RelayCommand]
    public async Task Save()
    {
        bool isFinalized = false;

        foreach (var transfer in _transfers)
        {
            if (transfer.StorageLocation.IsFinalDestination)
            {
                isFinalized = true;
            }
        }

        _pcbToEdit.CreatedDate = _createdAt;
        _pcbToEdit.SerialNumber = _serialNumber;
        _pcbToEdit.Finalized = isFinalized;
        _pcbToEdit.PcbTypeId = _selectedPcbType.Id;
        _pcbToEdit.Transfers = new List<Transfer>(_transfers);
        _pcbToEdit.Restriction.Name = _restriction;
        _pcbToEdit.ErrorTypes = new List<ErrorType>(_errorTypes);

        var response = await _pcbDataService.Update(_pcbId, _pcbToEdit);

        if (response != null)
        {
            if (response.Code == ResponseCode.Success)
            {
                _infoBarService.showMessage("Leiterplatte erfolgreich gespeichert", "Erfolg");

                _navigationService.NavigateTo("App.ViewModels.PcbPaginationViewModel");
            }
            else
            {
                _infoBarService.showError("Leiterplatte konnte nicht gespeichert werden", "Error");
            }
        }
        else
        {
            _infoBarService.showError("Leiterplatte konnte nicht gespeichert werden", "Error");
        }
    }

    [RelayCommand]
    public void Cancel()
    {
        _navigationService.GoBack();
    }

    public async void OnNavigatedTo(object parameter)
    {
        try
        {
            var test = parameter;
            _pcbToEdit = (Pcb)parameter;
            _pcbId = _pcbToEdit.Id;

            var result = await _pcbDataService.GetByIdEager(_pcbId);

            _pcbToEdit = result.Data;
            SerialNumber = _pcbToEdit.SerialNumber;
            CreatedAt = _pcbToEdit.CreatedDate;
            User = _pcbToEdit.Transfers[0].NotedBy;
            Restriction = _pcbToEdit.Restriction.Name;
            SelectedPcbType = _pcbToEdit.PcbType;

            var storageLocationResponse = await _storageLocationCrudService.GetAll();
            if (storageLocationResponse != null)
            {
                foreach (var item in storageLocationResponse.Data)
                {
                    _storageLocations.Add(item);
                }
            }

            var pcbResponse = await _pcbTypeCrudService.GetAll();
            if (pcbResponse != null)
            {
                foreach (var item in pcbResponse.Data)
                {
                    _pcbTypes.Add(item);
                }
            }

            _pcbToEdit.ErrorTypes.ForEach(x => _errorTypes.Add(x));
            _pcbToEdit.Transfers.ForEach(x => _transfers.Add(x));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    public void OnNavigatedFrom()
    {
        var test = "";
    }
}
