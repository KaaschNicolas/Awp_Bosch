using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Contracts.Services;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.Web.AtomPub;

namespace App.ViewModels;
public partial class CreateDiagnoseViewModel : ObservableValidator
{


    private string name;

    [Required]
    [MinLength(10)]
    public string Name
    {

        get => name;
        set {
            SetProperty(ref name, value, true);
            
        }
    }    



    private readonly ICrudService<Diagnose> _crudService;
    private readonly IInfoBarService _infoBarService;


    public CreateDiagnoseViewModel(ICrudService<Diagnose> crudService, IInfoBarService infoBarService)
    {
        _crudService = crudService;
        _infoBarService = infoBarService;

    }



    [RelayCommand]
    private async Task Save()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            var response = await _crudService.Create(new Diagnose { Name = name });
            // TODO check response -> error handling 
            if (response != null)
            {
                if (response.Code == ResponseCode.Success)
                {
                    _infoBarService.showMessage("Erfolgreich Leiterplatte erstellt", "Erfolg");
                }
                else
                {
                    // TODO Fehler in Dict damit man leichter Fehler ändern kann
                    _infoBarService.showError("Leiterplatte konnte nicht erstellt werden", "Error");
                }
            }
            else
            {
                _infoBarService.showError("Leiterplatte konnte nicht erstellt werden", "Error");
            }
            

        }

    }

}

