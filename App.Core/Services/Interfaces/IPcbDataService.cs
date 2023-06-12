using App.Core.DTOs;
using App.Core.Models;
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

        public Task<Response<List<PcbDTO>>> Like(int pageIndex, int pageSize, string queryText);

        public Task<Response<List<PcbDTO>>> GetWithFilter(int pageIndex, int pageSize, string where, string orderByProperty, bool isAscending);

        public Task<Response<List<PcbDTO>>> GetWithFilterStorageLocation(int pageIndex, int pageSize, int storageLocationId, string orderByProperty, bool isAscending);

        public Task<Response<T>> GetByIdEager(int id);

        public Task<Response<T>> Delete(int id);

        public Task<Response<List<PcbDTO>>> GetAllEager(int pageIndex, int pageSize, string orderByProperty, bool isAscending);
    }
}
