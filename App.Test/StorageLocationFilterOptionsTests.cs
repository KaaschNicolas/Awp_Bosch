using App.Core.Models.Enums;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests.Enums
{
    [TestFixture]
    public class StorageLocationFilterOptionsTests
    {
        [Test]
        public void Enum_Values_Are_Correct()
        {
            // Arrange

            // Act

            // Assert
            Assert.AreEqual("DwellTimeYellowHigh", StorageLocationFilterOptions.DwellTimeYellowHigh.ToString());
            Assert.AreEqual("DwellTimeYellowLow", StorageLocationFilterOptions.DwellTimeYellowLow.ToString());
            Assert.AreEqual("DwellTimeRedHigh", StorageLocationFilterOptions.DwellTimeRedHigh.ToString());
            Assert.AreEqual("DwellTimeRedLow", StorageLocationFilterOptions.DwellTimeRedLow.ToString());
            Assert.AreEqual("None", StorageLocationFilterOptions.None.ToString());
            Assert.AreEqual("Search", StorageLocationFilterOptions.Search.ToString());
        }

        [Test]
        public void Enum_Values_Are_Unique()
        {
            // Arrange

            // Act

            // Assert
            Assert.That(Enum.GetValues(typeof(StorageLocationFilterOptions)), Is.Unique);
        }
    }
}
