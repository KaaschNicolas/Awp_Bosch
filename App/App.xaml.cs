using App.Activation;
using App.Contracts.Services;
using App.Core.Contracts.Services;
using App.Core.DataAccess;
using App.Core.DTOs;
using App.Core.Helpers;
using App.Core.Models;
using App.Core.Services;
using App.Core.Services.Interfaces;
using App.Helpers;
using App.Models;
using App.Services;
using App.ViewModels;
using App.Views;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

using Serilog;

using Windows.Devices.WiFiDirect.Services;

namespace App;
// Die Klasse "App" ist die Hauptklasse der Anwendung und erbt von der Klasse "Application" in WinUI 3.
// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // Der .NET Generic Host bietet Dependency Injection, Konfiguration, Logging und andere Dienste.
    // In diesem Fall wird der Host verwendet, um die Dienste der Anwendung zu verwalten.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    // Methode zum Abrufen eines Dienstes aus dem Host.
    // Es wird das generische Type-Argument "T" erwartet, das den gewünschten Dienst repräsentiert.
    public static T GetService<T>()
        where T : class
    {
        // Überprüfen, ob der Dienst im Host registriert ist um ihn zurückgeben.
        // Andernfalls wird eine ArgumentException ausgelöst.
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    // Statische Eigenschaft, die das Hauptfenster der Anwendung repräsentiert.
    public static WindowEx MainWindow { get; } = new MainWindow();

    // Konstruktor der Klasse "App".
    public App()
    {
        InitializeComponent();

        // Konfiguration der Logger-Komponente "Serilog" aus der Konfigurationsdatei.
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(ConfigurationHelper.Configuration)
            .CreateLogger();

        // Konfiguration des Hosts.
        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        UseSerilog().
        ConfigureServices((context, services) =>
        {
            // Registrierung von Diensten

            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IInfoBarService, InfoBarService>();
            services.AddTransient<IDialogService, DialogService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();
            services.AddTransient<ILoggingService, LoggingService>();

            services.AddTransient<ICrudService<Pcb>, CrudService<Pcb>>();
            services.AddTransient<ICrudService<PcbType>, CrudService<PcbType>>();
            services.AddTransient<ICrudService<StorageLocation>, CrudService<StorageLocation>>();
            services.AddTransient<ICrudService<User>, CrudService<User>>();
            services.AddTransient<ICrudService<Diagnose>, CrudService<Diagnose>>();
            services.AddTransient<ICrudService<Comment>, CrudService<Comment>>();
            services.AddTransient<ICrudService<Device>, CrudService<Device>>();

            services.AddTransient<IStorageLocationDataService<StorageLocation>, StorageLocationDataService<StorageLocation>>();
            services.AddTransient<IPcbDataService<Pcb>, PcbDataService<Pcb>>();
            services.AddTransient<ITransferDataService<Transfer>, TransferDataService<Transfer>>();
            services.AddTransient<IMockDataService, MockDataService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IPcbTypeEvaluationService, PcbTypeEvaluationService>();
            services.AddTransient<IDashboardDataService<BaseEntity>, DashboardDataService<BaseEntity>>();


            // Views and ViewModels
            // Hier werden die Views und ViewModels der Anwendung registriert.
            services.AddTransient<PcbTypeI_OEvaluationViewModel>();
            services.AddTransient<PcbTypeI_OEvaluationPage>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<DashboardPage>();

            services.AddTransient<PcbTypeEvaluationViewModel>();
            services.AddTransient<PcbTypeEvaluationPage>();
            services.AddTransient<UpdateUserViewModel>();
            services.AddTransient<UpdateUserPage>();
            services.AddTransient<CreateUserViewModel>();
            services.AddTransient<CreateUserPage>();
            services.AddTransient<UsersViewModel>();
            services.AddTransient<UsersPage>();
            services.AddTransient<TransferDialogViewModel>();
            services.AddTransient<ICrudService<Comment>, CrudService<Comment>>();
            services.AddTransient<ICrudService<Device>, CrudService<Device>>();
            services.AddTransient<UpdatePcbViewModel>();
            services.AddTransient<UpdatePcbPage>();
            services.AddTransient<CreatePcbViewModel>();
            services.AddTransient<CreatePcbPage>();
            services.AddTransient<TransfersViewModel>();
            services.AddTransient<TransfersPage>();
            services.AddTransient<PcbSingleViewModel>();
            services.AddTransient<PcbSinglePage>();
            services.AddTransient<PcbPaginationViewModel>();
            services.AddTransient<UpdateStorageLocationPage>();
            services.AddTransient<UpdateStorageLocationViewModel>();
            services.AddTransient<UpdatePcbTypeViewModel>();
            services.AddTransient<UpdatePcbTypePage>();
            services.AddTransient<CreatePcbTypeViewModel>();
            services.AddTransient<CreatePcbTypePage>();
            services.AddTransient<StorageLocationPaginationViewModel>();
            services.AddTransient<DiagnoseViewModel>();
            services.AddTransient<DiagnosePage>();
            services.AddTransient<UpdateDiagnoseViewModel>();
            services.AddTransient<UpdateDiagnosePage>();
            services.AddTransient<CreateDiagnoseViewModel>();
            services.AddTransient<CreateDiagnosePage>();
            services.AddTransient<PcbTypeViewModel>();
            services.AddTransient<PcbTypePage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<BlankViewModel>();
            services.AddTransient<BlankPage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<CreateStorageLocationViewModel>();
            services.AddTransient<StorageLocation>();
            services.AddTransient<StorageLocationViewModel>();
            services.AddTransient<TransferDialogViewModel>();
            services.AddTransient<DwellTimeEvaluationViewModel>();
            services.AddTransient<DwellTimeEvalutionPage>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));

            // Registrierung des DbContexts "BoschContext" für die Datenbankverbindung.
            services.AddDbContext<BoschContext>(
                options => options.UseSqlServer(ConfigurationHelper.Configuration.GetConnectionString("BoschContext")),
                ServiceLifetime.Transient);
        }).
        Build();

        // Aktuellen Benutzer und seine Rolle im AuthServiceHelper speichern.
        var authService = Host.Services.GetService(typeof(IAuthenticationService)) as AuthenticationService;

        AuthServiceHelper.Rolle = authService.CurrentUser.Role;

        // Event-Handler für unbehandelte Ausnahmen registrieren.
        UnhandledException += App_UnhandledException;
    }

    // Event-Handler für unbehandelte Ausnahmen.
    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
        var ex = e.Exception;
    }

    // Überschriebene Methode "OnLaunched", die beim Start der Anwendung aufgerufen wird.
    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);


    }
}
