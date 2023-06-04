using App.Activation;
using App.Contracts.Services;
using App.Core.Services.Interfaces;
using App.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace App.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IDialogService _dialogService;
    private UIElement? _shell = null;

    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers, IThemeSelectorService themeSelectorService, IAuthenticationService authenticationService, IDialogService dialogService)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _themeSelectorService = themeSelectorService;
        _authenticationService = authenticationService;
        _dialogService = dialogService;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        // Execute tasks before activation.
        await InitializeAsync();

        // Set the MainWindow Content.
        if (App.MainWindow.Content == null)
        {
            _shell = App.GetService<ShellPage>();
            App.MainWindow.Content = _shell ?? new Frame();
        }

        // Handle activation via ActivationHandlers.
        await HandleActivationAsync(activationArgs);

        // Activate the MainWindow.
        App.MainWindow.Activate();

        // Execute tasks after activation.
        await StartupAsync();
    }

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        await _themeSelectorService.SetRequestedThemeAsync();
        await Task.CompletedTask;
        if (!_authenticationService.IsDbActive)
        {
            if (App.MainWindow.Content is FrameworkElement fe)
            {
                fe.Loaded += (ss, ee) => _dialogService.UnAuthorizedDialogAsync("No Connection", "Es besteht keine Verbindung zur Datenbank, \n Bitte wenden sie sich an einen Vorgesetzten.", App.MainWindow.Content.XamlRoot);
                return;
            }
            
            
           if (!_authenticationService.IsAuthenticated)
            {
                if (App.MainWindow.Content is FrameworkElement fr)
                {
                    fr.Loaded += (ss, ee) => _dialogService.UnAuthorizedDialogAsync("Unauthorized", "Sie haben keine Berechtigungen diese Anwendung zu nutzen.\n Bitte wenden sie sich an ihren nächsten Vorgesetzten,\n wenn Sie dennoch Zugriff benötigen.", App.MainWindow.Content.XamlRoot);
                    return;
                }
                return;
            } 
        }

        
    }
}
