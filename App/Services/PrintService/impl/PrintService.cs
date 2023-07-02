using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Printing;
using Windows.Graphics.Printing;

namespace App.Services.PrintService.impl;
public class PrintService : IPrintService
{
    private PrintManager _printManager;
    private PrintDocument _printDoc;
    private IPrintDocumentSource _printDocSource;
    private Page _contentArea;
    private IntPtr hWnd;

    public PrintService()
    {
         hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        _printManager = PrintManagerInterop.GetForWindow(hWnd);
        _printManager.PrintTaskRequested += PrintTaskRequested;
        _printDoc = new PrintDocument();
        _printDoc.Paginate += Paginate;
        _printDoc.GetPreviewPage += GetPreviewPage;
        _printDoc.AddPages += AddPages;
        _printDocSource = _printDoc.DocumentSource;
    }

    public async Task Print(Page page)
    {
        _contentArea = page;
        await PrintManagerInterop.ShowPrintUIForWindowAsync(hWnd);
    }

    private void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
    {
        var printTask = args.Request.CreatePrintTask("Drucken", PrintTaskSourceRequested);
        printTask.Completed += PrintTaskCompleted;
        _printManager.PrintTaskRequested -= PrintTaskRequested;
    }

    private void PrintTaskSourceRequested(PrintTaskSourceRequestedArgs args)
    {
        args.SetSource(_printDocSource);
    }

    private void Paginate(object sender, PaginateEventArgs e)
    {
        _printDoc.SetPreviewPage(1, _contentArea);
    }

    private void GetPreviewPage(object sender, GetPreviewPageEventArgs e)
    {
        _printDoc.SetPreviewPage(e.PageNumber, _contentArea);
    }

    private void AddPages(object sender, AddPagesEventArgs e)
    {
        _printDoc.AddPage(_contentArea);
        _printDoc.AddPagesComplete();
    }

    private void PrintTaskCompleted(object sender, PrintTaskCompletedEventArgs args)
    {
        //Platzhalter 
    }
}