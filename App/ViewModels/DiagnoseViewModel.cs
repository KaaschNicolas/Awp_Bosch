using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services;
using App.Core.Services.Interfaces;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public class DiagnoseViewModel : ObservableObject, INavigationAware
{

    private Diagnose _selectedItem;
    public Diagnose SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (value != null)
            {
                _selectedItem = value;

            }
        }
    }




    private ObservableCollection<Diagnose> _diagnoses = new();
    public ObservableCollection<Diagnose> Diagnoses
    {
        get => _diagnoses;
        set => _diagnoses = value;
    }

    private readonly ICrudService<Diagnose> _crudService;

    private readonly INavigationService _navigationService;
    public IInfoBarService InfoBarService
    {
        get;
    }

    public ICommand DeleteDiagnoseCommand
    {
        get;
    }


    public ICommand NavigateToUpdateDiagnoseCommand
    {
        get;
    }

    public DiagnoseViewModel(ICrudService<Diagnose> crudService, IInfoBarService infoBarService, INavigationService navigationService)
    {
        _crudService = crudService;
        InfoBarService = infoBarService;
        _navigationService = navigationService;
        DeleteDiagnoseCommand = new RelayCommand(DeleteDiagnose);
        NavigateToUpdateDiagnoseCommand = new RelayCommand<Diagnose>(NavigateToUpdateDiagnose);
        Diagnoses = new ObservableCollection<Diagnose>();
    }


    private async void DeleteDiagnose()
    {

        Diagnose diagnoseToRemove = SelectedItem;
        Diagnoses.Remove(diagnoseToRemove);
        await _crudService.Delete(diagnoseToRemove);
        InfoBarService.showMessage("Erfolgreich gelöscht", "Erfolgreich");
    }

    private async void NavigateToUpdateDiagnose(Diagnose diagnose)
    {
        _navigationService.NavigateTo("App.ViewModels.UpdateDiagnoseViewModel", diagnose);
        /*var item = Diagnoses.FirstOrDefault(i => i == _selectedItem);
        if (item != null)
        {
            item = _selectedItem;
        }*/

    }

    public async void OnNavigatedTo(object parameter)
    {
        Diagnoses.Clear();

        // TODO: Replace with real data.
        var response = await _crudService.GetAll();


        if (response.Code == ResponseCode.Success)
        {
            foreach (var item in response.Data)
            {
                Diagnoses.Add(item);
            }
        }
        else
        {
            InfoBarService.showError("ErrorMessage", response.ErrorMessage);
        }
    }

    public void OnNavigatedFrom()
    {

    }
}
