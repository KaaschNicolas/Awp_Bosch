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

namespace App.ViewModels;

public partial class PcbSingleViewModel : ObservableValidator, INavigationAware
{
    public PcbSingleViewModel ViewModel { get; }
    
    private string _serialNumber;
    public string SerialNumber
    {
        get => _serialNumber; //"0000652125";
        set
        { 
            _serialNumber = value;
            OnPropertyChanged(nameof(SerialNumber));
        }
    }

    [ObservableProperty]
    private Pcb _selectedItem;

    /*[ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private DateTime _createdDate;*/

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
    private User _notedBy;



    private int _id;
    
    private Pcb _pcb;

    private readonly ICrudService<Pcb> _crudService;
    private readonly ICrudService<StorageLocation> _storageService;
    private readonly IDialogService _dialogService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    private readonly ITransferDataService<Transfer> _transfersService;

    public PcbSingleViewModel(ICrudService<Pcb> crudService, ICrudService<StorageLocation> storageService, IInfoBarService infoBarService, IDialogService dialogService, INavigationService navigationService, ITransferDataService<Transfer> transfersService)
    {
        try
        {
            _crudService = crudService;
            _storageService = storageService;
            _dialogService = dialogService;
            _infoBarService = infoBarService;
            _navigationService = navigationService;
            _transfersService = transfersService;
            _transfers = new ObservableCollection<Transfer>();
            mockData = new()
            {
                Id = 1,
                CreatedDate = DateTime.Now,
                SerialNumber = "0000652125",
                ErrorDescription = "ErrorMessage",
                Restriction = null,
                Finalized = false,

                ErrorTypes = new List<ErrorType>()
                {
                    new ErrorType(){Id=1, Code="M320", ErrorDescription="Beschreibung:Verbindung kann nicht hergestellt werden" }

                }
            };
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

    public async void OnNavigatedTo(object parameter)
    {
        //_pcb = (Pcb)parameter; 

        var response = await _crudService.GetById(1);
        if (response.Code == ResponseCode.Success)
        {
            _pcb = response.Data as Pcb;
            
            SerialNumber = response.Data.SerialNumber;
            //_createdDate = response.Data.CreatedDate;
            Restriction = response.Data.Restriction;
            ErrorDescription = response.Data.ErrorDescription;
            ErrorTypes = response.Data.ErrorTypes;
            Finalized = response.Data.Finalized;
            if (!Finalized)
            {
                Status = "offen";
            }
            else
            {
                Status = "abgeschlossen";
            }
            PcbType = response.Data.PcbType;
            Comment = response.Data.Comment;
            Diagnose = response.Data.Diagnose;
        }
        else
        {
            _infoBarService.showError("ErrorMessage", "ErrorTitle");
        }

        var transfers = await _transfersService.GetTransfersByPcb(response.Data.Id);
        //_transfers = new ObservableCollection<Transfer>();
        if (transfers.Code == ResponseCode.Success)
        {
            foreach (var transfer in transfers.Data)
            {
                Storage= transfer.StorageLocation.StorageName;
                _transfers.Add(transfer);
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
