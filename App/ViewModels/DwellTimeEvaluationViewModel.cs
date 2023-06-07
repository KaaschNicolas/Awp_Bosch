using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;
using App.Core.Services;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
            GeneratePlotCommand = new AsyncRelayCommand(
                GeneratePlot
            );
            Refresh();  
        }

        private ITransferDataService<Transfer> _transferDataService;
        private IStorageLocationDataService<StorageLocation> _storageLocationDataService;

        public IAsyncRelayCommand GeneratePlotCommand { get; }

        public async Task GeneratePlot()
        {
            DwellTimeBarPlot = new PlotModel();
            DwellTimeBarPlot.PlotAreaBorderColor = OxyColors.Transparent;
            DwellTimeBarPlot = await ColumnSeries();
        }

        public async Task<PlotModel> ColumnSeries()
        {
            var model = new PlotModel();

            // specify axis keys
            var barSeries = new BarSeries { XAxisKey = "Value", YAxisKey = "Category" };
            List<BarItem> barItems = await FillData();
            barItems.ForEach(x => barSeries.Items.Add(x));
            
            model.Series.Add(barSeries);

            // specify key and position
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom, Key = "Category" };
            categoryAxis.IsZoomEnabled = false;
            var storageLocation = await _storageLocationDataService.GetAll();
            storageLocation.Data.ForEach(x => categoryAxis.Labels.Add(x.StorageName));
            
            model.Axes.Add(categoryAxis);

            // specify key and position
            var valueAxis = new LinearAxis { Position = AxisPosition.Left, Key = "Value" };
            valueAxis.IsZoomEnabled = false;
            model.Axes.Add(valueAxis);

            return model;
        }

        public async Task<List<BarItem>> FillData()
        {
            var data = await _transferDataService.GetAllEager();
            var barItems = new List<BarItem>();
            if (data.Code == ResponseCode.Success)
            {
                Dictionary<StorageLocation, double> keyValuePairs = new Dictionary<StorageLocation, double>();
                int count = 0;
                Dictionary<int, int> keys = new();
                foreach (var key in keyValuePairs.Keys)
                {
                    keys.Add(key.Id, keys[key.Id] + 1);
                }
                foreach (var item in data.Data)
                {
                    if (!keyValuePairs.Keys.Contains(item.StorageLocation))
                    {
                        count++;
                        keyValuePairs.Add(item.StorageLocation, Math.Round((DateTime.Now - item.CreatedDate).TotalDays));
                    }
                    else
                    {
                        count++;
                        keyValuePairs[item.StorageLocation] += Math.Round((DateTime.Now - item.CreatedDate).TotalDays);
                    }
                }

                foreach (var item in keyValuePairs)
                {
                    barItems.Add(new BarItem(item.Value));
                }
            }
            return barItems;
        }

        private void Refresh()
        {
            GeneratePlotCommand.ExecuteAsync(null);
        }
    }
}
