using System.ComponentModel.DataAnnotations;
using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public partial class UpdatePcbTypeViewModel : ObservableValidator, INavigationAware
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _pcbPartNumber;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _description;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private double _maxTransfer;

    private int _id;

    private PcbType _pcbType;

    private readonly ICrudService<PcbType> _crudService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    public UpdatePcbTypeViewModel(ICrudService<PcbType> crudService, IInfoBarService infoBarService, INavigationService navigationService)
    {
        _crudService = crudService;
        _infoBarService = infoBarService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    public async void Save()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            _pcbType.Id = _id;
            _pcbType.PcbPartNumber = _pcbPartNumber;
            _pcbType.Description = _description;
            _pcbType.MaxTransfer = (int)_maxTransfer;
            var response = await _crudService.Update(_id, _pcbType);
            if (response != null)
            {
                if (response.Code == ResponseCode.Success)
                {
                    _infoBarService.showMessage("Update der Fehlerkategorie war erfolgreich", "Erfolg");
                    _navigationService.NavigateTo("App.ViewModels.PcbTypeViewModel");


                }
                else
                {
                    _infoBarService.showError("Fehler beim Update der Fehlerkategorie", "Error");
                }

            }
            else
            {
                _infoBarService.showError("Fehler beim Update der Fehlerkategorie", "Error");
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
        _pcbType = (PcbType)parameter;
        _id = _pcbType.Id;
        _pcbPartNumber = _pcbType.PcbPartNumber;
        _description = _pcbType.Description;
        _maxTransfer = (double)_pcbType.MaxTransfer;
    }
}
