using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Contracts.Services;
using App.Core.Models;
using App.Core.Services;
using App.Core.Services.Interfaces;
using App.Models;
using App.Services;
using App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using WinUIEx;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.ViewModels;

// public class MDPartNumberViewModel : ObservableRecipient, INavigationAware

public class MDPartNumberViewModel : ObservableRecipient, INotifyPropertyChanged, INavigationAware
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


    private ICrudService<PcbType> _crudService;


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


    public ObservableCollection<PcbType> PcbTypes { get; } = new ObservableCollection<PcbType>();

    public MDPartNumberViewModel(ICrudService<PcbType> crudService, IInfoBarService infoBarService)
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



    //public ObservableCollection<Leiterplattentyp> Source { get; } = new ObservableCollection<Leiterplattentyp>();


/*    public MDPartNumberViewModel(ICrudService<PcbType> crudService, IInfoBarService infoBarService)
    {
        _crudService = crudService;
        InfoBarService = infoBarService;
    }
*/
 

public async void OnNavigatedTo(object parameter)
    {
        PcbTypes.Clear();

        // TODO: Replace with real data.
        var response = await _crudService.GetAll();
        

        if (response.Code == ResponseCode.Success) {
            foreach (var item in response.Data)
            {
                PcbTypes.Add(item);
            }
        }
        else
        {
            InfoBarService.showError("ErrorMessage", "ErrorTitle");
        }

       
      
       
        
    }


    public void OnNavigatedFrom()
    {
    }
}

