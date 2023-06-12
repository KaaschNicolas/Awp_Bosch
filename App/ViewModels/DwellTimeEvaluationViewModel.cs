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

        [ObservableProperty]
        private DateTime _from;

        [ObservableProperty]
        private DateTime _to;

        public DateTime MaxDate { get; private set; } = DateTime.Now;

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
            var barSeries = new BarSeries 
            { 
                XAxisKey = "Value",
                YAxisKey = "Category",
                LabelPlacement = LabelPlacement.Outside 
            };

            List<BarItem> barItems = await FillData();
            barItems.ForEach(x => x.Color = myColors[myRandom.Next(myColors.Count - 1)]);
            barItems.ForEach(x => barSeries.Items.Add(x));
            model.Series.Add(barSeries);

            // specify key and position
            var categoryAxis = new CategoryAxis
            { 
                Position = AxisPosition.Bottom,
                Key = "Category",
                IsZoomEnabled = false 
            };
            var storageLocation = await _storageLocationDataService.GetAll();
            storageLocation.Data.ForEach(x => categoryAxis.Labels.Add(x.StorageName));
            
            model.Axes.Add(categoryAxis);

            // specify key and position
            var valueAxis = new LinearAxis 
            { 
                Position = AxisPosition.Left,
                Key = "Value",
                IsZoomEnabled = false,
                Title = "Tage",
                TitlePosition = 0.9
            };
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
                Dictionary<int, int> keys = new();
                
                foreach (var item in data.Data)
                {
                    if (!keyValuePairs.Keys.Contains(item.StorageLocation))
                    {
                        keys.Add(item.StorageLocationId, 0);
                        keyValuePairs.Add(item.StorageLocation, 0);
                    }
                }

                //var g = from x in data.Data group x by x.PcbId;
                
                Dictionary<int, List<Transfer>> groupedTransfers = new();
                foreach(var group in data.Data.GroupBy(it => it.PcbId))
                {
                    var pcb = group.Key;
                    var date = group.ToList();
                    groupedTransfers.Add(group.Key, group.ToList());
                }

                foreach (var item in groupedTransfers.Values)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        if (i != 0)
                        {
                            keys[item[i - 1].StorageLocationId]++; 
                            keyValuePairs[item[i - 1].StorageLocation] += Math.Round((item[i].CreatedDate - item[i - 1].CreatedDate).TotalDays, 2);
                        } else if (item.Count == 1)
                        {
                            keys[item[0].StorageLocationId]++;
                            keyValuePairs[item[0].StorageLocation] += Math.Round((DateTime.Now - item[0].CreatedDate).TotalDays);
                        }
                    }
                }

                Dictionary<StorageLocation, double> finalRes = new();
                foreach (var value in keyValuePairs)
                {
                    finalRes.Add(value.Key, Math.Round(value.Value / keys[value.Key.Id], 2));
                }

                foreach (var item in finalRes)
                {
                    barItems.Add(new BarItem(item.Value));
                }
            }
            return barItems;
        }

        private Random myRandom = new Random();

        private List<OxyColor> myColors = new List<OxyColor>()
        {
            OxyColor.FromRgb(0, 191, 70),
            OxyColor.FromRgb(222, 0, 26),
            OxyColor.FromRgb(0, 232, 85),
            OxyColor.FromRgb(194, 43, 240),
            OxyColor.FromRgb(43, 151, 240),
            OxyColor.FromRgb(5, 93, 166),
            OxyColor.FromRgb(5, 166, 134)
        };

        private void Refresh()
        {
            GeneratePlotCommand.ExecuteAsync(null);
        }
    }
}
