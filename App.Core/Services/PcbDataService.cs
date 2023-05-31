
using App.Core.DataAccess;
using App.Core.Helpers;
using App.Core.Models;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

public class PcbDataService<T> : CrudServiceBase<T>, IPcbDataService<T> where T : Pcb
{
    private DateTime deleteCheckDate;

    public PcbDataService(BoschContext boschContext, ILoggingService loggingService) : base(boschContext,
        loggingService)
    {
        deleteCheckDate = new DateTime(2000, 01, 01);
    }

    public async Task<Response<List<T>>> GetAllQueryable(int pageIndex, int pageSize, string orderByProperty,
        bool isAscending)
    {
        try
        {
            var data = await _boschContext.Set<T>()
                .Where(pcb => pcb.DeletedDate < deleteCheckDate)
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
            var data = await _boschContext.Set<T>()
                .Where(pcb => pcb.DeletedDate < deleteCheckDate)
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
            var data = await _boschContext.Set<T>()
                .Where(where)
                .Where(pcb => pcb.DeletedDate < deleteCheckDate)
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

            return new Response<int>(ResponseCode.Success, data: count);
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
            var data = await _boschContext.Set<T>()
                .Where(x => EF.Functions.Like(x.SerialNumber, $"%{queryText}%"))
                .Where(pcb => (pcb.DeletedDate < deleteCheckDate))
                .CountAsync();
            return new Response<int>(ResponseCode.Success, data: data);
        }
        catch (DbUpdateException)
        {
            return new Response<int>(ResponseCode.Error, error: "MaxEntries() failed");
        }
    }

    public async Task<Response<List<T>>> GetAllSortedBy(int pageIndex, int pageSize, string orderByProperty,
        bool isAscending)
    {
        try
        {
            var data = await _boschContext.Set<T>()
                .Where(pcb => pcb.DeletedDate < deleteCheckDate)
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
                data = await _boschContext.Set<T>()
                    .Where(x => EF.Functions.Like(x.SerialNumber, $"%{queryText}%"))
                    .Where(pcb => (pcb.DeletedDate < deleteCheckDate))
                    .Skip((pageIndex) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                data = await _boschContext.Set<T>()
                    .Where(x => EF.Functions.Like(x.SerialNumber, $"%{queryText}%"))
                    .Where(pcb => (pcb.DeletedDate < deleteCheckDate))
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
            var data = await _boschContext.Set<T>()
                .Where(where)
                .Where(pcb => pcb.DeletedDate < deleteCheckDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include("PcbType")
                .ToListAsync();
            return new Response<List<T>>(ResponseCode.Success, data: data);
        }
        catch (DbUpdateException)
        {
            return new Response<List<T>>(ResponseCode.Error, error: "GetStorageLocationFiltered() failed");
        }
    }

    public async Task<Response<List<T>>> GetWithFilterStorageLocation(int pageIndex, int pageSize,
        int storageLocationId)
    {
        try
        {
            List<Transfer> lastTransfers = new();

            await _boschContext.Pcbs.Include(x => x.Transfers).ForEachAsync(x => lastTransfers.Add(x.Transfers.Last()));

            List<T> pcbs = new();

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

            var pcbsPaginated = pcbs
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return new Response<List<T>>(ResponseCode.Success, data: pcbsPaginated.ToList());
        }
        catch (DbUpdateException)
        {
            return new Response<List<T>>(ResponseCode.Error, error: "GetStorageLocationFiltered() failed");
        }
    }

    new public async Task<Response<T>> Delete(T entity)
    {
        try
        {
            _loggingService.Audit(LogLevel.Information, $"{typeof(T)} mit der ID {entity.Id} erfolgreich gelöscht.",
                null);
            //setzt alle DeletedDate der anhängenden Objekte null, die es auch nur für diese Leiterplatte gibt.

            Pcb pcb = _boschContext.Set<T>().Where(x => x.Id.Equals(entity.Id))
                .Include(pcb => pcb.Restriction)
                .Include(etp => etp.ErrorTypes)
                .Include(pcb => pcb.Transfers)
                .First();
            if (!(pcb is null))
            {
                pcb.DeletedDate = DateTime.Now;
                if (!(pcb.Restriction is null))
                {
                    pcb.Restriction.DeletedDate = DateTime.Now;
                }

                if (!(pcb.ErrorTypes is null))
                {
                    pcb.ErrorTypes.ForEach(et => et.DeletedDate = DateTime.Now);
                }

                if (!(pcb.ErrorTypes is null))
                {
                    pcb.Transfers.ForEach(t => t.DeletedDate = DateTime.Now);
                }

                await _boschContext.SaveChangesAsync();
            }

            return new Response<T>(ResponseCode.Success, $"{typeof(T)} erfolgreich gelöscht.");
        }
        catch (DbUpdateException)
        {
            _loggingService.Audit(LogLevel.Error, $"Fehler beim Löschen von {typeof(T)} mit der ID {entity.Id}", null);
            return new Response<T>(ResponseCode.Error,
                error: $"Fehler beim Löschen von {typeof(T)} mit der ID {entity.Id}");
        }
    }

    public async Task<Response<T>> GetByIdEager(int id)
    {
        try
        {
            var entity = await _boschContext.Set<T>()
                .Include(T => T.Transfers)
                .ThenInclude(transfer => transfer.NotedBy)
                .Include(T => T.Transfers)
                .ThenInclude(transfer => transfer.StorageLocation)
                .Include(T => T.Restriction)
                .Include(T => T.Comment)
                .Include(T => T.Diagnose)
                .Include(T => T.PcbType)
                .Include(T => T.ErrorTypes)
                .FirstAsync(x => x.Id == id);
            return new Response<T>(ResponseCode.Success, entity);
        }
        catch (DbUpdateException)
        {
            return new Response<T>(ResponseCode.Error, error: $"Fehler beim abfragen von {typeof(T)} mit der ID {id}");
        }
    }
}
