using App.Core.Services;
using App.Core.Services.Interfaces;
using App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Printing;
using System.Diagnostics;
using Windows.Graphics.Printing;

namespace App.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    // START: BOS-230
    private PrintManager printMan;
    private PrintDocument printDoc;
    private IPrintDocumentSource printDocSource;
    private IDataMatrixService dataMatrixService;
    // END BOS-230

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }


    #region Register for printing

    void RegisterPrint()
    {
        // Register for PrintTaskRequested event
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        printMan = PrintManagerInterop.GetForWindow(hWnd);
        printMan.PrintTaskRequested += PrintTaskRequested;
        printDoc = new PrintDocument();
        printDocSource = printDoc.DocumentSource;
        printDoc.Paginate += Paginate;
        printDoc.GetPreviewPage += GetPreviewPage;
        printDoc.AddPages += AddPages;
    }

    #endregion

    private async void OnPrintButtonClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            //RegisterPrint();
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            await PrintManagerInterop.ShowPrintUIForWindowAsync(hWnd);
        }
        catch (Exception ex)
        {
            {
                Debug.WriteLine(ex.Message);
                ContentDialog noPrintingDialog = new()
                {
                    XamlRoot = (sender as Button).XamlRoot,
                    Title = "Printing error",
                    Content = "\nSorry, printing can' t proceed at this time.",
                    PrimaryButtonText = "OK"
                };
                await noPrintingDialog.ShowAsync();
            }
        }
    }


    private void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
    {
        var printTask = args.Request.CreatePrintTask("Laufzettel", PrintTaskSourceRequested);
        printTask.Completed += PrintTaskCompleted;
        printMan.PrintTaskRequested -= PrintTaskRequested;
    }

    private void PrintTaskSourceRequested(PrintTaskSourceRequestedArgs args)
    {
        args.SetSource(printDocSource);
    }

    #region Print preview

    private void Paginate(object sender, PaginateEventArgs e)
    {
        printDoc.SetPreviewPageCount(1, PreviewPageCountType.Final);
    }

    private void GetPreviewPage(object sender, GetPreviewPageEventArgs e)
    {
        printDoc.SetPreviewPage(e.PageNumber, ContentArea);
    }

    #endregion

    #region Add pages to send to the printer

    private void AddPages(object sender, AddPagesEventArgs e)
    {
        printDoc.AddPage(ContentArea);
        printDoc.AddPagesComplete();
    }

    #endregion

    #region Print task completed

    private void PrintTaskCompleted(PrintTask sender, PrintTaskCompletedEventArgs args)
    {
        DispatcherQueue.TryEnqueue(async () =>
            {
                ContentDialog PrintingDialog = new()
                {
                    XamlRoot = Content.XamlRoot,
                    Title = "Drucken",
                    Content = "\nDruckvorgang abgeschlossen.\nStatus: " + args.Completion,
                    PrimaryButtonText = "OK"
                };
                await PrintingDialog.ShowAsync();
            });
    }

    #endregion

    // Matrix Code generieren
    private async void OnDataMatrixButtonClicked(object sender, RoutedEventArgs e)
    {
        dataMatrixService = new DataMatrixService();
        var data = text_test.Text;
        var path = @"C:\Logs";
        dataMatrixService.SaveDataMatrix(data, path);
    }
}