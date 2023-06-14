using App.Contracts.Services;
using App.ViewModels;
using App.Views;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

namespace App.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public PageService()
    {
        Configure<MainViewModel, MainPage>();
        Configure<BlankViewModel, BlankPage>();
        Configure<SettingsViewModel, SettingsPage>();
        Configure<StorageLocationViewModel, StorageLocationPage>();
        Configure<CreateStorageLocationViewModel, CreateStorageLocationPage>();
        Configure<UpdateStorageLocationViewModel, UpdateStorageLocationPage>();
        Configure<PcbTypeViewModel, PcbTypePage>();
        Configure<DiagnoseViewModel, DiagnosePage>();
        Configure<UpdateDiagnoseViewModel, UpdateDiagnosePage>();
        Configure<CreateDiagnoseViewModel, CreateDiagnosePage>();
        Configure<StorageLocationPaginationViewModel, StorageLocationsViewPage1>();
        Configure<CreatePcbTypeViewModel, CreatePcbTypePage>();
        Configure<UpdatePcbTypeViewModel, UpdatePcbTypePage>();
        Configure<CreatePcbViewModel, CreatePcbPage>();
        Configure<PcbSingleViewModel, PcbSinglePage>();
        Configure<TransfersViewModel, TransfersPage>();
        Configure<PcbPaginationViewModel, PcbViewPage>();
        Configure<UpdatePcbViewModel, UpdatePcbPage>();
        Configure<DwellTimeEvaluationViewModel, DwellTimeEvalutionPage>();
        Configure<PcbTypeEvaluationViewModel, PcbTypeEvaluationPage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
