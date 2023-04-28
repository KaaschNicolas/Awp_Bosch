using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Models;
public class PaginatedList<T> : List<T>
{
    public int PageIndex
    {
        get; private set;
    }

    public int PageCount
    {
        get; private set;
    }

    private PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageCount = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public static async Task<PaginatedList<T>> CreateAsync(
    Response<List<T>> source,
    int pageIndex,
    int pageSize)
    {
        IQueryable<T> query = source.Data.AsQueryable();
        int count = await query.CountAsync();
        List<T> items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}
