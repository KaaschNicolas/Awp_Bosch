using System.Collections.ObjectModel;
using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public partial class PcbTypeViewModel : ObservableValidator, INavigationAware
{

    [ObservableProperty]
    private PcbType _selectedItem;

    [ObservableProperty]
    private ObservableCollection<PcbType> _pcbTypes;

    private readonly ICrudService<PcbType> _crudService;
    private readonly IDialogService _dialogService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;

    public PcbTypeViewModel(ICrudService<PcbType> crudService, IInfoBarService infoBarService, IDialogService dialogService, INavigationService navigationService)
    {
        _crudService = crudService;
        _dialogService = dialogService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
        _pcbTypes = new ObservableCollection<PcbType>();

    }

    [RelayCommand]
    public async void Delete()
    {
        var result = await _dialogService.ConfirmDeleteDialogAsync("Sachnummer Löschen", "Sind Sie sicher, dass Sie diesen Eintrag löschen wollen?");
        if (result != null && result == true)
        {
            PcbType pcbToRemove = _selectedItem;
            _pcbTypes.Remove(pcbToRemove);
            await _crudService.Delete(pcbToRemove);
            _infoBarService.showMessage("Erfolgreich Leiterplatte gelöscht", "Erfolg");

        }
    }


    [RelayCommand]
    public void NavigateToUpdate(PcbType pcbType)
    {
        _navigationService.NavigateTo("App.ViewModels.UpdatePcbTypeViewModel", pcbType);

    }

    [RelayCommand]
    public void RefreshPartNumber() 
    {
        OnNavigatedTo(null);
    }


    public async void OnNavigatedTo(object parameter)
    {
        _pcbTypes.Clear();

        var response = await _crudService.GetAll();
        
        if (response.Code == ResponseCode.Success) {
            foreach (var item in response.Data)
            {
                _pcbTypes.Add(item);
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

