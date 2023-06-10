using App.Core.Services.Interfaces;

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

    private PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageCount = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public static async Task<PaginatedList<T>> CreateAsync(
    List<T> source,
    int pageIndex,
    int pageSize,
    int maxEntries
    )
    {
        if (maxEntries == 0)
        {
            // No results -> return page 0.
            return new PaginatedList<T>(new List<T>(), 0, 0, pageSize);
        }



        return new PaginatedList<T>(source, maxEntries, pageIndex, pageSize);
    }
}
