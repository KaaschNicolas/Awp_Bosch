using App.Core.Models;
using Microsoft.UI.Xaml.Controls;
namespace App.Services.PcbToPrintPageService
{
    public interface IPcbToPrintPageService
    {
        Page ConvertByPcb(Pcb pcb);
    }
}
