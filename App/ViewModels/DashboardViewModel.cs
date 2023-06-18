using App.Core.DTOs;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels;

public sealed partial class DashboardViewModel : ObservableRecipient
{
    public DashboardViewModel(IDashboardDataService<BaseEntity> dashboardDataService)
    {
        _dashboardDataService = dashboardDataService;
        LoadDashboardCommand = new AsyncRelayCommand(LoadDashboard);
    }

    private IDashboardDataService<BaseEntity> _dashboardDataService;

    [ObservableProperty]
    private DashboardPcbTypeDTO _pcbType1;

    [ObservableProperty]
    private DashboardPcbTypeDTO _pcbType2;

    [ObservableProperty]
    private DashboardPcbTypeDTO _pcbType3;

    [ObservableProperty]
    private DashboardStorageLocationDTO _storageLocation1;

    [ObservableProperty]
    private DashboardStorageLocationDTO _storageLocation2;

    [ObservableProperty]
    private DashboardStorageLocationDTO _storageLocation3;

    [ObservableProperty]
    private int _pcbsAddedToday;

    [ObservableProperty]
    private int _finalizedPcbsToday;

    [ObservableProperty]
    private int _pcbsInCirculation;

    [ObservableProperty]
    private int _pcbCountLast7Days;

    public IAsyncRelayCommand LoadDashboardCommand { get; set; }
    public async Task LoadDashboard()
    {
        var pcbsAddedToday = await _dashboardDataService.GetPcbsCreatedToday();

        if (pcbsAddedToday.Code == ResponseCode.Success)
        {
            PcbsAddedToday = pcbsAddedToday.Data;
        }

        var pcbCountLast7Days = await _dashboardDataService.GetPcbCountLast7Days();

        if (pcbCountLast7Days.Code == ResponseCode.Success)
        {
            PcbCountLast7Days = pcbCountLast7Days.Data;
        }

        var top3PcbType = await _dashboardDataService.GetTop3PcbTypes();

        if (top3PcbType.Code == ResponseCode.Success)
        {
            if (top3PcbType.Data.Count >= 1)
            {
                PcbType1 = top3PcbType.Data[0];
            }
            if (top3PcbType.Data.Count >= 2)
            {
                PcbType2 = top3PcbType.Data[1];
            }
            if (top3PcbType.Data.Count  == 3) 
            { 
                PcbType3 = top3PcbType.Data[2];
            }
        }

        var top3StorageLocations = await _dashboardDataService.GetTop3StorageLocations();

        if (top3StorageLocations.Code == ResponseCode.Success)
        {
            StorageLocation1 = top3StorageLocations.Data[0];
            StorageLocation2 = top3StorageLocations.Data[1];
            StorageLocation3 = top3StorageLocations.Data[2];
        }

        var  pcbInCirculation = await _dashboardDataService.GetPcbsInCirculation();

        if (pcbInCirculation.Code == ResponseCode.Success)
        {
            PcbsInCirculation = pcbInCirculation.Data;
        }
    }
}
