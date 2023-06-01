using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Services.PrintService.impl;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

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

    public Visibility RestrictionButtonVisibility;
    public Visibility RestrictionInfoBarVisibility;

    [ObservableProperty]
    private Pcb _selectedItem;

    [ObservableProperty]
    private DateTime _createdDate;

    //[ObservableProperty]
    //[NotifyDataErrorInfo]
    //[Required]
    //private string _serialNumber;

    [ObservableProperty]
    private Device _restriction;

    [ObservableProperty]
    private string _errorDescription;

    [ObservableProperty]
    private List<ErrorType> _errorTypes;

    [ObservableProperty]
    private string _firstErrorCode;

    [ObservableProperty]
    private string _secondErrorCode;

    [ObservableProperty]
    private string _firstErrorDescription;

    [ObservableProperty]
    private string _secondErrorDescription;

    [ObservableProperty]
    private bool _finalized;

    [ObservableProperty]
    private string _status;

    [ObservableProperty]
    private PcbType _pcbType;

    [ObservableProperty]
    private Comment _panelComment;

    [ObservableProperty]
    private Diagnose _diagnosePcb;

    [ObservableProperty]
    private ObservableCollection<Transfer> _transfers;

    [ObservableProperty]
    private List<Transfer> _sortedData;

    [ObservableProperty]
    private Transfer _transfer;

    [ObservableProperty]
    private string _transferComment;

    [ObservableProperty]
    private string _storage;

    [ObservableProperty]
    private string _notedBy;

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _inCirculationDays;

    [ObservableProperty]
    private string _colorDays;

    [ObservableProperty]
    private int _atLocationDays;

    [ObservableProperty]
    private string _colorTransferDays;

    private Pcb _pcb;

    [ObservableProperty]
    private ObservableCollection<Pcb> _pcbs;


    private readonly IAuthenticationService _authenticationService;
    private readonly ICrudService<StorageLocation> _storageLocationCrudService;
    private readonly ICrudService<Diagnose> _diagnoseCrudService;
    private readonly ICrudService<Pcb> _pcbCrudService;
    private readonly IPcbDataService<Pcb> _pcbDataService;
    private readonly ICrudService<StorageLocation> _storageService;
    private readonly ICrudService<Comment> _commentService;
    private readonly ICrudService<Device> _deviceService;
    private readonly IDialogService _dialogService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    private readonly ITransferDataService<Transfer> _transfersService;

    public IAsyncRelayCommand FirstAsyncCommand { get; }

    public PcbSingleViewModel(IPcbDataService<Pcb> pcbDataService, ICrudService<Pcb> pcbCrudService, ICrudService<StorageLocation> storageService, ICrudService<StorageLocation> storageLocationCrudService, ICrudService<Diagnose> diagnoseCrudService, IInfoBarService infoBarService, ICrudService<Comment> commentService, ICrudService<Device> deviceService, IDialogService dialogService, INavigationService navigationService, IAuthenticationService authenticationService, ITransferDataService<Transfer> transfersService)
    {
        _pcbDataService = pcbDataService;
        _authenticationService = authenticationService;
        _storageLocationCrudService = storageLocationCrudService;
        _diagnoseCrudService = diagnoseCrudService;
        _pcbCrudService = pcbCrudService;
        _storageService = storageService;
        _commentService = commentService;
        _deviceService = deviceService;
        _dialogService = dialogService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
        _transfersService = transfersService;
        _transfers = new ObservableCollection<Transfer>();
        _pcbs = new ObservableCollection<Pcb>();
    }


    [RelayCommand]
    public async void ShowTransfer()
    {

        var result = await _dialogService.ShowCreateTransferDialog("Weitergabe");
        if (result != null)
        {
            Transfer transfer = result.Item1;
            int? diagnoseId = result.Item2;

            transfer.PcbId = _pcb.Id;

            Response<Transfer> response;

            if (diagnoseId.HasValue)
            {
                response = await _transfersService.CreateTransfer(transfer, (int)diagnoseId);
            }
            else
            {
                response = await _transfersService.Create(transfer);
            }



            if (response.Code == ResponseCode.Success)
            {
                Refresh(_pcb);
                _infoBarService.showMessage("Weitergabe erfolgreich", "Erfolg");
            }
            else
            {
                _infoBarService.showError("Fehler bei der Weitergabe", "Error");
            }
        }
    }

    private void Refresh(object parameter)
    {
        _navigationService.NavigateTo("App.ViewModels.PcbSingleViewModel", _selectedItem);
    }

    [RelayCommand]
    public void Edit()
    {
        _navigationService.NavigateTo("App.ViewModels.UpdatePcbViewModel", _selectedItem);
    }

    [RelayCommand]
    public async void Print(Page page)
    {
        var _printService = new PrintService();
        await _printService.Print(page);
    }

    [RelayCommand]
    public async void Delete()
    {
        var result = await _dialogService.ConfirmDeleteDialogAsync("Leiterplatte Löschen", "Sind Sie sicher, dass Sie diesen Eintrag löschen wollen?");
        if (result != null && result == true)
        {
            Pcb pcbToRemove = _selectedItem;
            _pcbs.Remove(pcbToRemove);
            await _pcbDataService.Delete(pcbToRemove);
            _navigationService.NavigateTo("App.ViewModels.PcbPaginationViewModel");
            _infoBarService.showMessage("Erfolgreich Leiterplatte gelöscht", "Erfolg");
        }
        else
        {
            _infoBarService.showError("Leiterplatte konnte nicht gelöscht werden", "Fehler");
        }
    }

    [RelayCommand]
    public async void AddComment()
    {
        User currentUser = _authenticationService.currentUser();

        var result = await _dialogService.AddCommentDialog("Anmerkung hinzufügen");

        if (result != null)
        {
            result.NotedById = currentUser.Id;
            var commentResult = await _commentService.Create(result);
            _pcb.Comment = commentResult.Data;

            var response = await _pcbCrudService.Update(_pcb.Id, _pcb);
            if (response.Code == ResponseCode.Success)
            {
                PanelComment = _pcb.Comment;
                _infoBarService.showMessage("Anmerkung wurde hinzugefügt", "Erfolg");
            }
            else
            {
                _infoBarService.showMessage("Anmerkung konnte nicht hinzugefügt werden", "Fehler");
            }
        }

    }

    [RelayCommand]
    public async void AddRestriction()
    {
        var result = await _dialogService.AddRestrictionDialog("Einschränkung hinzufügen");

        if (result != null)
        {
            var deviceResult = await _deviceService.Create(result);
            Device device = deviceResult.Data;
            _pcb.Restriction = device;

            var response = await _pcbDataService.Update(_pcb.Id, _pcb);
            if (response.Code == ResponseCode.Success)
            {
                Restriction = response.Data.Restriction;
                _infoBarService.showMessage("Anmerkung wurde hinzugefügt", "Erfolg");
            }
            else
            {
                _infoBarService.showMessage("Anmerkung konnte nicht hinzugefügt werden", "Fehler");
            }
        }

    }

    public async void OnNavigatedTo(object parameter)
    {

        _pcb = (Pcb)parameter;

        _selectedItem = _pcb;

        var result = await _pcbDataService.GetByIdEager(_selectedItem.Id);

        _pcb = result.Data;

        if (_pcb.Restriction == null)
        {
            RestrictionInfoBarVisibility = Visibility.Collapsed;
            RestrictionButtonVisibility = Visibility.Visible;
        }
        else
        {
            RestrictionInfoBarVisibility = Visibility.Visible;
            RestrictionButtonVisibility = Visibility.Collapsed;
        }

        SerialNumber = _pcb.SerialNumber;
        CreatedDate = _pcb.CreatedDate;
        Restriction = _pcb.Restriction;


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
        DiagnosePcb = new Diagnose { };
        PanelComment = _pcb.Comment;
        NotedBy = (_pcb.Transfers.Last()).NotedBy.Name;
        AtLocationDays = 5;

        InCirculationDays = (int)Math.Round((DateTime.Now - _pcb.CreatedDate).TotalDays);
        if (InCirculationDays > 5)
        {
            ColorDays = "yellow";
        }
        else if (InCirculationDays > 10)
        {
            ColorDays = "red";
        }
        else
        {
            ColorDays = "green";
        }
    }





    public void OnNavigatedFrom()
    {
    }

    private void NavigateToPcbs()
    {
        _navigationService.NavigateTo("App.ViewModels.PcbPaginationViewModel");
    }

}