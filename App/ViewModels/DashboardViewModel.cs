using App.Core.DTOs;
using App.Core.Models;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Windows.Controls;

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
    private int _pcbsAddedToday;

    [ObservableProperty]
    private int _finalizedPcbsToday;

    [ObservableProperty]
    private int _pcbsInCirculation;

    [ObservableProperty]
    private int _pcbCountLast7Days;

    [ObservableProperty]
    private ObservableCollection<DashboardPcbTypeDTO> _pcbTypeDTOs = new();
    
    [ObservableProperty]
    private ObservableCollection<DashboardStorageLocationDTO> _storageLocationDTOs = new();

    [ObservableProperty]
    private ObservableCollection<DashboardDwellTimeDTO> _dwellTimeDTOs = new();

    public IAsyncRelayCommand LoadDashboardCommand { get; set; }
    public async Task LoadDashboard()
    {
        var pcbsAddedToday = await _dashboardDataService.GetPcbsCreatedToday();

        if (pcbsAddedToday.Code == ResponseCode.Success)
        {
            PcbsAddedToday = pcbsAddedToday.Data;
        }

        var  pcbInCirculation = await _dashboardDataService.GetPcbsInCirculation();

        if (pcbInCirculation.Code == ResponseCode.Success)
        {
            PcbsInCirculation = pcbInCirculation.Data;
        }

        var pcbCountLast7Days = await _dashboardDataService.GetPcbCountLast7Days();

        if (pcbCountLast7Days.Code == ResponseCode.Success)
        {
            PcbCountLast7Days = pcbCountLast7Days.Data;
        }

        var top3PcbType = await _dashboardDataService.GetTop3PcbTypes();

        if (top3PcbType.Code == ResponseCode.Success)
        {
            top3PcbType.Data.ForEach(x => PcbTypeDTOs.Add(x));

            if (PcbTypeDTOs.Count >= 1)
            {
                PcbTypeDTOs[0].Number = "1.";
                PcbTypeDTOs[0].Percentage = (PcbTypeDTOs[0].Count * 100 / PcbCountLast7Days);
            }

            if (PcbTypeDTOs.Count >= 2)
            {
                PcbTypeDTOs[1].Number = "2.";
                PcbTypeDTOs[1].Percentage = (PcbTypeDTOs[1].Count * 100 / PcbCountLast7Days);
            }

            if (PcbTypeDTOs.Count == 3)
            {
                PcbTypeDTOs[2].Number = "3.";
                PcbTypeDTOs[2].Percentage = (PcbTypeDTOs[2].Count * 100 / PcbCountLast7Days);
            }
        }

        var top3StorageLocations = await _dashboardDataService.GetTop3StorageLocations();

        if (top3StorageLocations.Code == ResponseCode.Success)
        {
            top3StorageLocations.Data.ForEach(x => StorageLocationDTOs.Add(x));

            if (StorageLocationDTOs.Count >= 1)
            {
                StorageLocationDTOs[0].Number = "1.";
                StorageLocationDTOs[0].Percentage = (top3StorageLocations.Data[0].CountPcbs * 100/ pcbInCirculation.Data);
            }

            if (StorageLocationDTOs.Count >= 2)
            {
                StorageLocationDTOs[1].Number = "2.";
                StorageLocationDTOs[1].Percentage = (top3StorageLocations.Data[1].CountPcbs * 100 / pcbInCirculation.Data);
            }

            if (StorageLocationDTOs.Count == 3)
            {
                StorageLocationDTOs[2].Number = "3.";
                StorageLocationDTOs[2].Percentage = (top3StorageLocations.Data[2].CountPcbs * 100 / pcbInCirculation.Data);
            }
        }

        var dwellTimeDTOs = await _dashboardDataService.GetDwellTimeDTO();

        if (dwellTimeDTOs.Code == ResponseCode.Success)
        {

            var code1 = StatusCodeExists(dwellTimeDTOs.Data, 1);
            if (code1.Item1)
            {
                code1.Item2.Color = "green";
                code1.Item2.Percentage = code1.Item2.CountDwellTimeStatus * 100 / pcbInCirculation.Data;
                DwellTimeDTOs.Add(code1.Item2);
            }
            else
            {
                DwellTimeDTOs.Add(new DashboardDwellTimeDTO()
                {
                    CountDwellTimeStatus = 0,
                    Percentage = 0,
                    Color = "green"
                });
            }

            var code2 = StatusCodeExists(dwellTimeDTOs.Data, 2);
            if (code2.Item1)
            {
                code2.Item2.Color = "yellow";
                code2.Item2.Percentage = code2.Item2.CountDwellTimeStatus * 100 / pcbInCirculation.Data;
                DwellTimeDTOs.Add(code2.Item2);
            }
            else
            {
                DwellTimeDTOs.Add(new DashboardDwellTimeDTO()
                {
                    CountDwellTimeStatus = 0,
                    Percentage = 0,
                    Color = "yellow"
                });
            }

            var code3 = StatusCodeExists(dwellTimeDTOs.Data, 3);
            if (code3.Item1)
            {
                code3.Item2.Color = "red";
                code3.Item2.Percentage = code3.Item2.CountDwellTimeStatus * 100 / pcbInCirculation.Data;
                DwellTimeDTOs.Add(code3.Item2);
            }
            else
            {
                DwellTimeDTOs.Add(new DashboardDwellTimeDTO()
                {
                    CountDwellTimeStatus = 0,
                    Percentage = 0,
                    Color = "red"
                });
            }
        }
    }

    private (bool, DashboardDwellTimeDTO) StatusCodeExists(List<DashboardDwellTimeDTO> dto, int code)
    {
        int count = 0;
        foreach (var item in dto)
        {
            if (item.DwellTimeStatus == code)
            {
                return (true, item);
            }
            count++;
        }
        return (false, null);
    }
}
