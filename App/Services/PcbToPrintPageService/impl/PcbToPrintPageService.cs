using App.Core.Models;
using App.Models;
using Microsoft.UI.Xaml.Controls;

namespace App.Services.PcbToPrintPageService.impl
{
    public class PcbToPrintPageService : IPcbToPrintPageService
    {
        public Page ConvertByPcb(Pcb pcb)
        {
            var printPageModel = new PrintPageModel();

            return printPageModel;
        }

        private Page ConvertByPcbId(int id)
        {
            return null;
        }

    }
}
