using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class StorageLocationViewModel : ObservableRecipient, INavigationAware
{


    private readonly ICrudService<StorageLocation> _crudService;
    private readonly IDialogService _dialogService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;


    [ObservableProperty]
    private StorageLocation _selectedItem;

    public ObservableCollection<StorageLocation> StorageLocations;

    public StorageLocationViewModel(ICrudService<StorageLocation> crudservice, IDialogService dialogService, IInfoBarService infoBarService, INavigationService navigationService)
    {
        _crudService = crudservice;
        _dialogService = dialogService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
        StorageLocations = new ObservableCollection<StorageLocation>();
    }


    [RelayCommand]
    public async void Delete()
    {
        var confirmDelete = await _dialogService.ConfirmDeleteDialogAsync("Lagerort löschen", "Sind Sie sicher, dass Sie den ausgewählten Lagerort löschen möchten?", "Löschen", "Abbrechen");
        if (confirmDelete != null)
        {
            StorageLocation slToRemove = SelectedItem;

            var response = await _crudService.Delete(slToRemove);
            if (response != null)
            {
                if (response.Code == ResponseCode.Success)
                {
                    StorageLocations.Remove(slToRemove);
                    _infoBarService.showMessage("Erfolgreich Lagerort gelöscht", "Erfolg");

                }
            }
            else if (response == null || response.Code == ResponseCode.Error)
            {
                _infoBarService.showError("Fehler beim Löschen des Lagerorts", "Error");
            }

        }


    }

    [RelayCommand]
    public void NavigateToCreate()
    {
        _navigationService.NavigateTo("App.ViewModels.CreateStorageLocationViewModel");
    }

    [RelayCommand]
    public void NavigateToUpdate()
    {
        _navigationService.NavigateTo("App.ViewModels.UpdateStorageLocationViewModel", SelectedItem);
    }

    [RelayCommand]
    public void Refresh()
    {
        OnNavigatedTo(new { });
    }

    public async void OnNavigatedTo(object parameter)
    {
        StorageLocations.Clear();

        var response = await _crudService.GetAll();

        if (response.Code == ResponseCode.Success)
        {
            foreach (var item in response.Data)
            {
                StorageLocations.Add(item);
            }
        }
        else
        {
            _infoBarService.showError("Lagerorte konnten nicht geladen werden", "Error");
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
