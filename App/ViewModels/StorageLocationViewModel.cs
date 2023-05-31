using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Controls;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace App.ViewModels;

public partial class StorageLocationViewModel : ObservableValidator, INotifyPropertyChanged, INavigationAware
{
    [ObservableProperty]
    private int _id;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    private string _storageName;

    [ObservableProperty]
    private string _dwellTimeYellow;

    [ObservableProperty]
    private string _dwellTimeRed;

    [ObservableProperty]
    public bool _isFinalDestination;

    [ObservableProperty]
    private StorageLocation _selectedItem;

    [ObservableProperty]
    private ObservableCollection<StorageLocation> _storageLocations;


    private readonly ICrudService<StorageLocation> _crudService;
    private readonly IDialogService _dialogService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;


    public bool _canExecute = true;


    //TODO: Refactor Commands using RelayCommand data annotation
    public ICommand CreateSL
    {
        get;
    }

    public ICommand UpdateSL
    {
        get;
    }

    public ICommand DeleteSL
    {
        get;
    }

    public ICommand NavigateToUpdateStorageLocationCommand
    {
        get;
    }

    public ICommand ConfirmationDeleteCommand
    {
        get;
    }

    public ICommand RefreshStorageLocationCommand
    {
        get;
    }




    public bool CanExecute
    {
        get => _canExecute;
        set
        {
            if (_canExecute == value)
            {
                return;
            }
            _canExecute = value;
        }
    }



    public StorageLocationViewModel(ICrudService<StorageLocation> crudservice, IDialogService dialogService, IInfoBarService infoBarService, INavigationService navigationService)
    {
        _crudService = crudservice;
        _dialogService = dialogService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
        NavigateToUpdateStorageLocationCommand = new RelayCommand<StorageLocation>(NavigateToUpdateStorageLocation);
        CreateSL = new RelayCommand(CreateStorageLocation);
        DeleteSL = new RelayCommand(DeleteStorageLocation);
        RefreshStorageLocationCommand = new RelayCommand(RefreshStorageLocation);
        StorageLocations = new ObservableCollection<StorageLocation>();
    }

    private StorageLocation _storageLocation;



    public async void CreateStorageLocation()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            if (IsFinalDestination)
            {
                _dwellTimeRed = "--";
                _dwellTimeYellow = "--";
            }
            //TODO: check sl if create was successfull
            var sl = await _crudService.Create(new StorageLocation { StorageName = _storageName, DwellTimeYellow = _dwellTimeYellow, DwellTimeRed = _dwellTimeRed, IsFinalDestination = _isFinalDestination });
            _infoBarService.showMessage("Erfolgreich Lagerort erstellt", "Erfolg");
            _navigationService.NavigateTo("App.ViewModels.StorageLocationViewModel");
        }
    }

    public async void DeleteStorageLocation()
    {
        var confirmDelete = await _dialogService.ConfirmDeleteDialogAsync("Lagerort löschen", "Sind Sie sicher, dass Sie den ausgewählten Lagerort löschen möchten?", "Löschen", "Abbrechen");
        if (confirmDelete != null)
        {
            StorageLocation slToRemove = SelectedItem;
            StorageLocations.Remove(slToRemove);
            //TODO: check sl if delete was successfull
            await _crudService.Delete(slToRemove);
            _infoBarService.showMessage("Erfolgreich Lagerort gelöscht", "Erfolg");
        }


    }

    private void NavigateToUpdateStorageLocation(StorageLocation storageLocation)
    {
        _navigationService.NavigateTo("App.ViewModels.UpdateStorageLocationViewModel", storageLocation);
    }

    public async void RefreshStorageLocation()
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
            _infoBarService.showError("ErrorMessage", "ErrorTitle");
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
