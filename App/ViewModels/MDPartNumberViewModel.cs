using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Contracts.Services;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Models;
using App.Services;
using App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using WinUIEx;

namespace App.ViewModels;

public class MDPartNumberViewModel : ObservableRecipient, INavigationAware
{

    private readonly ISampleDataService _sampleDataService;

    private ICrudService _crudService;

    public IInfoBarService InfoBarService
    {
        get;
    }

    public ObservableCollection<PartNumber> Source { get; } = new ObservableCollection<Leiterplattentyp>();

    public ObservableCollection<Leiterplattentyp> Source
    { get; } = new ObservableCollection<PartNumber>()
    public MDPartNumberViewModel(ISampleDataService sampleDataService, IInfoBarService infoBarService)
    {
        _sampleDataService = sampleDataService;
        InfoBarService = infoBarService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
        InfoBarService.showError("ErrorMessage","ErrorTitle");
        
    }

    public async Task<Leiterplattentyp> CreatePartNumber(PartNumber partNumber)
    {

        try
        {
            return await _crudService.Create(new Leiterplattentyp { LpSachnummer = "123456", MaxWeitergaben = 3 });
           
        }
        catch (Exception)
        {

            InfoBarService.showError("Error bei erstellen von Leiterplatte", "Error");

        }       

    }

    public void OnNavigatedFrom()
    {
    }
}

