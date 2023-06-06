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

        public DwellTimeEvaluationViewModel(ITransferDataService<Transfer> transferDataService, IStorageLocationDataService<StorageLocation> storageLocationDataService)
        {
            _transferDataService = transferDataService;
            _storageLocationDataService = storageLocationDataService;
            InitializeBarPlot();    
        }

        private ITransferDataService<Transfer> _transferDataService;
        private IStorageLocationDataService<StorageLocation> _storageLocationDataService;

        private async Task InitializeBarPlot()
        {
            DwellTimeBarPlot = new PlotModel();
            DwellTimeBarPlot.PlotAreaBorderColor = OxyColors.Transparent;
            DwellTimeBarPlot = await ColumnSeries();

            //var barSeries = new HistogramSeries()
            //{
            //    ItemsSource = await GenerateBarItems(),
            //    LabelPlacement = LabelPlacement.Outside,
            //    TextColor = OxyColors.WhiteSmoke
            //};

            //DwellTimeBarPlot.Series.Add(barSeries);

            ////DwellTimeBarPlot.Axes.Add(new CategoryAxis
            ////{
            ////    Position = AxisPosition.Bottom,
            ////    Key = "StorageLocation"
            ////});

            //DwellTimeBarPlot.Axes.Add(new CategoryAxis
            //{
            //    Position = AxisPosition.Bottom,
            //    Key = "DwellTime",
            //});
            //DwellTimeBarPlot.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, MinimumPadding = 0, AbsoluteMinimum = 0 });
        }

        private async Task<List<HistogramItem>> GenerateBarItems()
        {
            var transfer = await _transferDataService.GetAll();

            if (transfer.Code == ResponseCode.Success)
            {
                var q = from x in transfer.Data
                        group x by new { x.StorageLocationId, CreatedAt = x.CreatedDate } into g
                        select new TransferDto(g.Key.StorageLocationId, g.Key.CreatedAt);
                List<HistogramItem> barItems = new();
                List<int> distinctStorageLocationIds = new();
                foreach(var item in q)
                {
                    if (!distinctStorageLocationIds.Contains(item.StorageLocationId))
                    {
                        distinctStorageLocationIds.Add(item.StorageLocationId);
                        barItems.Add(new HistogramItem(0, 1, 2, item.StorageLocationId));
                    }
                }
                
                return barItems;
            }
            return null;
        }

        public async Task<PlotModel> ColumnSeries()
        {
            var model = new PlotModel();

            // specify axis keys
            var barSeries = new BarSeries { XAxisKey = "Value", YAxisKey = "Category" };
            barSeries.Items.Add(new BarItem { Value = 10 });
            barSeries.Items.Add(new BarItem { Value = 12 });
            barSeries.Items.Add(new BarItem { Value = 16 });
            model.Series.Add(barSeries);

            // specify key and position
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom, Key = "Category" };
            var storageLocation = await _storageLocationDataService.GetAll();
            storageLocation.Data.ForEach(x => categoryAxis.Labels.Add(x.StorageName));
            
            model.Axes.Add(categoryAxis);

            // specify key and position
            var valueAxis = new LinearAxis { Position = AxisPosition.Left, Key = "Value" };
            model.Axes.Add(valueAxis);

            return model;
        }

        private record TransferDto(int StorageLocationId, DateTime CreatedAt);
    }
}
