
using App.Core.DataAccess;
using App.Core.Helpers;
using App.Core.Models;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public class PcbDataService<T> : CrudServiceBase<T>, IPcbDataService<T> where T : Pcb
{
    public PcbDataService(BoschContext boschContext, ILoggingService loggingService) : base(boschContext, loggingService) { }

    public async Task<Response<List<T>>> GetAllQueryable(int pageIndex, int pageSize, string orderByProperty, bool isAscending)
    {
        try
        {
            var data = await _boschContext
                .Set<T>()
                .OrderBy(orderByProperty, isAscending)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new Response<List<T>>(ResponseCode.Success, data: data);
        }
        catch (DbUpdateException)
        {
            return new Response<List<T>>(ResponseCode.Error, error: "GetAllQueryable() failed");
        }
    }

    public async Task<Response<int>> MaxEntries()
    {
        try
        {
            var data = await _boschContext
                .Set<T>()
                .CountAsync();
            return new Response<int>(ResponseCode.Success, data: data);
        }
        catch (DbUpdateException)
        {
            return new Response<int>(ResponseCode.Error, error: "MaxEntries() failed");
        }
    }

    public async Task<Response<int>> MaxEntriesFiltered(Expression<Func<T, bool>> where)
    {
        try
        {
            var data = await _boschContext
                .Set<T>()
                .Where(where)
                .CountAsync();
            return new Response<int>(ResponseCode.Success, data: data);
        }
        catch (DbUpdateException)
        {
            return new Response<int>(ResponseCode.Error, error: "MaxEntries() failed");
        }
    }

    public async Task<Response<int>> MaxEntriesByStorageLocation(int storageLocationId)
    {
        try
        {
            List<Transfer> lastTransfers = new();

            await _boschContext.Pcbs.Include(x => x.Transfers).ForEachAsync(x => lastTransfers.Add(x.Transfers.Last()));

            List<Pcb> pcbs = new();

            //lastTransfers.ForEach(async x =>
            //{
            //    if (x.StorageLocationId == storageLocationId)
            //    {
            //        var res = await GetById(x.PcbId);
            //        if (res.Code == ResponseCode.Success)
            //        {
            //            pcbs.Add(res.Data);
            //        }
            //    }
            //});
            foreach (var transfers in lastTransfers)
            {
                if (transfers.StorageLocationId == storageLocationId)
                {
                    var res = await GetById(transfers.PcbId);
                    if (res.Code == ResponseCode.Success)
                    {
                        pcbs.Add(res.Data);
                    }
                }
            }

            var count = pcbs.Count;
            //var test = from p in _boschContext.Pcbs
            //           join t in _boschContext.Transfers on p.Id equals t.PcbId
            //           group t by p.Id into grp
            //           select grp.OrderByDescending(x => x.Id).First() into grpOldest
            //           where grpOldest.StorageLocationId == storageLocationId 
            //           select new { PcbId = grpOldest.PcbId };
            


            //var dynamicList =  _boschContext
            //    .Pcbs
            //    .FromSqlRaw($"SELECT MAX(T.CreatedDate) AS 'CreatedAt', T.PcbId FROM Pcbs  AS P JOIN Transfers AS T ON P.Id = T.PcbId WHERE T.StorageLocationId = '4'  GROUP BY T.PcbId").AsEnumerable().Count();

            //List<int> list = new();
            //pcbs.ForEach(x => list.Add(x.TransferId));
            //var lastTransfer = _boschContext.Transfers.Include(t => t.Pcb).Where(t => t.StorageLocationId == storageLocationId).Where(t => list.Contains(t.Id) ).Count();

            return new Response<int>(ResponseCode.Success, data: 1);
        }
        catch (DbUpdateException)
        {
            return new Response<int>(ResponseCode.Error, error: "MaxEntries() failed");
        }
    }

    public async Task<Response<int>> MaxEntriesSearch(string queryText)
    {
        try
        {
            var data = await _boschContext
                .Set<T>()
                .Where(x => EF.Functions.Like(x.SerialNumber, $"%{queryText}%"))
                .CountAsync();
            return new Response<int>(ResponseCode.Success, data: data);
        }
        catch (DbUpdateException)
        {
            return new Response<int>(ResponseCode.Error, error: "MaxEntries() failed");
        }
    }

    public async Task<Response<List<T>>> GetAllSortedBy(int pageIndex, int pageSize, string orderByProperty, bool isAscending)
    {
        try
        {
            var data = await _boschContext
                .Set<T>()
                .OrderBy(orderByProperty, isAscending)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            return new Response<List<T>>(ResponseCode.Success, data: data);
        }
        catch (DbUpdateException)
        {
            return new Response<List<T>>(ResponseCode.Error, error: "GetAllSortedBy() failed");
        }
    }

    public async Task<Response<List<T>>> Like(int pageIndex, int pageSize, string queryText)
    {
        try
        {
            List<T> data;

            if (pageIndex == 0)
            {
                data = await _boschContext
                .Set<T>()
                .Where(x => EF.Functions.Like(x.SerialNumber, $"%{queryText}%"))
                .Skip((pageIndex) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            else
            {
                data = await _boschContext
                    .Set<T>()
                    .Where(x => EF.Functions.Like(x.SerialNumber, $"%{queryText}%"))
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            return new Response<List<T>>(ResponseCode.Success, data: data);
        }
        catch (DbUpdateException)
        {
            return new Response<List<T>>(ResponseCode.Error, error: "GetStorageLocationFiltered() failed");
        }
    }

    public async Task<Response<List<T>>> GetWithFilter(int pageIndex, int pageSize, Expression<Func<T, bool>> where)
    {
        try
        {
            var data = await _boschContext
                .Set<T>()
                .Where(where)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new Response<List<T>>(ResponseCode.Success, data: data);
        }
        catch (DbUpdateException)
        {
            return new Response<List<T>>(ResponseCode.Error, error: "GetStorageLocationFiltered() failed");
        }
    }

    public async Task<Response<List<T>>> GetWithFilterStorageLocation(int pageIndex, int pageSize, int storageLocationId)
    {
        try
        {
            var data = await _boschContext
                .Set<T>()
                .Include(e => e.Transfers.LastOrDefault().StorageLocationId == storageLocationId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new Response<List<T>>(ResponseCode.Success, data: data);
        }
        catch (DbUpdateException)
        {
            return new Response<List<T>>(ResponseCode.Error, error: "GetStorageLocationFiltered() failed");
        }
    }
}
