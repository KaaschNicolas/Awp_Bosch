using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Errors;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels;
public partial class UpdateStorageLocationViewModel : ObservableValidator, INavigationAware
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    [MaxLength(100, ErrorMessage = ValidationErrorMessage.MaxLength100)]
    private string _storageName;

    [ObservableProperty]
    private string _dwellTimeYellow;

    [ObservableProperty]
    private string _dwellTimeRed;

    [ObservableProperty]
    public bool _isFinalDestination;

    private readonly INavigationService _navigationService;
    private readonly IInfoBarService _infoBarService;
    private readonly ICrudService<StorageLocation> _crudService;


    private StorageLocation _storageLocation;

    public UpdateStorageLocationViewModel(ICrudService<StorageLocation> crudService, IInfoBarService infoBarService, INavigationService navigationService)
    {
        _crudService = crudService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
    }


    [RelayCommand]
    public async void Save()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            _storageLocation.StorageName = _storageName;
            if (IsFinalDestination)
            {
                _dwellTimeRed = "--";
                _dwellTimeYellow = "--";
            }
            _storageLocation.DwellTimeYellow = _dwellTimeYellow;
            _storageLocation.DwellTimeRed = _dwellTimeRed;
            _storageLocation.IsFinalDestination = _isFinalDestination;
            var response = await _crudService.Update(_storageLocation.Id, _storageLocation);
            if (response != null)
            {
                if (response.Code == ResponseCode.Success)
                {
                    _infoBarService.showMessage("Update des Lagerortes war erfolgreich", "Erfolg");
                    _navigationService.NavigateTo("App.ViewModels.StorageLocationViewModel");
                }
                else
                {
                    _infoBarService.showError("Fehler beim Update des Lagerortes", "Error");
                }
            }
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        _storageLocation = (StorageLocation)parameter;
        StorageName = _storageLocation.StorageName;
        DwellTimeYellow = _storageLocation.DwellTimeYellow;
        DwellTimeRed = _storageLocation.DwellTimeRed;
        IsFinalDestination = _storageLocation.IsFinalDestination;
    }

    public void OnNavigatedFrom()
    {
    }

    public void Cancel()
    {
        _navigationService.GoBack();
    }
}