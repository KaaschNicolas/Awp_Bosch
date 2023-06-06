using App.Contracts.Services;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Errors;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels;

public partial class CreateStorageLocationViewModel : ObservableValidator
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
    private bool _isFinalDestination;


    private readonly ICrudService<StorageLocation> _crudService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;

    public CreateStorageLocationViewModel(ICrudService<StorageLocation> crudService, IInfoBarService infoBarService, INavigationService navigationService)
    {
        _crudService = crudService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
    }


    public async Task Save()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            var dwellTimeRedVal = DwellTimeRed;
            var dwellTimeYellowVal = DwellTimeYellow;

            if (IsFinalDestination)
            {
                dwellTimeRedVal = "--";
                dwellTimeRedVal = "--";
            }
            var response = await _crudService.Create(new StorageLocation { StorageName = StorageName, DwellTimeYellow = dwellTimeYellowVal, DwellTimeRed = dwellTimeRedVal, IsFinalDestination = IsFinalDestination });
            if (response != null)
            {
                if (response.Code == ResponseCode.Success)
                {
                    _infoBarService.showMessage("Lagerort erfolgreich erstellt", "Erfolg");
                    _navigationService.NavigateTo("App.ViewModels.StorageLocationViewModel");
                }
                else
                {
                    _infoBarService.showError("Lagerort konnte nicht erstellt werden", "Error");
                }

            }
            else
            {
                _infoBarService.showError("Lagerort konnte nicht erstellt werden", "Error");
            }
        }

    }

    [RelayCommand]
    public void Cancel()
    {
        _navigationService.GoBack();
    }

}
