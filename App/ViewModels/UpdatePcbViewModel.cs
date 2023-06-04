using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class UpdatePcbViewModel : ObservableRecipient, INavigationAware
{
    private readonly IPcbDataService<Pcb> _pcbDataService;
    private readonly ICrudService<PcbType> _pcbTypeCrudService;
    private readonly ICrudService<StorageLocation> _storageLocationCrudService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authenticationService;
    private readonly ICrudService<Pcb> _pcbCrudService;

    private int _pcbId;
    private Pcb _pcbToEdit;
    public DateTime MaxDate { get; private set; } = DateTime.Now;

    private DateTime _createdAt;
    public DateTime CreatedAt
    {
        get => _createdAt;
        set
        {
            if (value == DateTime.MinValue)
            {
                OnPropertyChanged(nameof(CreatedAt));
                return;
            }
            SetProperty(ref _createdAt, value);

        }
    }

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
    private Device _restriction;

    [ObservableProperty]
    private string _comment;

    [ObservableProperty]
    private ObservableCollection<StorageLocation> _storageLocations;

    [ObservableProperty]
    private ObservableCollection<PcbType> _pcbTypes;

    [ObservableProperty]
    private ObservableCollection<ErrorType> _errorTypes;

    [ObservableProperty]
    private ObservableCollection<TransferDTO> _transfers;

    private bool restrictionExists;

    public UpdatePcbViewModel(IPcbDataService<Pcb> pcbDataService, ICrudService<StorageLocation> storageLocationCrudService, ICrudService<PcbType> pcbTypesCrudService, IInfoBarService infoBarService, INavigationService navigationService, IAuthenticationService authenticationService)
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
        _transfers = new ObservableCollection<TransferDTO>();

    }



    [RelayCommand]
    public async Task Save()
    {
        bool isFinalized = false;

        List<Transfer> transfers = new();
        foreach (TransferDTO transferDTO in Transfers)
        {
            Transfer transfer = transferDTO.GetTransfer();
            transfers.Add(transfer);
            if (transfer.StorageLocation.IsFinalDestination)
            {
                isFinalized = true;
            }
        }

        _pcbToEdit.CreatedDate = CreatedAt;
        _pcbToEdit.SerialNumber = SerialNumber;
        _pcbToEdit.Finalized = isFinalized;
        _pcbToEdit.PcbTypeId = SelectedPcbType.Id;
        _pcbToEdit.Transfers = new List<Transfer>(transfers);
        _pcbToEdit.Restriction = Restriction;
        _pcbToEdit.ErrorTypes = new List<ErrorType>(ErrorTypes);

        Response<Pcb> response;

        if (!restrictionExists && _pcbToEdit.Restriction.Name != "")
        {
            response = await _pcbDataService.CreateRestrictionAndUpdate(_pcbToEdit);
        }
        else
        {
            response = await _pcbDataService.Update(_pcbId, _pcbToEdit);
        }

        if (response != null && response.Code == ResponseCode.Success)
        {
            _infoBarService.showMessage("Leiterplatte erfolgreich gespeichert", "Erfolg");
            _navigationService.GoBack();
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

        _pcbToEdit = (Pcb)parameter;
        _pcbId = _pcbToEdit.Id;

        var result = await _pcbDataService.GetByIdEager(_pcbId);

        _pcbToEdit = result.Data;

        // check if restriction exists if not create new in save method
        restrictionExists = _pcbToEdit.Restriction is null ? false : true;
        SerialNumber = _pcbToEdit.SerialNumber;
        CreatedAt = _pcbToEdit.CreatedDate;
        User = _pcbToEdit.Transfers[0].NotedBy;
        Restriction = _pcbToEdit.Restriction ??= new Device { Name = "" };
        SelectedPcbType = _pcbToEdit.PcbType;

        var storageLocationResponse = await _storageLocationCrudService.GetAll();
        if (storageLocationResponse != null)
        {
            foreach (var item in storageLocationResponse.Data)
            {
                StorageLocations.Add(item);
            }
        }

        var pcbResponse = await _pcbTypeCrudService.GetAll();
        if (pcbResponse != null)
        {
            foreach (var item in pcbResponse.Data)
            {
                PcbTypes.Add(item);
            }
        }

        _pcbToEdit.ErrorTypes.ForEach(x => ErrorTypes.Add(x));
        _pcbToEdit.Transfers.ForEach(x => Transfers.Add(new TransferDTO(x)));
    }



    public void OnNavigatedFrom()
    {

    }
}
