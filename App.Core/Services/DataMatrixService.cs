using App.Core.Services.Interfaces;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;

namespace App.Core.Services
{
    public class DataMatrixService : IDataMatrixService
    {
        public Bitmap GetDataMatrix(string data)
        {
            return GenerateDataMatrix(data);
        }
        
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

        private Bitmap GenerateDataMatrix(string data)
        {
            BarcodeWriter<Bitmap> barcodeWriter = new()
            {
                Format = BarcodeFormat.DATA_MATRIX,
                Options = new ZXing.Common.EncodingOptions
                {
                    Margin = 1,
                    PureBarcode = true,
                    Height = 150,
                    Width = 150
                },
                Renderer = new BitmapRenderer()
            };

            Bitmap barcodeBitmap = barcodeWriter.Write(data);

            return barcodeBitmap;
        }
    }
}
