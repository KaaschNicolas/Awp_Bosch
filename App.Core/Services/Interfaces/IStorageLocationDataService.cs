using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;
using App.Core.Models.Enums;

namespace App.Core.Services.Interfaces;
public interface IStorageLocationDataService<T> where T : StorageLocation
{
    public Task<Response<List<T>>> GetAllQueryable(int pageIndex, int pageSize);

    public Task<Response<int>> MaxEntries();
    public Task<Response<int>> MaxEntriesFiltered(Expression<Func<T, bool>> where);

    public Task<Response<List<T>>> GetAllSortedBy(int pageIndex, int pageSize, string orderByProperty, bool isAscending);

    public Task<Response<List<T>>> GetWithFilter(int pageIndex, int pageSize, StorageLocationFilterOptions filterOptions, Expression<Func<T, bool>> where);
}
