using App.Core.Contracts.Services;
using App.Core.Models; // Assuming the Person class is defined in this namespace
using App.Core.Services;
using Moq;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Xunit;
using Assert = Xunit.Assert;

namespace App.Core.Tests.Services
{
    public class FileServiceTests
    {
        private readonly IFileService _fileService;
        private readonly Mock<IFileService> _fileServiceMock;

        public FileServiceTests()
        {
            _fileServiceMock = new Mock<IFileService>();
            _fileService = new FileService();
        }

        [Fact]
        public void Read_Should_Deserialize_Content_From_File()
        {
            // Arrange
            string folderPath = "C:\\Files"; // Update with the desired folder path for testing
            string fileName = "test.json";
            string fileContent = "{\"Name\":\"John\",\"Age\":30}"; // Sample JSON content
            var expectedContent = new Person { Name = "John", Age = 30 }; // Sample expected deserialized object

            // Create the test file
            string filePath = Path.Combine(folderPath, fileName);
            File.WriteAllText(filePath, fileContent, Encoding.UTF8);

            // Act
            var result = _fileService.Read<Person>(folderPath, fileName);

            // Assert
            Assert.Equal(expectedContent, result);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void Save_Should_Serialize_Content_And_Write_To_File()
        {
            // Arrange
            string folderPath = "C:\\Files"; // Update with the desired folder path for testing
            string fileName = "test.json";
            var content = new Person { Name = "John", Age = 30 }; // Sample content to save

            // Act
            _fileService.Save<Person>(folderPath, fileName, content);

            // Assert
            string filePath = Path.Combine(folderPath, fileName);
            Assert.True(File.Exists(filePath));

            string fileContent = File.ReadAllText(filePath, Encoding.UTF8);
            var deserializedContent = JsonConvert.DeserializeObject<Person>(fileContent);
            Assert.Equal(content, deserializedContent);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void Delete_Should_Delete_Existing_File()
        {
            // Arrange
            string folderPath = "C:\\Files"; // Update with the desired folder path for testing
            string fileName = "test.json";
            string filePath = Path.Combine(folderPath, fileName);

            // Create the test file
            File.WriteAllText(filePath, "", Encoding.UTF8);

            // Act
            _fileService.Delete(folderPath, fileName);

            // Assert
            Assert.False(File.Exists(filePath));
        }

        private class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
