using App.Contracts.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using App.Core.Services;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public class MD_CreatePartNumberViewModel : ObservableRecipient
{

    //private readonly ISampleDataService _sampleDataService;

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
        return this._canExecute;
    }


    public async void CreatePartNumber()
    {

        var lpt = await _crudService.Create(new Leiterplattentyp { LpSachnummer = "123456", MaxWeitergaben = 3 });
        InfoBarService.showMessage("Erfolgreich Leiterplatte erstellt", "Erfolg");

    }
}
