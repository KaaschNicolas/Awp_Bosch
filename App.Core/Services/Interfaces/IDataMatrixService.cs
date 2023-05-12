using System.Drawing;

namespace App.Core.Services.Interfaces
{
    public interface IDataMatrixService
    {
        Bitmap GetDataMatrix(string data);
        void SaveDataMatrix(string data, string path);
    }
}
