using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class PcbViewModel : ObservableRecipient, INavigationAware
{

    private readonly ICrudService<Pcb> _crudService;
    private readonly IInfoBarService _infoBarService;


    [ObservableProperty]
    private ObservableCollection<Pcb> _pcbs = new();
    public PcbViewModel(ICrudService<Pcb> crudService, IInfoBarService infoBarService)
    {
        _crudService = crudService;
        _infoBarService = infoBarService;
    }



    private async Task LoadDataAsync()
    {
        _pcbs.Clear();
        var response = await _crudService.GetAll();
        if (response != null && response.Code == ResponseCode.Success)
        {
            foreach (var item in response.Data)
            {
                _pcbs.Add(item);
            }
        }
        else
        {
            _infoBarService.showError("Fehler beim Laden der Leiterplatten-Daten", "Error");
        }
    }
    public void OnNavigatedFrom()
    {

    }

    public async void OnNavigatedTo(object parameter)
    {
        await LoadDataAsync();
    }




}
