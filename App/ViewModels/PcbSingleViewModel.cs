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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

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
    private Comment _panelComment;

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
    private List<Transfer> _sortedData;

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

    [ObservableProperty]
    private ObservableCollection<Pcb> _pcbs;


    private readonly IAuthenticationService _authenticationService;
    private readonly ICrudService<StorageLocation> _storageLocationCrudService;
    private readonly ICrudService<Diagnose> _diagnoseCrudService;
    private readonly IPcbDataService<Pcb> _pcbDataService;
    private readonly ICrudService<StorageLocation> _storageService;
    private readonly ICrudService<Comment> _commentService;
    private readonly ICrudService<Device> _deviceService;
    private readonly IDialogService _dialogService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    private readonly ITransferDataService<Transfer> _transfersService;

    public IAsyncRelayCommand FirstAsyncCommand { get; }

    public PcbSingleViewModel(IPcbDataService<Pcb> pcbDataService, ICrudService<StorageLocation> storageService, ICrudService<StorageLocation> storageLocationCrudService, ICrudService<Diagnose> diagnoseCrudService, IInfoBarService infoBarService, ICrudService<Comment> commentService, ICrudService<Device> deviceService, IDialogService dialogService, INavigationService navigationService, IAuthenticationService authenticationService, ITransferDataService<Transfer> transfersService)
    {
        try
        {
            _pcbDataService = pcbDataService;
            _authenticationService = authenticationService;
            _storageLocationCrudService = storageLocationCrudService;
            _diagnoseCrudService = diagnoseCrudService;
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
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    /*[RelayCommand]
    public async void Delete()
    {
        //var pcbResponse = await _pcbDataService.GetAll();
        //if (pcbResponse.Code == ResponseCode.Success)
        //{
        //    foreach (var pcb in pcbResponse)
        //    {
        //        Pcbs.Add(pcb);
        //    }
        //}

        var result = await _dialogService.ConfirmDeleteDialogAsync("Leiterplatte Löschen", "Sind Sie sicher, dass Sie diesen Eintrag löschen möchten?");
        if (result != null && result == true)
        {
            //Pcb pcbToRemove = SelectedItem;
            //Pcbs.Remove(pcbToRemove);
            //await _pcbDataService.Delete(pcbToRemove);
            _infoBarService.showMessage("Erfolgreich Leiterplatte gelöscht", "Erfolg");
            //NavigateToPcbs();
        }
    }*/

    Pcb mockData { get; set; }

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

        if(result != null)
        {
            result.NotedById = currentUser.Id;
            var commentResult = await _commentService.Create(result);
            Comment comment = commentResult.Data;
            _pcb.Comment = comment;

            var response = await _pcbDataService.Update(_pcb.Id, _pcb);
            if (response.Code == ResponseCode.Success)
            {
                PanelComment = response.Data.Comment;
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
        try
        {
            _pcb = (Pcb)parameter;

            var pcbResponse = await _pcbDataService.GetAll();
            if (pcbResponse.Data != null)
            {
                foreach (var item in pcbResponse.Data)
                {
                    _pcbs.Add(item);
                }
            }

            _selectedItem = _pcb;
            Id = _pcb.Id;

            var result = await _pcbDataService.GetByIdEager(Id);

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
            /*
            if(Restriction == null)
            {
                IsButtonVisible = true;
                RestrictionVisibility = "Visible";
            }
            else
            {
                IsButtonVisible = false;
                RestrictionVisibility = "Collapsed";
            }*/
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
            PanelComment = _pcb.Comment;
            Diagnose = _pcb.Diagnose;
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


            var transfers = await _transfersService.GetTransfersByPcb(_pcb.Id);

            //_transfers = new ObservableCollection<Transfer>();
            if (transfers.Code == ResponseCode.Success)
            {
                for (int i = 0; i < (transfers.Data).Count; i++)
                {
                    var transfer = (transfers.Data)[i];
                    transfer.Id = i + 1;
                    NotedBy = transfer.NotedBy.Name;
                    Storage = transfer.StorageLocation.StorageName;
                    Transfers.Add(transfer);

                    AtLocationDays = (int)Math.Round((DateTime.Now - transfer.CreatedDate).TotalDays);

                    if (transfer == transfers.Data[transfers.Data.Count - 1])
                    {
                        AtLocationDays = (int)Math.Round((DateTime.Now - transfer.CreatedDate).TotalDays);
                        if (AtLocationDays > int.Parse(transfer.StorageLocation.DwellTimeYellow))
                        {
                            ColorTransferDays = "yellow";
                        }
                        else if (AtLocationDays > int.Parse(transfer.StorageLocation.DwellTimeRed))
                        {
                            ColorTransferDays = "red";
                        }
                        else
                        {
                            ColorTransferDays = "green";
                        }
                    }
                }

                SortedData = transfers.Data.ToList();
                SortedData.Reverse();
            }
            else
            {
                _infoBarService.showError("Couldn't load transfer list", "Transfer List");
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
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
