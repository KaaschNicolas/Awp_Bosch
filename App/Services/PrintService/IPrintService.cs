using Microsoft.UI.Xaml;

namespace App.Services.PrintService
{
    interface IPrintService
    {
        Task Print(UIElement contentElement);
    }
}
