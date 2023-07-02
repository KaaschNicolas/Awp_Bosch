using App.Core.DTOs;
using App.Core.Models;
using App.Core.Models.Enums;
using System.Linq.Expressions;

namespace App.Core.Services.Interfaces
{
    public interface IPcbDataService<T> : ICrudService<T> where T : Pcb
    {
        public Task<Response<List<PcbDTO>>> GetAllQueryable(int pageIndex, int pageSize, string orderByProperty, bool isAscending);

        public Task<Response<int>> MaxEntries();

        public Task<Response<int>> MaxEntriesFiltered(Expression<Func<T, bool>> where);

        public Task<Response<int>> MaxEntriesByStorageLocation(int storageLocationId);

        public Task<Response<int>> MaxEntriesSearch(string queryText);

        public Task<Response<int>> MaxEntriesPcbTypes(string selectedPcbTypes);

        public Task<Response<List<PcbDTO>>> Like(int pageIndex, int pageSize, string queryText);

        public Task<Response<List<PcbDTO>>> GetWithFilter(int pageIndex, int pageSize, string value, string orderByProperty, bool isAscending, PcbFilterOptions filterOptions);

        public Task<Response<T>> GetByIdEager(int id);

        public Task<Response<T>> Delete(int id);

    }
}
