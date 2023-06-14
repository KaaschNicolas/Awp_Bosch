using App.Core.Models.Enums;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests.Enums
{
    [TestFixture]
    public class PcbFilterOptionsTests
    {
        [Test]
        public void Enum_Values_Are_Correct()
        {
            // Arrange

            // Act

            // Assert
            Assert.AreEqual("None", PcbFilterOptions.None.ToString());
            Assert.AreEqual("Search", PcbFilterOptions.Search.ToString());
            Assert.AreEqual("Filter1", PcbFilterOptions.Filter1.ToString());
            Assert.AreEqual("Filter2", PcbFilterOptions.Filter2.ToString());
            Assert.AreEqual("Filter3", PcbFilterOptions.Filter3.ToString());
            Assert.AreEqual("FilterStorageLocation", PcbFilterOptions.FilterStorageLocation.ToString());
        }

        [Test]
        public void Enum_Values_Are_Unique()
        {
            // Arrange

            // Act

            // Assert
            Assert.That(Enum.GetValues(typeof(PcbFilterOptions)), Is.Unique);
        }
    }
}
