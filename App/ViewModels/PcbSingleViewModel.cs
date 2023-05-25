using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using static ZXing.QrCode.Internal.Version;
using App.Core.Services;

namespace App.ViewModels;

public partial class PcbSingleViewModel : ObservableValidator, INavigationAware
{
    public PcbSingleViewModel ViewModel { get; }
    
    private string _serialNumber;
    public string SerialNumber
    {
        get => _serialNumber; 
        set
        { 
            _serialNumber = value;
            OnPropertyChanged(nameof(SerialNumber));
        }
    }

    [ObservableProperty]
    private Pcb _selectedItem;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private DateTime _createdDate;

    //[ObservableProperty]
    //[NotifyDataErrorInfo]
    //[Required]
    //private string _serialNumber;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private Device _restriction;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _errorDescription;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private List<ErrorType> _errorTypes;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _firstErrorCode; 
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _secondErrorCode;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _firstErrorDescription;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _secondErrorDescription;

    /*[ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _code;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _errorDescription2;*/

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private bool _finalized;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _status;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private PcbType _pcbType;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private Comment _comment;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private Diagnose _diagnose;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private ObservableCollection<Transfer> _transfers;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private Transfer _transfer;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _transferComment;

    [ObservableProperty]
    private string _storage;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _notedBy;


    [ObservableProperty]
    private int _id;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private int _inCirculationDays;
    
    [ObservableProperty]
    [Required]
    private string _colorDays;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private int _atLocationDays;

    [ObservableProperty]
    [Required]
    private string _colorTransferDays;


    private Pcb _pcb;


    private readonly IAuthenticationService _authenticationService;
    private readonly ICrudService<StorageLocation> _storageLocationCrudService;
    private readonly ICrudService<Diagnose> _diagnoseCrudService;
    private readonly ICrudService<Pcb> _crudService;
    private readonly ICrudService<StorageLocation> _storageService;
    private readonly IDialogService _dialogService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    private readonly ITransferDataService<Transfer> _transfersService;

