using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace App.Models
{
    public sealed partial class PrintPageModel : Page
    {
        public PrintPageModel()
        {
            this.InitializeComponent();
        }

        public PrintPageModel(string seriennummer, string leiterplatte, BitmapImage datamatrix, string einschraenkung, string anmerkungen, string status, int umlaufTage, string aktuellerStandort, int verweildauer, string letzteBearbeitung, string oberfehler, string oberfehlerBeschreibung, string unterfehler, string unterfehlerBeschreibung)
        {
            this.InitializeComponent();
            datumTextBlock.Text = DateTime.Now.ToString("dd.MM.yyy HH:mm:ss");
            seriennummerTextBlock.Text = seriennummer;
            leiterplatteTextBlock.Text = leiterplatte;
            datamatrixImage.Source = datamatrix;
            einschraenkungInfoBar.Message = einschraenkung;
            anmerkungenTextBlock.Text = anmerkungen;
            statusTextBlock.Text = status;
            umlaufTageTextBlock.Text = umlaufTage.ToString();
            aktuellerStandortTextBlock.Text = aktuellerStandort;
            verweildauerTextBlock.Text = verweildauer.ToString();
            letzteBearbeitungTextBlock.Text = letzteBearbeitung;
            oberfehlerTextBLock.Text = oberfehler;
            oberfehlerBeschreibungTextBlock.Text = oberfehlerBeschreibung;
            unterfehlerTextBlock.Text = unterfehler;
            unterfehlerBeschreibungTextBlock.Text = unterfehlerBeschreibung;
        }
    }
}
