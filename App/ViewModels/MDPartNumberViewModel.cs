using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using App.Contracts.Services;
using App.Contracts.ViewModels;
using App.Core.Contracts.Services;
using App.Core.Models;
using App.Core.Services;
using App.Core.Services.Interfaces;
using App.Models;
using App.Services;
using App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using WinUIEx;

namespace App.ViewModels;

// public class MDPartNumberViewModel : ObservableRecipient, INavigationAware

public class MDPartNumberViewModel : ObservableRecipient
{



    private ICrudService _crudService;


    public IInfoBarService InfoBarService
    {
        get;
    }

    public ObservableCollection<PcbType> Source { get; } = new ObservableCollection<PcbType>();
    //public ObservableCollection<Leiterplattentyp> Source { get; } = new ObservableCollection<Leiterplattentyp>();


    public MDPartNumberViewModel(ICrudService crudService, IInfoBarService infoBarService)
    {
        _crudService = crudService;
        InfoBarService = infoBarService;
    }

 

/*    public async void OnNavigatedTo(object parameter)
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
*/

    public void OnNavigatedFrom()
    {
    }
}

