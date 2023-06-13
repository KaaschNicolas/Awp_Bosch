using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.DTOs;
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
        private DateTime? _from;

        [ObservableProperty]
        private DateTime? _to;

        public DateTime MaxDate { get; private set; } = DateTime.Now;

        public DwellTimeEvaluationViewModel(ITransferDataService<Transfer> transferDataService, IStorageLocationDataService<StorageLocation> storageLocationDataService)
        {
            _transferDataService = transferDataService;
            _storageLocationDataService = storageLocationDataService;
            GeneratePlotCommand = new AsyncRelayCommand(
                async () => await GeneratePlot()
            );
              
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

            var avgDwellTimeDTO = await _transferDataService.GetAvgDwellTimeByStorageLocation(_from, _to);
            
            if (avgDwellTimeDTO.Code == ResponseCode.Success)
            {
                var barItems = new List<BarItem>();
                avgDwellTimeDTO.Data.ForEach(x => barItems.Add(new BarItem() { Value = x.AvgDwellTime }));
                barItems.ForEach(x => barSeries.Items.Add(x));
                //barItems.ForEach(x => x.Color = myColors[myRandom.Next(myColors.Count - 1)]);
                model.Series.Add(barSeries);

                // specify key and position
                var categoryAxis = new CategoryAxis
                { 
                    Position = AxisPosition.Bottom,
                    Key = "Category",
                    IsZoomEnabled = false 
                };
                var storageLocation = await _storageLocationDataService.GetAll();
                avgDwellTimeDTO.Data.ForEach(x => categoryAxis.Labels.Add(x.StorageName));
                model.Axes.Add(categoryAxis);
            }

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
