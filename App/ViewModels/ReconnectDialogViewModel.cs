using App.Core.DataAccess;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public sealed partial class ReconnectDialogViewModel : ObservableObject
    {
        private BoschContext _boschContext;
        public ReconnectDialogViewModel(BoschContext boschContext)
        {
            _boschContext = boschContext;
            CheckConCommand = new AsyncRelayCommand(CheckConnection);
            
        }

        [ObservableProperty]
        private bool _connectionAvailable;

        public IAsyncRelayCommand CheckConCommand { get; }

        public async Task<bool> CheckConnection()
        {
                return await _boschContext.Database.CanConnectAsync();
        }
    }
}
