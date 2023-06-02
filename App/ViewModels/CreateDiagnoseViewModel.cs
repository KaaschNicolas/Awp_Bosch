using System.ComponentModel.DataAnnotations;
using App.Contracts.Services;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace App.ViewModels;
public partial class CreateDiagnoseViewModel : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [MinLength(10)]
    private string _name;

    private readonly ICrudService<Diagnose> _crudService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;

    public CreateDiagnoseViewModel(ICrudService<Diagnose> crudService, IInfoBarService infoBarService, INavigationService navigationService)
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
            var response = await _crudService.Create(new Diagnose { Name = _name });
            if (response != null)
            {
                if (response.Code == ResponseCode.Success)
                {
                    _infoBarService.showMessage("Erfolgreich Leiterplatte erstellt", "Erfolg");
                    _navigationService.NavigateTo("App.ViewModels.DiagnoseViewModel");
                }
                else
                {
                    // TODO Fehler in Dict damit man leichter Fehler ändern kann
                    _infoBarService.showError("Leiterplatte konnte nicht erstellt werden", "Error");
                }
            }
            else
            {
                _infoBarService.showError("Leiterplatte konnte nicht erstellt werden", "Error");
            }


        }

    }

    [RelayCommand]
    public void Cancel()
    {
        _navigationService.GoBack();
    }

}

