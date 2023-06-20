using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services;
using App.Core.Services.Interfaces;
using App.Helpers;
using App.Messages;
using App.Models;
using App.Services.PrintService.impl;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class PcbSingleViewModel : ObservableValidator, INavigationAware
{
    public PcbSingleViewModel ViewModel { get; }

    [ObservableProperty]
    private string _serialNumber;

    [ObservableProperty]
    private Visibility _restrictionButtonVisibility;

    [ObservableProperty]
    private Visibility _restrictionInfoBarVisibility;

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
    private ObservableCollection<Transfer> _sortedData;

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
    private int _atLocationDays;

    [ObservableProperty]
    private SolidColorBrush _colorTransferDays;

    private Pcb _pcb;
    private int _pcbId;

    [ObservableProperty]
    private ObservableCollection<Pcb> _pcbs;

    private readonly IAuthenticationService _authenticationService;
    private readonly ICrudService<Pcb> _pcbCrudService;
    private readonly IPcbDataService<Pcb> _pcbDataService;
    private readonly ICrudService<Comment> _commentService;
    private readonly IDialogService _dialogService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    private readonly ITransferDataService<Transfer> _transfersService;

    public PcbSingleViewModel(
        IPcbDataService<Pcb> pcbDataService,
        ICrudService<Pcb> pcbCrudService,
        IInfoBarService infoBarService,
        ICrudService<Comment> commentService,
        IDialogService dialogService,
        INavigationService navigationService,
        IAuthenticationService authenticationService,
        ITransferDataService<Transfer> transfersService)
    {
        _pcbDataService = pcbDataService;
        _authenticationService = authenticationService;
        _pcbCrudService = pcbCrudService;
        _commentService = commentService;
        _dialogService = dialogService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
        _transfersService = transfersService;
        _transfers = new ObservableCollection<Transfer>();
        _pcbs = new ObservableCollection<Pcb>();
    }

    public PcbSingleViewModel(){}


    [RelayCommand]
    public async void ShowTransfer()
    {
        var response = await _dialogService.ShowCreateTransferDialog();
        if (response != null && response.Code == ResponseCode.Success)
        {
            var addedTransfer = response.Data;
            addedTransfer.Id = SortedData.Count() + 1;
            SortedData.Insert(0, addedTransfer);
            _infoBarService.showMessage("Weitergabe erfolgreich", "Erfolg");
            await Load();
        }
        else if ((response != null && response.Code == ResponseCode.Error) || response == null)
        {
            _infoBarService.showError("Fehler bei der Weitergabe", "Error");
        }
    }

    [RelayCommand]
    public void Edit()
    {
        _navigationService.NavigateTo("App.ViewModels.UpdatePcbViewModel", SelectedItem.Id);
    }

    [RelayCommand]
    public async void Print()
    {
        IDataMatrixService _dmService = new DataMatrixService();
        var dmImage = _dmService.GetDataMatrix(SerialNumber);
        var dmImageConverted = BitmapToBitmapImageConverter.Convert(dmImage);
        var pcbPrintPageDto = new PcbPrintPageDTO()
        {
            Seriennummer = SerialNumber,
            Sachnummer = PcbType.PcbPartNumber,
            Datamatrix = dmImageConverted,
            Einschraenkung = Restriction.Name,
            Panel = PanelComment,
            Status = Status,
            UmlaufTage = InCirculationDays,
            AktuellerStandort = Storage,
            Verweildauer = AtLocationDays,
            LetzteBearbeitung = NotedBy,
            Oberfehler = FirstErrorCode,
            OberfehlerBeschreibung = FirstErrorDescription,
            Unterfehler = SecondErrorCode,
            UnterfehlerBeschreibung = SecondErrorDescription
        };
        //var printPageModel = new PrintPageModel(SerialNumber, PcbType.PcbPartNumber, dmImageConverted, Restriction.Name, PanelComment, Status, InCirculationDays, Storage ,AtLocationDays, NotedBy, FirstErrorCode, FirstErrorDescription, SecondErrorCode, SecondErrorDescription);
        var printPageModel = new PrintPageModel(pcbPrintPageDto);
        var _printService = new PrintService();
        _printService.Print(printPageModel);
    }

    [RelayCommand]
    public async void Delete()
    {
        var result = await _dialogService.ConfirmDeleteDialogAsync("Leiterplatte Löschen", "Sind Sie sicher, dass Sie diesen Eintrag löschen wollen?");
        if (result != null && result == true)
        {
            Pcb pcbToRemove = SelectedItem;
            Pcbs.Remove(pcbToRemove);
            await _pcbDataService.Delete(pcbToRemove.Id);
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
        User currentUser = _authenticationService.CurrentUser;

        var result = await _dialogService.AddCommentDialog("Anmerkung hinzufügen");

        if (result != null)
        {
            result.NotedById = currentUser.Id;
            var commentResult = await _commentService.Create(result);
            Comment comment = commentResult.Data;
            _pcb.Comment = comment;

            var response = await _pcbCrudService.Update(_pcb.Id, _pcb);
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
            _pcb.Restriction = result;
            var response = await _pcbCrudService.Update(_pcb.Id, _pcb);
            if (response != null && response.Code == ResponseCode.Success)
            {
                Restriction = response.Data.Restriction;
                showRestriction();
                _infoBarService.showMessage("Anmerkung wurde hinzugefügt", "Erfolg");
            }
            else
            {
                _infoBarService.showMessage("Anmerkung konnte nicht hinzugefügt werden", "Fehler");
            }
        }

    }

    private void showAddRestrictionButton()
    {
        RestrictionInfoBarVisibility = Visibility.Collapsed;
        RestrictionButtonVisibility = Visibility.Visible;
    }

    private void showRestriction()
    {
        RestrictionInfoBarVisibility = Visibility.Visible;
        RestrictionButtonVisibility = Visibility.Collapsed;
    }

    public async Task Load()
    {
        var result = await _pcbDataService.GetByIdEager(_pcbId);
        _pcb = result.Data;
        //TODO: Check if selectedItem can be replaced with _pcb
        SelectedItem = _pcb;

        if (_pcb.Restriction.Name == "")
        {
            showAddRestrictionButton();
        }
        else
        {
            showRestriction();
        }

        SerialNumber = _pcb.SerialNumber;
        CreatedDate = _pcb.CreatedDate;
        Restriction = _pcb.Restriction;
        ErrorDescription = _pcb.ErrorDescription;
        ErrorTypes = _pcb.ErrorTypes;
        if (ErrorTypes[0].Code != null && ErrorTypes[0].ErrorDescription != null && ErrorTypes != null)
        {
            FirstErrorCode = ErrorTypes[0].Code;
            FirstErrorDescription = ErrorTypes[0].ErrorDescription;

            if (ErrorTypes[1].Code != null && ErrorTypes[1].ErrorDescription != null)
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
        Status = Finalized ? "abgeschlossen" : "offen";
        PcbType = _pcb.PcbType;
        PanelComment = _pcb.Comment;
        DiagnosePcb = _pcb.Diagnose;
        NotedBy = _pcb.Transfers.Last().NotedBy.Name;

        InCirculationDays = (int)Math.Round((DateTime.Now - _pcb.CreatedDate).TotalDays);

        var transfers = await _transfersService.GetTransfersByPcb(_pcb.Id);

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
                    if (!transfer.StorageLocation.DwellTimeYellow.Equals("--") && !transfer.StorageLocation.DwellTimeRed.Equals("--"))
                    {
                        if (AtLocationDays >= int.Parse(transfer.StorageLocation.DwellTimeRed))
                        {
                            ColorTransferDays = new SolidColorBrush(Colors.Red);
                        }
                        else if (AtLocationDays >= int.Parse(transfer.StorageLocation.DwellTimeYellow))
                        {
                            ColorTransferDays = new SolidColorBrush(Colors.Yellow);
                        }
                        else
                        {
                            ColorTransferDays = new SolidColorBrush(Colors.LimeGreen);
                        }
                    }
                    else
                    {
                        ColorTransferDays = new SolidColorBrush(Colors.Transparent);
                    }
                }
            }
            SortedData = new ObservableCollection<Transfer>(transfers.Data.ToList().OrderByDescending(x => x.CreatedDate));
        }
        else
        {
            _infoBarService.showError("Couldn't load transfer list", "Transfer List");
        }
    }


    public async void OnNavigatedTo(object parameter)
    {
        _pcbId = (int)parameter;

        // Register Messenger used with ShowTransfer
        WeakReferenceMessenger.Default.Register<PcbSingleViewModel, CurrentPcbRequestMessage>(this, (r, m) =>
        {
            m.Reply(_pcbId);
        });

        await Load();
    }

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

}