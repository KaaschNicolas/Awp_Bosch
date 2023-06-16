using App.Core.DataAccess;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels
{
    public sealed partial class ReconnectDialogViewModel : ObservableObject
    {
        private BoschContext _boschContext;
        public ReconnectDialogViewModel(BoschContext boschContext)
        {
            _boschContext = boschContext;
        }

        [ObservableProperty]
        private bool _isConnected;


        [RelayCommand]
        public async Task CheckConnection()
        {
            IsConnected = await _boschContext.Database.CanConnectAsync();
        }
    }
}
