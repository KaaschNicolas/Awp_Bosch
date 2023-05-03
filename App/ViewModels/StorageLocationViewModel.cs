using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WinRT;

namespace App.ViewModels;

public class StorageLocationViewModel : ObservableRecipient, INotifyPropertyChanged,INavigationAware
{
    private int _id;
    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged(nameof(Id));
        } 
    }
    
    private string _storageName;

    public string StorageName
    {
        get => _storageName;
        set
        {
            _storageName = value;
            OnPropertyChanged(nameof(StorageName));
        }
    }

    private int _dwellTimeYellow;

    public int DwellTimeYellow
    {
        get => _dwellTimeYellow;
        set
        {
            _dwellTimeYellow = value;
            OnPropertyChanged(nameof(DwellTimeYellow));
        }
    }

    private int _dwellTimeRed;
    
    public int DwellTimeRed
    {
        get => _dwellTimeRed;
        set
        {
            _dwellTimeRed = value;
            OnPropertyChanged(nameof(DwellTimeRed));
        }
    }

    private readonly ICrudService<StorageLocation> _crudService;

    private readonly IDialogService _dialogService;

    private readonly IInfoBarService _infoBarService;

    private readonly INavigationService _navigationService;


    public bool _canExecute = true;

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

    private StorageLocation _selectedItem;
    public StorageLocation SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged(nameof(SelectedItem));
        }
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

    private ObservableCollection<StorageLocation> _storageLocations;
    public ObservableCollection<StorageLocation> StorageLocations
    {
        get => _storageLocations;
        set
        {
            _storageLocations = value;
            OnPropertyChanged(nameof(StorageLocations));
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
        StorageLocations = new ObservableCollection<StorageLocation>();
    }

    private StorageLocation _storageLocation;
    
    
    
    public async void CreateStorageLocation()
    {
        var sl = await _crudService.Create(new StorageLocation { StorageName = _storageName, DwellTimeYellow = _dwellTimeYellow, DwellTimeRed = _dwellTimeRed });
        _infoBarService.showMessage("Erfolgreich Lagerort erstellt", "Erfolg");
        _navigationService.NavigateTo("App.ViewModels.StorageLocationViewModel");
    }


    public async void DeleteStorageLocation()
    {
        var confirmDelete = await _dialogService.ConfirmDeleteDialogAsync("Lagerort löschen", "Sind Sie sicher, dass Sie den ausgewählten Lagerort löschen möchten?", "Löschen", "Abbrechen");
        if(confirmDelete != null)
        {
            StorageLocation slToRemove = SelectedItem;
            StorageLocations.Remove(slToRemove);
            await _crudService.Delete(slToRemove);
            _infoBarService.showMessage("Erfolgreich Lagerort gelöscht", "Erfolg");
        }

        
    }

    private void NavigateToUpdateStorageLocation(StorageLocation storageLocation)
    {
        _navigationService.NavigateTo("App.ViewModels.UpdateStorageLocationViewModel", storageLocation);
    }

    public async void OnNavigatedTo(object parameter)
    {
        StorageLocations.Clear();

        var response = await _crudService.GetAll();

        if (response.Code == ResponseCode.Success) { 
            foreach (var item in response.Data)
            {
                if (item.DeletedDate < new DateTime(2000,01,01))
                {
                    StorageLocations.Add(item);
                }
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
