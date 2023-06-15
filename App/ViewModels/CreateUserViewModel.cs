using App.Contracts.Services;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using App.Errors;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;


namespace App.ViewModels;
public partial class CreateUserViewModel : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    [MaxLength(100, ErrorMessage = ValidationErrorMessage.MaxLength100)]
    private string _name;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    [MaxLength(100, ErrorMessage = ValidationErrorMessage.MaxLength100)]
    private string _adusername;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    private Role _role;

    public IEnumerable<Role> Roles
    {
        get
        {
            return Enum.GetValues(typeof(Role)).Cast<Role>();
        }
    }

    private readonly ICrudService<User> _crudService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;

    public CreateUserViewModel(ICrudService<User> crudService, IInfoBarService infoBarService, INavigationService navigationService)
    {
        _crudService = crudService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;

        _role = Role.Lesezugriff;
    }

    [RelayCommand]
    public async Task Save()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            var response = await _crudService.Create(new User { Name = _name, AdUsername = _adusername , Role= _role});
            if (response != null)
            {
                if (response.Code == ResponseCode.Success)
                {
                    _infoBarService.showMessage("Erfolgreich Leiterplatte erstellt", "Erfolg");
                    _navigationService.NavigateTo("App.ViewModels.UsersViewModel");
                }
                else
                {
                    // TODO Fehler in Dict damit man leichter Fehler ändern kann
                    _infoBarService.showError("Benutzer konnte nicht erstellt werden", "Error");
                }
            }
            else
            {
                _infoBarService.showError("Benutzer konnte nicht erstellt werden", "Error");
            }
        }
    }

    [RelayCommand]
    public void Cancel()
    {
        _navigationService.GoBack();
    }
}
