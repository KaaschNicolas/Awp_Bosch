using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;
using App.Core.Services;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace App.ViewModels
{
    public partial class DwellTimeEvaluationViewModel : ObservableObject
    {
        [ObservableProperty]
        private PlotModel _dwellTimeBarPlot;

        public DwellTimeEvaluationViewModel(ITransferDataService<Transfer> transferDataService)
        {
            _transferDataService = transferDataService;
            InitializeBarPlot();    
        }

        private ITransferDataService<Transfer> _transferDataService;

        private async Task InitializeBarPlot()
        {
            DwellTimeBarPlot = new PlotModel();
            DwellTimeBarPlot.PlotAreaBorderColor = OxyColors.Transparent;

            var barSeries = new BarSeries
            {
                ItemsSource = await GenerateBarItems(),
                LabelPlacement = LabelPlacement.Outside,
                TextColor = OxyColors.WhiteSmoke
            };

            DwellTimeBarPlot.Series.Add(barSeries);

            DwellTimeBarPlot.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Key = "Lagerort"
            });
        }

        private async Task<List<BarItem>> GenerateBarItems()
        {
            var transfer = await _transferDataService.GetAll();

            if (transfer.Code == ResponseCode.Success)
            {
                var q = from x in transfer.Data
                        group x by new { x.StorageLocationId, CreatedAt = x.CreatedDate } into g
                        select new TransferDto(g.Key.StorageLocationId, g.Key.CreatedAt);
                List<BarItem> barItems = new();
                List<int> distinctStorageLocationIds = new();
                foreach(var item in q)
                {
                    if (!distinctStorageLocationIds.Contains(item.StorageLocationId))
                    {
                        distinctStorageLocationIds.Add(item.StorageLocationId);
                        barItems.Add(new BarItem(value: 1));
                    }
                }

                
                return barItems;
            }
            return null;
        }

        private record TransferDto(int StorageLocationId, DateTime CreatedAt);
    }
}
