using System.Collections.ObjectModel;
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

public class DiagnoseViewModel : ObservableRecipient, INavigationAware
{
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    private Diagnose _selectedItem;
    public Diagnose SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged(nameof(SelectedItem));
        }
    }




    private ObservableCollection<Diagnose> _diagnoses = new();
    public ObservableCollection<Diagnose> Diagnoses
    {
        get => _diagnoses;
        set {
            _diagnoses = value;
            OnPropertyChanged(nameof(Diagnose));
        }
    }

    private readonly ICrudService<Diagnose> _crudService;


    public IInfoBarService InfoBarService
    {
        get;
    }

    public ICommand DeleteDiagnoseCommand
    {
        get;
    }


    public ICommand CreateDiagnoseCommand
    {
        get; 
    }

    public DiagnoseViewModel(ICrudService<Diagnose> crudService, IInfoBarService infoBarService, IDialogService dialogService)
    {
        _crudService = crudService;
        InfoBarService = infoBarService;
        CreateDiagnoseCommand = new RelayCommand(CreateDiagnose);
        DeleteDiagnoseCommand = new RelayCommand(DeleteDiagnose);
        Diagnoses = new ObservableCollection<Diagnose>();

    }


    private async void CreateDiagnose()
    {

        var response = await _crudService.Create(new Diagnose { Name = _name });
        // TODO check response -> error handling 
        InfoBarService.showMessage("Erfolgreich Leiterplatte erstellt", "Erfolg");

    }
    private async void DeleteDiagnose()
    {
        Diagnose diagnoseToRemove = SelectedItem;
        Diagnoses.Remove(diagnoseToRemove);
        await _crudService.Delete(diagnoseToRemove);
        InfoBarService.showMessage("Erfolgreich gelöscht", "Erfolgreich");

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
