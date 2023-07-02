using Microsoft.UI.Xaml.Controls;

namespace App.Models
{
    public sealed partial class PrintPageModel : Page
    {
        public PrintPageModel()
        {
            this.InitializeComponent();
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