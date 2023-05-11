namespace App.Test;

[TestClass]
public class CreatePcbTest
{
    private readonly IPcbDataService _pcbDataService = new();

    [TestMethod]
    public void TestMethod1()
    {

        var errorTypeInvalid = new ErrorType { Code = "123456" };
        var restrictionInvalid = new Device();
        var pcbTypeInvalid = new PcbType();
        var transfer = new Transfer();
        // Arrange
        var pcbInvalid = new Pcb();
        pcbInvalid.SerialNumber = "123456789";
        // Act
        _pcbDataService.Create(transfer, pcbTypeInvalid, errorTypeInvalid);
        // Assert
        Console.WriteLine(string.Join(Environment.NewLine, validationResult.GetErrors()));
        Assert.IsFalse(validationResult.IsValid());
    }
}