    public PcbSingleViewModel(ICrudService<Pcb> crudService, ICrudService<StorageLocation> storageService, ICrudService<StorageLocation> storageLocationCrudService, ICrudService<Diagnose> diagnoseCrudService, IInfoBarService infoBarService, IDialogService dialogService, INavigationService navigationService, IAuthenticationService authenticationService, ITransferDataService<Transfer> transfersService)
    {
        try
        {
            _crudService = crudService;
            _authenticationService = authenticationService;
            _storageLocationCrudService = storageLocationCrudService;
            _diagnoseCrudService = diagnoseCrudService;
            _storageService = storageService;
            _dialogService = dialogService;
            _infoBarService = infoBarService;
            _navigationService = navigationService;
            _transfersService = transfersService;
            _transfers = new ObservableCollection<Transfer>();
        }
        catch(Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    [RelayCommand]
    public async void Delete()
    {
        var result = await _dialogService.ConfirmDeleteDialogAsync("Leiterplatte Löschen", "Sind Sie sicher, dass Sie diesen Eintrag löschen möchten?");
        if (result != null && result == true)
        {
            //Pcb pcbToRemove = _selectedItem;
            //_pcb.Remove(pcbToRemove);
            //await _crudService.Delete(pcbToRemove);
            _infoBarService.showMessage("Erfolgreich Leiterplatte gelöscht", "Erfolg");
        }
    }

    Pcb mockData { get; set; }

    [RelayCommand]
    public async void ShowTransfer()
    {
        User currentUser = _authenticationService.currentUser();
        var storageLocationsResponse = await _storageLocationCrudService.GetAll();
        var storageLocations = new List<StorageLocation>();
        if (storageLocationsResponse.Code == ResponseCode.Success)
        {
            storageLocations = storageLocationsResponse.Data;
        }

        var diagnoseResponse = await _diagnoseCrudService.GetAll();
        var diagnoses = new List<Diagnose>();
        if (diagnoseResponse.Code == ResponseCode.Success)
        {
            diagnoses = diagnoseResponse.Data;
        }
        var result = await _dialogService.ShowCreateTransferDialog("Weitergabe", currentUser, storageLocations, diagnoses);
        if (result != null)
        {
            Transfer transfer = result.Item1;
            int diagnoseId = result.Item2;

            transfer.PcbId = _selectedItem.Id;

            var response = await _transfersService.CreateTransfer(transfer, diagnoseId);
            if (response == ResponseCode.Success)
            {
                _infoBarService.showMessage("Weitergabe erfolgreich", "Erfolg");
            }
            else
            {
                _infoBarService.showError("Fehler bei der Weitergabe", "Error");

            }


        }


    }

    public async void OnNavigatedTo(object parameter)
    {
        _pcb = (Pcb)parameter; 

        SerialNumber = _pcb.SerialNumber;
        CreatedDate = _pcb.CreatedDate;
        Restriction = _pcb.Restriction;
        ErrorDescription = _pcb.ErrorDescription;
        ErrorTypes = _pcb.ErrorTypes;
        if (ErrorTypes != null)
        {
            FirstErrorCode = ErrorTypes[0].Code;
            FirstErrorDescription = ErrorTypes[0].ErrorDescription;

            if (ErrorTypes[1] != null)
            {
               SecondErrorCode = ErrorTypes[1].Code;
               SecondErrorDescription = ErrorTypes[1].ErrorDescription;
            }
            else
            {
               SecondErrorCode = " nicht vorhanden";
               SecondErrorDescription = " nicht vorhanden";
            }
        }
        else
        {
            FirstErrorCode = " nicht vorhanden";
            FirstErrorDescription = " nicht vorhanden";
            SecondErrorCode = " nicht vorhanden";
            SecondErrorDescription = " nicht vorhanden";
        }
        
        Finalized = _pcb.Finalized;
        if (!Finalized)
        {
            Status = "offen";
        }
        else
        {
            Status = "abgeschlossen";
        }
        PcbType = _pcb.PcbType;
        Comment = _pcb.Comment;
        Diagnose = _pcb.Diagnose;
        //NotedBy = _pcb.NotedBy;

        InCirculationDays = (int)Math.Round((DateTime.Now - _pcb.CreatedDate).TotalDays);
        if(InCirculationDays > 5) 
        {
            ColorDays = "yellow";
        }
        else if (InCirculationDays > 10)
        {
            ColorDays = "red";
        }
        else
        {
            ColorDays="green";
        }

        var transfers = await _transfersService.GetTransfersByPcb(_pcb.Id);
        
        //_transfers = new ObservableCollection<Transfer>();
        if (transfers.Code == ResponseCode.Success)
        {
            for (int i = 0; i < (transfers.Data).Count; i++)//each (var transfer in transfers.Data)
            {
                var transfer = (transfers.Data)[i];
                transfer.Id = i + 1;
                //if (transfer == (transfers.Data)[0])
                //{
                //    transfer.Id = 1;
                //}
                //else
                //{
                    
                //}
                NotedBy = transfer.NotedBy.Name;
                Storage= transfer.StorageLocation.StorageName;
                _transfers.Add(transfer);

                if(transfer == transfers.Data[transfers.Data.Count - 1])
                {
                    var AtLocationDays = (int)Math.Round((transfer.CreatedDate - DateTime.Now).TotalDays);
                    if(AtLocationDays > transfer.StorageLocation.DwellTimeYellow)
                    {
                        ColorTransferDays = "yellow";
                    }
                    else if(AtLocationDays > transfer.StorageLocation.DwellTimeRed)
                    {
                        ColorTransferDays = "red";
                    }
                    else
                    {
                        ColorTransferDays = "green";
                    }
                }
            }
        }
        else
        {
            _infoBarService.showError("Couldn't load transfer list", "Transfer List");
        }
        
    }

    public void OnNavigatedFrom()
    {
    }
}
