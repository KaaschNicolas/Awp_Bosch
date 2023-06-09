﻿using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using App.Errors;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels;
public partial class UpdateDiagnoseViewModel : ObservableValidator, INavigationAware
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = ValidationErrorMessage.Required)]
    [MaxLength(100, ErrorMessage = ValidationErrorMessage.MaxLength100)]
    private string _name;

    private int _id = 0;
    public int Id => _id;

    private readonly IInfoBarService _infoBarService;
    private readonly ICrudService<Diagnose> _crudService;
    private readonly INavigationService _navigationService;

    private Diagnose _diagnose;



    public UpdateDiagnoseViewModel(ICrudService<Diagnose> crudService, IInfoBarService infoBarService, INavigationService navigationService)
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
            _diagnose.Id = _id;
            _diagnose.Name = _name;
            var response = await _crudService.Update(_id, _diagnose);
            if (response != null)
            {
                if (response.Code == ResponseCode.Success)
                {
                    _infoBarService.showMessage("Update der Fehlerkategorie war erfolgreich", "Erfolg");
                    _navigationService.NavigateTo("App.ViewModels.DiagnoseViewModel");

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

    public async void OnNavigatedTo(object parameter)
    {
        _diagnose = (Diagnose)parameter;
        _id = _diagnose.Id;
        _name = _diagnose.Name;
    }

    public void OnNavigatedFrom()
    {
    }

    public void Cancel()
    {
        _navigationService.GoBack();
    }
}
