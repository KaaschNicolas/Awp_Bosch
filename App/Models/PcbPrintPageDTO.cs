using App.Core.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace App.Models
{
    public class PcbPrintPageDTO
    {
        public string Seriennummer { get; set; }
        public string Sachnummer { get; set; }
        public BitmapImage Datamatrix { get; set; }
        public string Einschraenkung { get; set; }
        public Comment Panel { get; set; }
        public string Status { get; set; }
        public int UmlaufTage { get; set; }
        public string AktuellerStandort { get; set; }
        public int Verweildauer { get; set; }
        public string LetzteBearbeitung { get; set; }
        public string Oberfehler { get; set; }
        public string OberfehlerBeschreibung { get; set; }
        public string Unterfehler { get; set; }
        public string UnterfehlerBeschreibung { get; set; }
    }
}
