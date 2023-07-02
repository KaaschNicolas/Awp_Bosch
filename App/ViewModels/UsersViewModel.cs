using System.Collections.ObjectModel;
using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Contracts.Services;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public partial class UsersViewModel : ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private User _selectedItem;

    [ObservableProperty]
    public ObservableCollection<User> _users = new ();

    private readonly ICrudService<User> _crudService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    public IInfoBarService _infoBarService;
    public UsersViewModel(ICrudService<User> crudService, IInfoBarService infoBarService, INavigationService navigationService, IDialogService dialogService)
    {
        _crudService = crudService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
        _dialogService = dialogService;
    }
    [RelayCommand]
    public async void Delete()
    {
        var result = await _dialogService.ConfirmDeleteDialogAsync("Benutzer löschen", "Sind sie sicher, dass sie diesen Benutzer löschen wollen?");
        if (result != null && result == true)
        {
            User userToRemove = SelectedItem;
            Users.Remove(userToRemove);
            await _crudService.Delete(userToRemove);
            _infoBarService.showMessage("Erfolgreich gelöscht", "Erfolgreich");
        }

    }

    [RelayCommand]
    public void NavigateToUpdate(User user)
    {
        _navigationService.NavigateTo("App.ViewModels.UpdateUserViewModel", user);

    }

    [RelayCommand]
    public void RefreshUser()
    {
        OnNavigatedTo(null);
    }

    public async void OnNavigatedTo(object parameter)
    {
        _users.Clear();

        var response = await _crudService.GetAll();


        if (response.Code == ResponseCode.Success)
        {
            foreach (var item in response.Data)
            {
                _users.Add(item);
            }
        }
        else
        {
            _infoBarService.showError("Daten konnten nicht geladen werden", "Error");
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
