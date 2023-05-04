using App.Core.Models;
using App.Core.Validator;
namespace App.Test;

[TestClass]
public class PcbValidatorTest
{

    [TestMethod]
    public void TestMethod1()
    {

        var errorTypeInvalid = new ErrorType { Code = "123456" };
        var restrictionInvalid = new Device();
        var pcbTypeInvalid = new PcbType();
        // Arrange
        var pcbInvalid = new Pcb();
        pcbInvalid.SerialNumber = "123456789";
        // Act
        var validationResult = new PcbValidator(pcbInvalid);
        // Assert
        Console.WriteLine(string.Join(Environment.NewLine, validationResult.GetErrors()));
        Assert.IsFalse(validationResult.IsValid());
    }
}