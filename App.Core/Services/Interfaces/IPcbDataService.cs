namespace App.Core.Services.Interfaces
{
    public interface IPcbDataService<T> : ICrudService<T> where T : Pcb
    {
        public Task<Response<List<T>>> GetAllQueryable(int pageIndex, int pageSize, string orderByProperty, bool isAscending);

        public Task<Response<int>> MaxEntries();

        public Task<Response<int>> MaxEntriesFiltered(Expression<Func<T, bool>> where);

        public Task<Response<int>> MaxEntriesByStorageLocation(int storageLocationId);

        public Task<Response<int>> MaxEntriesSearch(string queryText);

        public Task<Response<List<T>>> GetAllSortedBy(int pageIndex, int pageSize, string orderByProperty, bool isAscending);

        public Task<Response<List<T>>> Like(int pageIndex, int pageSize, string queryText);

        public Task<Response<List<T>>> GetWithFilter(int pageIndex, int pageSize, Expression<Func<T, bool>> where);

        public Task<Response<List<T>>> GetWithFilterStorageLocation(int pageIndex, int pageSize, int storageLocationId);

        public Task<Response<T>> GetByIdEager(int id);

        public Task<Response<List<T>>> GetAllEager(int pageIndex, int pageSize, string orderByProperty, bool isAscending);
    }
}
