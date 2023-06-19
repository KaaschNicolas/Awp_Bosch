using App.Core.DTOs;
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
        public Task<Response<List<DashboardPcbTypeDTO>>> GetTop3PcbTypes();
        public Task<Response<int>> GetPcbCountLast7Days();
        public Task<Response<int>> GetPcbsCreatedToday();
        public Task<Response<int>> GetFinalizedPcbsToday();
        public Task<Response<int>> GetPcbsInCirculation();
        public Task<Response<List<DashboardStorageLocationDTO>>> GetTop3StorageLocations();
        public Task<Response<List<DashboardDwellTimeDTO>>> GetDwellTimeDTO();
    }
}
