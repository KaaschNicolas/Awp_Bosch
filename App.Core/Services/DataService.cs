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
        var path = "C:\\Users\\danie\\Documents\\GitHub\\Awp_Bosch\\App.Core\\Services\\mockData\\";

        //Mocking of ErrorType Data
        var file = File.ReadAllText(path + "errorTypeMockData.json");
        var errorTypeMockData = JsonSerializer.Deserialize<List<ErrorType>>(file);
        _boschContext.AddRange(errorTypeMockData);
        Console.WriteLine(errorTypeMockData);

        _boschContext.SaveChanges();
        //Mocking of Device Data
        file = File.ReadAllText(path + "deviceMockData.json");
        var deviceMockData = JsonSerializer.Deserialize<List<Device>>(file);
        _boschContext.AddRange(deviceMockData);
        Console.WriteLine(deviceMockData);

        _boschContext.SaveChanges();
        //Mocking of User Data
        file = File.ReadAllText(path + "userMockData.json");
        var userMockData = JsonSerializer.Deserialize<List<User>>(file);
        _boschContext.AddRange(userMockData);
        Console.WriteLine(userMockData);

        _boschContext.SaveChanges();

        //Mocking of transfer Data
        file = File.ReadAllText(path + "transferMockData.json");
        var transferMockData = JsonSerializer.Deserialize<List<Transfer>>(file);
        _boschContext.AddRange(transferMockData);
        Console.WriteLine(transferMockData);

        _boschContext.SaveChanges();

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
