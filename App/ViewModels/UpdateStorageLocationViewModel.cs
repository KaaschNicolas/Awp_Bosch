using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Controls;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels;
public partial class UpdateStorageLocationViewModel : ObservableValidator, INavigationAware
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    private string _storageName = "";

    [ObservableProperty]
    private string _dwellTimeYellow = "0";

    [ObservableProperty]
    private string _dwellTimeRed = "";

    [ObservableProperty]
    public bool _isFinalDestination;


    private int _id = 0;
    public int Id => _id;

    private readonly INavigationService _navigationService;

    private readonly IInfoBarService InfoBarService;

    private readonly ICrudService<StorageLocation> _crudService;



    private StorageLocation _storageLocation;



    public UpdateStorageLocationViewModel(ICrudService<StorageLocation> crudService, IInfoBarService infoBarService, INavigationService navigationService)
    {
        _crudService = crudService;
        InfoBarService = infoBarService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    public async void Save()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            _storageLocation.Id = _id;
            _storageLocation.StorageName = _storageName;
            if (IsFinalDestination)
            {
                _dwellTimeRed = "--";
                _dwellTimeYellow = "--";
            }
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