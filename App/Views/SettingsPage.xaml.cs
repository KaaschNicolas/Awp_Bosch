using App.ViewModels;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Printing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Printing;
using System;
using Windows.Graphics.Printing;
using Windows.Graphics.Printing;
using System.ComponentModel;

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

        // Build a PrintDocument and register for callbacks
        printDoc = new PrintDocument();
        printDocSource = printDoc.DocumentSource;
        printDoc.Paginate += Paginate;
        printDoc.GetPreviewPage += GetPreviewPage;
        printDoc.AddPages += AddPages;
    }

    #endregion

    private async void OnPrintButtonClicked(object sender, RoutedEventArgs e)
    {
        RegisterPrint();
        try
        {
            // Show print UI
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            await PrintManagerInterop.ShowPrintUIForWindowAsync(hWnd);
        }
        catch
        {
            // Printing cannot proceed at this time
            ContentDialog noPrintingDialog = new ContentDialog()
            {
                XamlRoot = (sender as Button).XamlRoot,
                Title = "Printing error",
                Content = "\nSorry, printing can' t proceed at this time.",
                PrimaryButtonText = "OK"
            };
            await noPrintingDialog.ShowAsync();
        }
    }

    private void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
    {
        // Create the PrintTask.
        // Defines the title and delegate for PrintTaskSourceRequested
        var printTask = args.Request.CreatePrintTask("Print", PrintTaskSourceRequested);

        // Handle PrintTask.Completed to catch failed print jobs
        printTask.Completed += PrintTaskCompleted;
    }

    private void PrintTaskSourceRequested(PrintTaskSourceRequestedArgs args)
    {
        // Set the document source.
        args.SetSource(printDocSource);
    }

    #region Print preview

    private void Paginate(object sender, PaginateEventArgs e)
    {
        // Da nur 1 Seite gedruckt wird, wird die Seitenzahl auf 1 gesetzt
        printDoc.SetPreviewPageCount(1, PreviewPageCountType.Final);
    }

    private void GetPreviewPage(object sender, GetPreviewPageEventArgs e)
    {
        // Provide a UIElement as the print preview.
        printDoc.SetPreviewPage(e.PageNumber,ok);
    }

    #endregion

    #region Add pages to send to the printer

    private void AddPages(object sender, AddPagesEventArgs e)
    {
        printDoc.AddPage(ok);

        // Indicate that all of the print pages have been provided
        printDoc.AddPagesComplete();
    }

    #endregion

    #region Print task completed

    private void PrintTaskCompleted(PrintTask sender, PrintTaskCompletedEventArgs args)
    {
        // Notify the user when the print operation fails.
        if (args.Completion == PrintTaskCompletion.Failed)
        {
            DispatcherQueue.TryEnqueue(async () =>
            {
                ContentDialog noPrintingDialog = new ContentDialog()
                {
                    XamlRoot = Content.XamlRoot,
                    Title = "Drucken",
                    Content = "\nDruckvorgang fehlgeschlagen-",
                    PrimaryButtonText = "OK"
                };
                await noPrintingDialog.ShowAsync();
            });
        } else if (args.Completion == PrintTaskCompletion.Canceled)
        {
            DispatcherQueue.TryEnqueue(async () =>
            {
                ContentDialog noPrintingDialog = new ContentDialog()
                {
                    XamlRoot = Content.XamlRoot,
                    Title = "Drucken",
                    Content = "\nDruckvorgang abgebrochen-",
                    PrimaryButtonText = "OK"
                };
                await noPrintingDialog.ShowAsync();
            });
        }

    }

    #endregion

}
