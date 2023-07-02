
using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using App.Errors;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels;

public partial class CreatePcbViewModel : ObservableValidator, INavigationAware
{
    private readonly ICrudService<Pcb> _pcbCrudService;
    private readonly ICrudService<PcbType> _pcbTypeCrudService;
    private readonly ICrudService<StorageLocation> _storageLocationCrudService;
    private readonly ICrudService<Diagnose> _diagnoseCrudService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authenticationService;


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
    public DateTime MaxDate { get; private set; } = DateTime.Now;

    [ObservableProperty]
    private User _createdBy;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    private PcbType _selectedPcbType;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Seriennummer muss genau 10 Zahlen besitzen.")]
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
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    private StorageLocation _selectedStorageLocation;

    [ObservableProperty]
    private Diagnose _selectedDiagnose;

    [ObservableProperty]
    private string _comment;

    [ObservableProperty]
    private ObservableCollection<StorageLocation> _storageLocations;

    [ObservableProperty]
    private ObservableCollection<PcbType> _pcbTypes;


    [ObservableProperty]
    private ObservableCollection<Diagnose> _diagnoses;
    public CreatePcbViewModel(ICrudService<Pcb> pcbCrudService, ICrudService<Diagnose> diagnoseCrudService, ICrudService<StorageLocation> storageLocationCrudService, ICrudService<PcbType> pcbTypesCrudService, IInfoBarService infoBarService, INavigationService navigationService, IAuthenticationService authenticationService)
    {
        _pcbCrudService = pcbCrudService;
        _storageLocationCrudService = storageLocationCrudService;
        _pcbTypeCrudService = pcbTypesCrudService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
        _authenticationService = authenticationService;
        _diagnoseCrudService = diagnoseCrudService;

        _storageLocations = new ObservableCollection<StorageLocation>();
        _pcbTypes = new ObservableCollection<PcbType>();
        _diagnoses = new ObservableCollection<Diagnose>();

    }

    [RelayCommand]
    public async Task Save()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            Transfer transfer = new Transfer
            {
                StorageLocationId = _selectedStorageLocation.Id,
                Comment = _comment,
                NotedById = _createdBy.Id,
                CreatedDate = _createdAt
            };
            ErrorType errorType1 = new ErrorType { Code = _errorCode1, ErrorDescription = _errorDescription1 };
            ErrorType errorType2 = new ErrorType { Code = _errorCode2, ErrorDescription = _errorDescription2 };
            Device restriction = new Device { Name = _restriction ??= "" };
            var errorTypes = new List<ErrorType> { errorType1, errorType2 };
            var transfers = new List<Transfer>() { transfer };

            Pcb pcb = new Pcb
            {
                CreatedDate = CreatedAt,
                SerialNumber = SerialNumber,
                Finalized = SelectedStorageLocation.IsFinalDestination,
                PcbTypeId = SelectedPcbType.Id,
                Transfers = transfers,
                Restriction = restriction,
                ErrorTypes = errorTypes,
                DiagnoseId = SelectedDiagnose != null ? SelectedDiagnose.Id : null,
            };
            var response = await _pcbCrudService.Create(pcb);

            if (response != null)
            {
                if (response.Code == ResponseCode.Success)
                {
                    _infoBarService.showMessage("Leiterplatte erfolgreich erstellt", "Erfolg");
                    _navigationService.NavigateTo("App.ViewModels.PcbPaginationViewModel");
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

    }

    [RelayCommand]
    public void Cancel()
    {
        _navigationService.NavigateTo("App.ViewModels.PcbPaginationViewModel");
    }

    public async void OnNavigatedTo(object parameter)
    {
        _createdAt = DateTime.Now;

        var pcbResponse = await _pcbTypeCrudService.GetAll();
        if (pcbResponse != null)
        {
            pcbResponse.Data.ForEach(_pcbTypes.Add);
        }

        var storageLocationResponse = await _storageLocationCrudService.GetAll();
        if (storageLocationResponse != null)
        {
            storageLocationResponse.Data.ForEach(_storageLocations.Add);

        }

        var diagnoseResponse = await _diagnoseCrudService.GetAll();
        if (diagnoseResponse != null)
        {
            diagnoseResponse.Data.ForEach(_diagnoses.Add);
        }

        CreatedBy = _authenticationService.CurrentUser;

    }
        
    public void OnNavigatedFrom()
    {

    }
}
