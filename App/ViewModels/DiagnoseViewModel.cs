using System.Collections.ObjectModel;
using System.Windows.Input;
using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public partial class DiagnoseViewModel : ObservableObject, INavigationAware
{
    [ObservableProperty]
    private Diagnose _selectedItem;

    [ObservableProperty]
    private ObservableCollection<Diagnose> _diagnoses = new();

    private readonly ICrudService<Diagnose> _crudService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    public IInfoBarService _infoBarService;


    public DiagnoseViewModel(ICrudService<Diagnose> crudService, IInfoBarService infoBarService, INavigationService navigationService, IDialogService dialogService)
    {
        _crudService = crudService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
        _dialogService = dialogService;

    }
    [RelayCommand]
    public async void Delete()
    {
        var result = await _dialogService.ConfirmDeleteDialogAsync("Fehlerkategorie löschen", "Sind sie sicher, dass sie diesen Eintrag löschen wollen?");
        if (result != null && result == true)
        {
            Diagnose diagnoseToRemove = SelectedItem;
            Diagnoses.Remove(diagnoseToRemove);
            await _crudService.Delete(diagnoseToRemove);
            _infoBarService.showMessage("Erfolgreich gelöscht", "Erfolgreich");
        }

    }

    [RelayCommand]
    public void NavigateToUpdate(Diagnose diagnose)
    {
        _navigationService.NavigateTo("App.ViewModels.UpdateDiagnoseViewModel", diagnose);

    }

    [RelayCommand]
    public void RefreshDiagnose()
    {
        OnNavigatedTo(null);
    }

    public async void OnNavigatedTo(object parameter)
    {
        _diagnoses.Clear();

        var response = await _crudService.GetAll();


        if (response.Code == ResponseCode.Success)
        {
            foreach (var item in response.Data)
            {
                _diagnoses.Add(item);
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
