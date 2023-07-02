using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using App.Errors;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels;

public partial class UpdateUserViewModel : ObservableValidator, INavigationAware
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

    public IEnumerable<Role> Roles {
        get {
            return Enum.GetValues(typeof(Role)).Cast<Role>();
            } 
    }

    private int _id;

    private User _user;

    private readonly ICrudService<User> _crudService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    public UpdateUserViewModel(ICrudService<User> crudService, IInfoBarService infoBarService, INavigationService navigationService)
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
            _user.Id = _id;
            _user.Name = _name;
            _user.AdUsername = _adusername;
            _user.Role = _role;

            var response = await _crudService.Update(_id, _user);
            if (response != null)
            {
                if (response.Code == ResponseCode.Success)
                {
                    _infoBarService.showMessage("Benutzer erfolgreich bearbeitet", "Erfolg");
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

    public void OnNavigatedFrom() { }
    public void OnNavigatedTo(object parameter)
    {
        _user = (User)parameter;
        _id = _user.Id;
        _name = _user.Name;
        _adusername = _user.AdUsername;
        Role = _user.Role;
    }
}
