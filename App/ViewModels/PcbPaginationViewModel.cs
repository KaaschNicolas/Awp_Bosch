using App.Contracts.Services;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public partial class PcbPaginationViewModel : ObservableRecipient
    {
        public PcbPaginationViewModel(IPcbDataService<Pcb> crudService, IInfoBarService infoBarService, IDialogService dialogService, INavigationService navigationService) 
        {
            _crudService = crudService;
            FirstAsyncCommand = new AsyncRelayCommand(
                async () => await GetPcbs(1, _pageSize, _isSortingAscending),
                () => _pageNumber != 1
            );

            PreviousAsyncCommand = new AsyncRelayCommand(
                async () => await GetPcbs(_pageNumber -1, _pageSize, _isSortingAscending),
                () => _pageNumber > 1 
            );

            NextAsyncCommand = new AsyncRelayCommand(
                async () => await GetPcbs(_pageNumber + 1, _pageSize, _isSortingAscending),
                () => _pageNumber < _pageCount
            );

            LastAsyncCommand = new AsyncRelayCommand(
                async () => await GetPcbs(_pageCount, _pageSize, _isSortingAscending),
                () => _pageNumber != _pageCount
            );

            SortByCommand = new AsyncRelayCommand(
                async () => await GetPcbs(_pageNumber, _pageSize, _isSortingAscending),
                () => _pageNumber != _pageCount
            );

            FilterItems = new AsyncRelayCommand(
                async () => await GetPcbs(_pageNumber, _pageSize, _isSortingAscending),
                () => _pageNumber != _pageCount && _filterOptions != PcbFilterOptions.None
            );
        }

        private readonly IPcbDataService<Pcb> _crudService;
        private readonly IInfoBarService _infoBarService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        public IAsyncRelayCommand FirstAsyncCommand { get; }
        public IAsyncRelayCommand PreviousAsyncCommand { get; }
        public IAsyncRelayCommand NextAsyncCommand { get; }
        public IAsyncRelayCommand LastAsyncCommand { get; }
        public IAsyncRelayCommand SortByCommand { get; }
        public IAsyncRelayCommand FilterItems { get; }

        private int _pageSize = 10;
        private int _pageNumber;
        private int _pageCount;
        private string _queryText;
        private string _sortyBy;

        [ObservableProperty]
        private ObservableCollection<Pcb> _pcbs;

        [ObservableProperty]
        private bool _isSortingAscending;

        [ObservableProperty]
        private PcbFilterOptions _filterOptions;

        public List<int> PageSizes => new() { 5, 10, 15, 20 };

        public string QueryText { get => _queryText; set => SetProperty(ref _queryText, value); }

        public int PageNumber { get => _pageNumber; set => SetProperty(ref _pageNumber, value); }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                SetProperty(ref _pageSize, value);
                Refresh();
            }
        }

        public int PageCount { get => _pageCount; set => SetProperty(ref _pageCount, value); }

        public string SortBy { get => _sortyBy; set => SetProperty(ref _sortyBy, value); }

        [ObservableProperty]
        private Pcb _selectedItem;

        private async Task GetPcbs(int pageIndex, int pageSize, bool isAscending)
        {
            Response<List<Pcb>> pcbs;
            Response<int> maxEntries;

            if (FilterOptions != PcbFilterOptions.None)
            {
                switch (_filterOptions)
                {
                    case PcbFilterOptions.None:
                        Expression<Func<Pcb, bool>> where1 = x => x.Finalized == true;
                        maxEntries = await _crudService.MaxEntriesFiltered(where1);
                        pcbs = await _crudService.GetWithFilter(pageIndex, pageSize, where1);
                        break;
                    case PcbFilterOptions.Search:
                        break;
                    case PcbFilterOptions.Filter1:
                        break;
                    case PcbFilterOptions.Filter2:
                        break;
                    case PcbFilterOptions.Filter3:
                        break;
                    default:
                        break;
                }
            }
        }

        [RelayCommand]
        public async void Delete()
        {
            var result = await _dialogService.ConfirmDeleteDialogAsync("Leiterplatte Löschen", "Sind Sie sicher, dass Sie diesen Eintrag löschen wollen?");
            if (result != null && result == true)
            {
                Pcb pcbToRemove = _selectedItem;
                _pcbs.Remove(pcbToRemove);
                await _crudService.Delete(pcbToRemove);
                _infoBarService.showMessage("Erfolgreich Leiterplatte gelöscht", "Erfolg");
            }
        }

        [RelayCommand]
        public void NavigateToUpdate(Pcb pcb)
        {
            _navigationService.NavigateTo("App.ViewModels.PcbSingleViewModel", pcb);
        }

        private void Refresh()
        {
            _pageNumber = 0;
            FirstAsyncCommand.ExecuteAsync(null);
        }
    }
}
