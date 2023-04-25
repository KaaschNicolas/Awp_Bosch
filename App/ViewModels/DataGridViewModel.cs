using System.Collections.ObjectModel;

using App.Contracts.ViewModels;
using App.Core.Contracts.Services;
using App.Core.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace App.ViewModels;

public class DataGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<PartNumber> Source { get; } = new ObservableCollection<PartNumber>();

    public DataGridViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
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
    }

    public void OnNavigatedFrom()
    {
    }
}
