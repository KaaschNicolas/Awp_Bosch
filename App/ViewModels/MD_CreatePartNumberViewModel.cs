using App.Contracts.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using App.Core.Services;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Specialized;
using System.ComponentModel;

namespace App.ViewModels;

public class MD_CreatePartNumberViewModel : ObservableRecipient, INotifyPropertyChanged
{

    //private readonly ISampleDataService _sampleDataService;
    private string _pcbPartNumber;
    public string PcbPartNumber
    {
        get => _pcbPartNumber;
        set
        {
            _pcbPartNumber = value;
            OnPropertyChanged(nameof(PcbPartNumber));

        }
    }

    private int _maxTransfer;
    public int MaxTransfer
    {
        get => _maxTransfer;
        set
        {
            _maxTransfer = value;
            OnPropertyChanged(nameof(MaxTransfer));
        }
    }


    private ICrudService _crudService;


    public IInfoBarService InfoBarService
    {
        get;
    }

    private bool _canExecute = true;


    public ICommand CreatePN
    {
        get;
    }



    public bool CanExecute
    {
        get
        {
            return _canExecute;
        }

        set
        {
            if (_canExecute == value)
            {
                return;
            }

            _canExecute = value;
        }
    }




    public MD_CreatePartNumberViewModel(ICrudService crudService, IInfoBarService infoBarService)
    {
        _crudService = crudService;
        InfoBarService = infoBarService;
        this._maxTransfer = 0;
        this._pcbPartNumber = string.Empty;
        CreatePN = new RelayCommand(CreatePartNumber, CanExecuteCreate);
    }

    public bool CanExecuteCreate()
    {
        return _canExecute;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public async void CreatePartNumber()
    {

        var lpt = await _crudService.Create(new PcbType { PcbPartNumber = _pcbPartNumber, MaxTransfer = _maxTransfer });
        InfoBarService.showMessage("Erfolgreich Leiterplatte erstellt", "Erfolg");

    }
}
