using System.Reflection;
using System.Text.Json;
using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
//using Excel = Microsoft.Office.Interop.Excel;

namespace App.Core.Services;
public class MockDataService : IMockDataService
{
    private BoschContext _boschContext;
    public MockDataService(BoschContext boschContext)
    {
        _boschContext = boschContext;
        //SeedMockData();
        //fill where "Kreisläufer" Excel is located 
        //SeedFromExcel("");
    }

    public async void SeedMockData()
    {
        var prepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Split("App")[0], @"App.Core\Services\mockData");
        //Mocking of ErrorType Data
        var count = await _boschContext.ErrorTypes.CountAsync();
        if (count.Equals(0)) {
            var path = Path.Combine(prepath, "errorTypeMockData.json");
            var file = File.ReadAllText(path);
            var mockData = JsonSerializer.Deserialize<List<ErrorType>>(file);
            _boschContext.AddRange(mockData);
            Console.WriteLine(mockData);

            _boschContext.SaveChanges(); 
        }
        //Mocking of Device Data
        count = await _boschContext.Devices.CountAsync();
        if (count.Equals(0))
        {
            var path = Path.Combine(prepath, "deviceMockData.json");
            var file = File.ReadAllText(path); 
            var mockData = JsonSerializer.Deserialize<List<Device>>(file);
            _boschContext.AddRange(mockData);
            Console.WriteLine(mockData);

            _boschContext.SaveChanges();
        }
        //Mocking of User Data
        count = await _boschContext.Users.CountAsync();
        if (count.Equals(0))
        {
            var path = Path.Combine(prepath, "userMockData.json");
            var file = File.ReadAllText(path); 
            var mockData = JsonSerializer.Deserialize<List<User>>(file);
            _boschContext.AddRange(mockData);
            Console.WriteLine(mockData);

            _boschContext.SaveChanges();
        }
        //Mocking of transfer Data
        count = await _boschContext.Transfers.CountAsync();
        if (count.Equals(0))
        {
            var path = Path.Combine(prepath, "transferMockData.json");
            var file = File.ReadAllText(path);
            var mockData = JsonSerializer.Deserialize<List<Transfer>>(file);
            _boschContext.AddRange(mockData);
            Console.WriteLine(mockData);

            _boschContext.SaveChanges();
        }

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
