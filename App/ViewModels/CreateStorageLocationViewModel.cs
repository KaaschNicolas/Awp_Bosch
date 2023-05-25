using System.ComponentModel.DataAnnotations;
using App.Contracts.Services;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public partial class CreateStorageLocationViewModel : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _storageName;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private int _dwellTimeYellow;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private int _dwellTimeRed;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private bool _finalDisposition;

    private readonly ICrudService<StorageLocation> _crudService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;

    public CreateStorageLocationViewModel( ICrudService<StorageLocation> crudService, IInfoBarService infoBarService, INavigationService navigationService)
    {
        _crudService = crudService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
    }


    [RelayCommand]
    public async Task Save()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            var response = await _crudService.Create(new StorageLocation { StorageName = _storageName, DwellTimeYellow = _dwellTimeYellow, DwellTimeRed = _dwellTimeRed });
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
