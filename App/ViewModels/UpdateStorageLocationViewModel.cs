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
public class UpdateStorageLocationViewModel: ObservableRecipient, INavigationAware
{
    private string _storageName = "";
    public string StorageName
    {
        get => _storageName;
        set
        {
            if (_storageName != value)
            {
                _storageName = value;
                OnPropertyChanged(nameof(StorageName));

            }

        }
    }

    private string _dwellTimeYellow = "0";
    public string DwellTimeYellow
    {
        get => _dwellTimeYellow;
        set
        {
            if (_dwellTimeYellow != value)
            {
                _dwellTimeYellow = value;
                OnPropertyChanged(nameof(DwellTimeYellow));

            }

        }
    }

    private string _dwellTimeRed = "";
    public string DwellTimeRed
    {
        get => _dwellTimeRed;
        set
        {
            if (_dwellTimeRed != value)
            {
                _dwellTimeRed = value;
                OnPropertyChanged(nameof(DwellTimeRed));

            }

        }
    }

    public bool _isFinalDestination;
    public bool IsFinalDestination
    {
        get => _isFinalDestination;
        set
        {
            _isFinalDestination = value;
            OnPropertyChanged(nameof(IsFinalDestination));
        }
    }

    private int _id = 0;
    public int Id => _id;

    private readonly INavigationService _navigationService;

    private readonly IInfoBarService InfoBarService;

    private readonly ICrudService<StorageLocation> _crudService;

    public ICommand SaveCommand
    {
        get;
    }

    private StorageLocation _storageLocation;



    public UpdateStorageLocationViewModel(ICrudService<StorageLocation> crudService, IInfoBarService infoBarService, INavigationService navigationService)
    {
        _crudService = crudService;
        InfoBarService = infoBarService;
        SaveCommand = new RelayCommand(Save);
        _navigationService = navigationService;
    }


    public async void Save()
    {
        _storageLocation.Id = _id;
        _storageLocation.StorageName = _storageName;
        _storageLocation.DwellTimeYellow = _dwellTimeYellow;
        _storageLocation.DwellTimeRed = _dwellTimeRed;
        _storageLocation.IsFinalDestination = _isFinalDestination;
        var response = await _crudService.Update(_id, _storageLocation);
        if (response != null)
        {
            if (response.Code == ResponseCode.Success)
            {
                InfoBarService.showMessage("Update des Lagerortes war erfolgreich", "Erfolg");
                _navigationService.NavigateTo("App.ViewModels.StorageLocationViewModel");
            }
            else
            {
                InfoBarService.showError("Fehler beim Update des Lagerortes", "Error");
            }
        }

    }

    public void OnNavigatedTo(object parameter)
    {
        //var param = await _crudService.GetById(_storageLocation.Id);
        
        _storageLocation = (StorageLocation)parameter;
        _id = _storageLocation.Id;
        _storageName = _storageLocation.StorageName;
        _dwellTimeYellow = _storageLocation.DwellTimeYellow;
        _dwellTimeRed = _storageLocation.DwellTimeRed;
        _isFinalDestination = _storageLocation.IsFinalDestination;

    }

    public void OnNavigatedFrom()
    {
    }
}