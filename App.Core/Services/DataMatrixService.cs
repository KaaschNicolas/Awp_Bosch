using App.Core.Services.Interfaces;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Windows.Compatibility;

namespace App.Core.Services
{
    public class DataMatrixService : IDataMatrixService
    {
        // Generiert einen DataMatrix-Code als Bitmap anhand der angegebenen Daten
        public Bitmap GetDataMatrix(string data)
        {
            return GenerateDataMatrix(data);
        }

        // Generiert einen DataMatrix-Code anhand der angegebenen Daten und speichert ihn als PNG-Datei im angegebenen Pfad
        public void SaveDataMatrix(string data, string path)
        {
            try
            {
                var bitmap = GenerateDataMatrix(data);
                var filename = data + "_datamatrix.png";
                var fullPath = Path.Combine(path, filename);
                bitmap.Save(fullPath, ImageFormat.Png);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        // Generiert einen DataMatrix-Code als Bitmap anhand der angegebenen Daten
        private Bitmap GenerateDataMatrix(string data)
        {
            BarcodeWriter<Bitmap> barcodeWriter = new()
            {
                Format = BarcodeFormat.DATA_MATRIX,
                Options = new ZXing.Common.EncodingOptions
                {
                    Margin = 1,
                    PureBarcode = true,
                    Height = 25,
                    Width = 25
                },
                Renderer = new BitmapRenderer()
            };

            Bitmap barcodeBitmap = barcodeWriter.Write(data);

            return barcodeBitmap;
        }
    }
}
