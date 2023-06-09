﻿using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using App.Errors;
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
    private User _createdBy;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    private PcbType _selectedPcbType;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Seriennummer muss aus genau 10 Zahlen bestehen.")]
    private string _serialNumber;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MaxLength(5, ErrorMessage = "Fehler ID darf nur 5 Zeichen enthalten")]
    private string _errorCode1;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MaxLength(5, ErrorMessage = "Fehler ID darf nur 5 Zeichen enthalten")]
    private string _errorCode2;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MaxLength(100, ErrorMessage = ValidationErrorMessage.MaxLength100)]
    private string _errorDescription1;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MaxLength(100, ErrorMessage = ValidationErrorMessage.MaxLength100)]
    private string _errorDescription2;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MaxLength(100, ErrorMessage = ValidationErrorMessage.MaxLength100)]
    private string _restriction;

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
            List<Transfer> transfers = new();
            foreach (TransferDTO transferDTO in Transfers)
            {
                Transfer transfer = transferDTO.GetTransfer();
                transfers.Add(transfer);
            }

            _pcbToEdit.ErrorTypes[0].Code = ErrorCode1;
            _pcbToEdit.ErrorTypes[0].ErrorDescription = ErrorDescription1;

            _pcbToEdit.ErrorTypes[1].Code = ErrorCode2;
            _pcbToEdit.ErrorTypes[1].ErrorDescription = ErrorDescription2;

            _pcbToEdit.CreatedDate = CreatedAt;
            _pcbToEdit.SerialNumber = SerialNumber;
            //TODO: Finalized depending on last transfer
            _pcbToEdit.Finalized = transfers.Last().StorageLocation.IsFinalDestination;
            _pcbToEdit.PcbTypeId = SelectedPcbType.Id;
            _pcbToEdit.Transfers = new List<Transfer>(transfers);
            _pcbToEdit.Restriction.Name = Restriction;
            _pcbToEdit.DiagnoseId = DiagnosePcb != null ? DiagnosePcb.Id : null;
            _pcbToEdit.ErrorTypes = new List<ErrorType>(_pcbToEdit.ErrorTypes);


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

        _pcbId = (int)parameter;

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
            CreatedBy = _pcbToEdit.Transfers[0].NotedBy;
            Restriction = _pcbToEdit.Restriction.Name;
            SelectedPcbType = _pcbToEdit.PcbType;
            ErrorCode1 = _pcbToEdit.ErrorTypes[0].Code;
            ErrorCode2 = _pcbToEdit.ErrorTypes[1].Code;
            ErrorDescription1 = _pcbToEdit.ErrorTypes[0].ErrorDescription;
            ErrorDescription2 = _pcbToEdit.ErrorTypes[1].ErrorDescription;
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
