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

public class MD_CreatePartNumberViewModel : ObservableObject, INotifyPropertyChanged
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

    private string _description;
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged(nameof(Description));
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
        CreatePN = new RelayCommand(CreatePartNumber, CanExecuteCreate);
    }

    public bool CanExecuteCreate()
    {
        return _canExecute;
    }



    public async void CreatePartNumber()
    {

        var lpt = await _crudService.Create(new PcbType { PcbPartNumber = _pcbPartNumber, MaxTransfer = _maxTransfer, Description = _description });
        InfoBarService.showMessage("Erfolgreich Leiterplatte erstellt", "Erfolg");

    }
}
