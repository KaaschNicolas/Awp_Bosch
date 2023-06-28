using Microsoft.UI.Xaml.Controls;

namespace App.Services.PrintService
{
    interface IPrintService
    {
        Task Print(Page page);
    }
}
