using App.Core.Services;
using App.Core.Services.Interfaces;
using Moq;
using System.Drawing;
using Xunit;
using Assert = Xunit.Assert;

namespace App.Core.Tests.Services
{
    public class DataMatrixServiceTests
    {
        private readonly IDataMatrixService _dataMatrixService;

        public DataMatrixServiceTests()
        {
            _dataMatrixService = new DataMatrixService();
        }

        [Fact]
        public void GetDataMatrix_Should_Return_DataMatrix_Bitmap()
        {
            // Arrange
            string data = "Test Data";

            // Act
            Bitmap result = _dataMatrixService.GetDataMatrix(data);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Bitmap>(result);
        }

        [Fact]
        public void SaveDataMatrix_Should_Save_DataMatrix_Image_To_Path()
        {
            // Arrange
            string data = "Test Data";
            string path = "C:\\Images"; // Update with the desired path for testing

            // Act
            _dataMatrixService.SaveDataMatrix(data, path);

            // Assert
            // Check if the image file was saved to the specified path
            string expectedFilePath = Path.Combine(path, data + "_datamatrix.png");
            Assert.True(File.Exists(expectedFilePath));
        }
    }
}
