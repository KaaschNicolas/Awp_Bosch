
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
    public PcbDataService(BoschContext boschContext, ILoggingService loggingService) : base(boschContext, loggingService) { }

    public async Task<Response<List<T>>> GetAllQueryable(int pageIndex, int pageSize, string orderByProperty, bool isAscending)
    {
        try
        {
            var data = await _boschContext.Set<T>()
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
                .CountAsync();
            return new Response<int>(ResponseCode.Success, data: data);
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
            var data = await _boschContext.Set<T>()
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
                .Skip((pageIndex) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            else
            {
                data = await _boschContext.Set<T>()
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
            var data = await _boschContext.Set<T>()
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

    new public async Task<Response<T>> Delete(T entity)
    {
        try
        {
            _loggingService.Audit(LogLevel.Information, $"{typeof(T)} mit der ID {entity.Id} erfolgreich gelöscht.", null);
            //setzt alle DeletedDate der anhängenden Objekte null, die es auch nur für diese Leiterplatte gibt.
            
            Pcb pcb = _boschContext.Set<T>().Where(x => x.Id.Equals(entity.Id)).Include(pcb => pcb.Restriction)
               
                      .Include(etp => etp.ErrorTypes)
                      .Include(pcb => pcb.Transfers)
                .First();
            if (!(pcb is null)) {
                pcb.DeletedDate = DateTime.Now;
                if (!(pcb.Restriction is null)) {
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
            return new Response<T>(ResponseCode.Error, error: $"Fehler beim Löschen von {typeof(T)} mit der ID {entity.Id}");
        }
    }
}
