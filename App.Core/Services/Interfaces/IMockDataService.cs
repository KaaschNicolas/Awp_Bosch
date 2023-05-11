using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services.Interfaces;
public interface IMockDataService
{
    public void SeedFromExcel(string path);
    public void SeedMockData();
}
