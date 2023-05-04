using System.Text.Json;
using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Interfaces;
//using Excel = Microsoft.Office.Interop.Excel;

namespace App.Core.Services;
public class DataService : IDataService
{
    private BoschContext _boschContext;
    public DataService(BoschContext boschContext)
    {
        _boschContext = boschContext;
        SeedMockData();
        //fill where "Kreisläufer" Excel is located 
        //SeedFromExcel("");
    }

    public void SeedMockData()
    {
        var path = "C:\\Users\\Student\\Documents\\AWP\\Awp_Bosch\\App.Core\\Services\\mockData\\";

        //Mocking of Diagnose Data
        var file = File.ReadAllText(path + "diagnoseMockData.json");
        var diagnoseMockData = JsonSerializer.Deserialize<List<Diagnose>>(file);
        _boschContext.AddRange(diagnoseMockData);
        Console.WriteLine(diagnoseMockData);

        //Mocking of Device Data
        file = File.ReadAllText(path + "errorTypeMockData.json");
        var errorTypeMockData = JsonSerializer.Deserialize<List<ErrorType>>(file);
        _boschContext.AddRange(errorTypeMockData);
        Console.WriteLine(errorTypeMockData);

        //Mocking of Device Data
        file = File.ReadAllText(path + "deviceMockData.json");
        var deviceMockData = JsonSerializer.Deserialize<List<Device>>(file);
        _boschContext.AddRange(deviceMockData);
        Console.WriteLine(deviceMockData);

        //Mocking of User Data
        file = File.ReadAllText(path + "userMockData.json");
        var userMockData = JsonSerializer.Deserialize<List<User>>(file);
        _boschContext.AddRange(userMockData);
        Console.WriteLine(userMockData);

        //Mocking of Comment Data
        file = File.ReadAllText(path + "commentMockData.json");
        var commentMockData = JsonSerializer.Deserialize<List<Comment>>(file);
        _boschContext.AddRange(commentMockData);
        Console.WriteLine(commentMockData);

        //Mocking of StorageLocation Data
        file = File.ReadAllText(path + "storageLocationMockData.json");
        var storageLocationMockData = JsonSerializer.Deserialize<List<StorageLocation>>(file);
        _boschContext.AddRange(storageLocationMockData);
        Console.WriteLine(storageLocationMockData);

        //Mocking of transfer Data
        file = File.ReadAllText(path + "transferMockData.json");
        var transferMockData = JsonSerializer.Deserialize<List<Transfer>>(file);
        _boschContext.AddRange(transferMockData);
        Console.WriteLine(transferMockData);

        //Mocking of pcb Data
        file = File.ReadAllText(path + "pcbMockData.json");
        var pcbMockData = JsonSerializer.Deserialize<List<Pcb>>(file);
        _boschContext.AddRange(pcbMockData);
        Console.WriteLine(pcbMockData);
        //_boschContext.SaveChanges();

    }
    public void SeedFromExcel(string path)
    {
        /*
        Excel.Application xlApp = new Excel.Application();
        Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"sandbox_test.xlsx");
        Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
        Excel.Range xlRange = xlWorksheet.UsedRange;
        */
        //_boschContext.SaveChanges();

        // _boschContext.SaveChanges();
    }
}
