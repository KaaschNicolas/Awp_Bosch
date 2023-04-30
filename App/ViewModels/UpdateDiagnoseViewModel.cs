using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services;
using App.Core.Services.Interfaces;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace App.ViewModels;
public class UpdateDiagnoseViewModel: ObservableRecipient, INavigationAware
{
    private string _name = "";
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));

            }

        }
    }

    private int _id = 0;
    public int Id => _id;

    public IInfoBarService InfoBarService
    {
        get;
    }

    private readonly ICrudService<Diagnose> _crudService;

    public ICommand SaveCommand
    {
        get;
    }

    private Diagnose _diagnose;



    public UpdateDiagnoseViewModel(ICrudService<Diagnose> crudService, IInfoBarService infoBarService)
    {
        _crudService = crudService;
        InfoBarService = infoBarService;
        SaveCommand = new RelayCommand(Save);   
    }


    public async void Save()
    {
        _diagnose.Id = _id;
        _diagnose.Name = _name;
        var response = await _crudService.Update(_id, _diagnose);
        if (response != null)
        {
            if (response.Code == ResponseCode.Success)
            {
                InfoBarService.showMessage("Update der Fehlerkategorie war erfolgreich", "Erfolg");
            }
            else
            {
                InfoBarService.showError("Fehler beim Update der Fehlerkategorie", "Error");
            }
        }

    }

    public async void OnNavigatedTo(object parameter)
    {
        _diagnose = (Diagnose)parameter;
        _id = _diagnose.Id;
        _name = _diagnose.Name;
        

    }

    public void OnNavigatedFrom()
    {
    }
}
