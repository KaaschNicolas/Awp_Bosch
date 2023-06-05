using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class TransferDialogViewModel : ObservableObject
{
    [ObservableProperty]
    private Visibility _isVisible = Visibility.Collapsed;

    [ObservableProperty]
    private User _notedBy;

    [ObservableProperty]
    private DateTime _transferDate = DateTime.Now;

    private StorageLocation _selectedStorageLocation;
    public StorageLocation SelectedStorageLocation
    {
        get => _selectedStorageLocation;
        set
        {
            if (value.IsFinalDestination == true)
            {
                IsVisible = Visibility.Visible;
            }
            else
            {
                IsVisible = Visibility.Collapsed;
            }
            SetProperty(ref _selectedStorageLocation, value);
        }
    }

    [ObservableProperty]
    private Diagnose _selectedDiagnose;

    [ObservableProperty]
    private string _comment;

    [ObservableProperty]
    private ObservableCollection<StorageLocation> _storageLocations = new();

    [ObservableProperty]
    private ObservableCollection<Diagnose> _diagnoses = new();

    private readonly IAuthenticationService _authenticationService;
    private readonly ICrudService<StorageLocation> _storageLocationCrudService;
    private readonly ICrudService<Diagnose> _diagnoseCrudService;
    private readonly ITransferDataService<Transfer> _transferDataService;
    public TransferDialogViewModel(IAuthenticationService authenticationService, ICrudService<StorageLocation> storageLocationCrudService, ICrudService<Diagnose> diagnoseCrudService)
    {
        _authenticationService = authenticationService;
        _diagnoseCrudService = diagnoseCrudService;
        _storageLocationCrudService = storageLocationCrudService;
        _notedBy = _authenticationService.CurrentUser;
        LoadData();
    }

    private async void LoadData()
    {

        //TODO: Error handling
        var resStorageLocations = await _storageLocationCrudService.GetAll();
        if (resStorageLocations.Code == ResponseCode.Success)
        {
            resStorageLocations.Data.ForEach(x => _storageLocations.Add(x));
        }

        var resDiagnoses = await _diagnoseCrudService.GetAll();
        if (resDiagnoses.Code == ResponseCode.Success)
        {
            resDiagnoses.Data.ForEach(x => _diagnoses.Add(x));
        }
    }

    private async void Save()
    {
        await _transferDataService.CreateTransfer();
    }
}

