using App.Core.Contracts.Services;
using App.Core.Models;

namespace App.Core.Services;

// This class holds sample data used by some generated pages to show how they can be used.
// TODO: The following classes have been created to display sample data. Delete these files once your app is using real data.
// 1. Contracts/Services/ISampleDataService.cs
// 2. Services/SampleDataService.cs
// 3. Models/SampleCompany.cs
// 4. Models/PartNumber.cs
// 5. Models/PartNumberDetail.cs
public class SampleDataService : ISampleDataService
{
    private List<PartNumber> _allPartNumbers;

    public SampleDataService()
    {
    }



    private static IEnumerable<PartNumber> AllPartNumbers()
    {
        return new List<PartNumber>()
        {
            new PartNumber()
            {
                Number = "ALFKI",
                Description = "Company A",
                MaxTransfer = 1,

            }
                  
        };
    }

    public async Task<IEnumerable<PartNumber>> GetGridDataAsync()
    {
        if (_allPartNumbers == null)
        {
            _allPartNumbers = new List<PartNumber>(AllPartNumbers());
        }

        await Task.CompletedTask;
        return _allPartNumbers;
    }
}
