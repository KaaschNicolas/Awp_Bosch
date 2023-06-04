using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels;

public partial class UpdatePcbViewModel : ObservableValidator, INavigationAware
{
    private readonly IPcbDataService<Pcb> _pcbDataService;
    private readonly ICrudService<PcbType> _pcbTypeCrudService;
    private readonly ICrudService<StorageLocation> _storageLocationCrudService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authenticationService;
    private readonly ICrudService<Pcb> _pcbCrudService;
    private readonly ICrudService<Diagnose> _diagnoseCrudService;

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
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Sachnummer muss aus genau 10 Zahlen bestehen.")]
    private PcbType _selectedPcbType;

    [ObservableProperty]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Seriennummer muss aus genau 10 Zahlen bestehen.")]
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
    private Diagnose _diagnosePcb;

    [ObservableProperty]
    private ObservableCollection<Diagnose> _diagnoses;

    [ObservableProperty]
    private ObservableCollection<StorageLocation> _storageLocations;

    [ObservableProperty]
    private ObservableCollection<PcbType> _pcbTypes;

    [ObservableProperty]
    private ObservableCollection<ErrorType> _errorTypes;

    [ObservableProperty]
    private ObservableCollection<TransferDTO> _transfers;


    public UpdatePcbViewModel(IPcbDataService<Pcb> pcbDataService, ICrudService<Diagnose> diagnoseCrudService, ICrudService<StorageLocation> storageLocationCrudService, ICrudService<PcbType> pcbTypesCrudService, IInfoBarService infoBarService, INavigationService navigationService, IAuthenticationService authenticationService)
    {

        _storageLocationCrudService = storageLocationCrudService;
        _pcbTypeCrudService = pcbTypesCrudService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
        _authenticationService = authenticationService;
        _pcbDataService = pcbDataService;
        _diagnoseCrudService = diagnoseCrudService;

        _storageLocations = new ObservableCollection<StorageLocation>();
        _pcbTypes = new ObservableCollection<PcbType>();
        _errorTypes = new ObservableCollection<ErrorType>();
        _transfers = new ObservableCollection<TransferDTO>();
        _diagnoses = new ObservableCollection<Diagnose>();

    }

    [RelayCommand]
    public async Task Save()
    {
        ValidateAllProperties();
        if (!HasErrors)
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
            _pcbToEdit.Diagnose = DiagnosePcb;
            _pcbToEdit.ErrorTypes = new List<ErrorType>(ErrorTypes);


            var response = await _pcbDataService.Update(_pcbId, _pcbToEdit);


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
        if (result != null && result.Code == ResponseCode.Success)
        {
            _pcbToEdit = result.Data;

            // needs to happen before DiagnosePcb gets assigned, otherwise Diagnose won't display
            var diagnoseResponse = await _diagnoseCrudService.GetAll();
            if (diagnoseResponse != null && diagnoseResponse.Code == ResponseCode.Success)
            {
                diagnoseResponse.Data.ForEach(x => Diagnoses.Add(x));
            }
            else
            {
                _infoBarService.showError("Fehler beim Laden der Fehlerkategorien", "Error");
            }

            var storageLocationResponse = await _storageLocationCrudService.GetAll();
            if (storageLocationResponse != null && storageLocationResponse.Code == ResponseCode.Success)
            {
                storageLocationResponse.Data.ForEach(x => StorageLocations.Add(x));
            }
            else
            {
                _infoBarService.showError("Fehler beim Laden der Lagerorte", "Error");
            }

            var pcbTypeResponse = await _pcbTypeCrudService.GetAll();
            if (pcbTypeResponse != null && pcbTypeResponse.Code == ResponseCode.Success)
            {
                pcbTypeResponse.Data.ForEach(x => PcbTypes.Add(x));
            }
            else
            {
                _infoBarService.showError("Fehler beim Laden der Sachnummern", "Error");
            }


            SerialNumber = _pcbToEdit.SerialNumber;
            CreatedAt = _pcbToEdit.CreatedDate;
            DiagnosePcb = _pcbToEdit.Diagnose;
            User = _pcbToEdit.Transfers[0].NotedBy;
            Restriction = _pcbToEdit.Restriction;
            SelectedPcbType = _pcbToEdit.PcbType;


            _pcbToEdit.ErrorTypes.ForEach(x => ErrorTypes.Add(x));
            _pcbToEdit.Transfers.ForEach(x => Transfers.Add(new TransferDTO(x)));
        }
        else
        {
            _infoBarService.showError("Fehler beim Laden der Leiterplatte", "Error");
        }
    }



    public void OnNavigatedFrom()
    {

    }
}
