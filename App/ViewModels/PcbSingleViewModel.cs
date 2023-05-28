using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static ZXing.QrCode.Internal.Version;

namespace App.ViewModels;

public partial class PcbSingleViewModel : ObservableValidator, INavigationAware
{
    public PcbSingleViewModel ViewModel { get; }
    
    private string _serialNumber = "0000652125";
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
    private string _serialNumber;*/

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
    private bool _finalized;

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

    private int _id;
    
    private Pcb _pcb;

    private readonly ICrudService<Pcb> _crudService;
    private readonly IDialogService _dialogService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;

    public PcbSingleViewModel(IPcbDataService<Pcb> crudService, IInfoBarService infoBarService, IDialogService dialogService, INavigationService navigationService)
    {
        _crudService = crudService;
        _dialogService = dialogService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
        mockData = new()
        {
            Id = 1,
            CreatedDate=DateTime.Now,
            SerialNumber="0000652125",
            ErrorDescription="ErrorMessage",
            Finalized=false,
            ErrorTypes= new List<ErrorType>()
            {
                new ErrorType(){Id=1, Code="M320", ErrorDescription="Beschreibung:Verbindung kann nicht hergestellt werden" }

            }
        };
    }

    [RelayCommand]
    public async void Delete()
    {
        var result = await _dialogService.ConfirmDeleteDialogAsync("Leiterplatte Löschen", "Sind Sie sicher, dass Sie diesen Eintrag löschen möchten?");
        if (result != null && result == true)
        {
            //TODO: entkommentieren wenn einzel Ansicht passt
            //Pcb pcbToRemove = _selectedItem;
            //_pcb.Remove(pcbToRemove);
            //await _crudService.Delete(_pcb);
            _infoBarService.showMessage("Erfolgreich Leiterplatte gelöscht", "Erfolg");
            _navigationService.NavigateTo("App.ViewModels.PcbPaginationViewModel");

        }
    }

    Pcb mockData { get; set; }

    public void OnNavigatedTo(object parameter)
    {
        //_pcb = (Pcb)parameter;
        _pcb=new Pcb();
        //_id = _pcb.Id;
        _serialNumber = "0000652125";//_pcb.SerialNumber;
        //_restriction = _pcb.Restriction;
        _errorDescription = "Fehlerbeschreibung";// _pcb.ErrorDescription;
        /*_errorTypes = _pcb.ErrorTypes;
        _finalized = false;
        _pcbType = _pcb.PcbType;
        _comment = _pcb.Comment;
        _diagnose =  _pcb.Diagnose;*/
    }

    public void OnNavigatedFrom()
    {
    }
}
