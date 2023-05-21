
using Microsoft.UI.Xaml.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;

namespace App.Helpers
{
    public class BitmapToBitmapImageConverter
    {
        public BitmapToBitmapImageConverter()
        {
        }

        public static BitmapImage GetBitmapImage(Bitmap bitmap)
        {
            return convertBitmap(bitmap);
        }

        private static BitmapImage convertBitmap(Bitmap bitmap)
        {
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(memoryStream.AsRandomAccessStream());
            memoryStream.Dispose();
            return bitmapImage;
            }
        }
}