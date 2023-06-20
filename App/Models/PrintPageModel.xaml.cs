using App.Core.Models;
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

        public PrintPageModel(string seriennummer, string sachnummer, BitmapImage datamatrix, string einschraenkung, Comment panel, string status, int umlaufTage, string aktuellerStandort, int verweildauer, string letzteBearbeitung, string oberfehler, string oberfehlerBeschreibung, string unterfehler, string unterfehlerBeschreibung)
        {
            this.InitializeComponent();
            datumTextBlock.Text = DateTime.Now.ToString("dd.MM.yyy HH:mm:ss");
            seriennummerTextBlock.Text = seriennummer;
            sachnummerTextBlock.Text = sachnummer;
            datamatrixImage.Source = datamatrix;
            einschraenkungInfoBar.Message = einschraenkung;
            anmerkungenTextBlock.Text = panel == null ? "" : panel.Content;
            anmerkungenBearbeitetvonTextBlock.Text = "bearbeitet von: " + letzteBearbeitung;
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

        public PrintPageModel(PcbPrintPageDTO p)
        {
            this.InitializeComponent();
            datumTextBlock.Text = DateTime.Now.ToString("dd.MM.yyy HH:mm:ss");
            seriennummerTextBlock.Text = p.Seriennummer;
            sachnummerTextBlock.Text = p.Sachnummer;
            datamatrixImage.Source = p.Datamatrix;
            einschraenkungInfoBar.Message = p.Einschraenkung;
            anmerkungenTextBlock.Text = p.Panel == null ? "" : p.Panel.Content;
            anmerkungenBearbeitetvonTextBlock.Text = "bearbeitet von: " + p.LetzteBearbeitung;
            statusTextBlock.Text = p.Status;
            umlaufTageTextBlock.Text = p.UmlaufTage.ToString();
            aktuellerStandortTextBlock.Text = p.AktuellerStandort;
            verweildauerTextBlock.Text = p.Verweildauer.ToString();
            letzteBearbeitungTextBlock.Text = p.LetzteBearbeitung;
            oberfehlerTextBLock.Text = p.Oberfehler;
            oberfehlerBeschreibungTextBlock.Text = p.OberfehlerBeschreibung;
            unterfehlerTextBlock.Text = p.Unterfehler;
            unterfehlerBeschreibungTextBlock.Text = p.UnterfehlerBeschreibung;
        }
    }
}