using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;
using App.Core.Services.Interfaces;
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
    private readonly ILoggingService _logger;

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize, ILoggingService loggingService)
    {
        _logger = loggingService;
        PageIndex = pageIndex;
        PageCount = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    private PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageCount = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public static async Task<PaginatedList<T>> CreateAsync(
    IQueryable<T> source,
    int pageIndex,
    int pageSize)
    {
        var count = await source.CountAsync();
        if (count == 0)
        {
            // No results -> return page 0.
            return new PaginatedList<T>(new List<T>(), 0, 0, pageSize);
        }

        List<T> items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}
