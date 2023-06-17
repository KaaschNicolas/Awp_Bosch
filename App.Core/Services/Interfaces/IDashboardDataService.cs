using App.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services.Interfaces
{
    public interface IDashboardDataService<T> where T : BaseEntity
    {
        public Task<Response<int>> GetTop3PcbTypes();
    }
}
