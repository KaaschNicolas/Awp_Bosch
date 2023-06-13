
using App.Core.DataAccess;
using App.Core.DTOs;
using App.Core.Helpers;
using App.Core.Models;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq.Expressions;

public class PcbDataService<T> : CrudServiceBase<T>, IPcbDataService<T> where T : Pcb
{
    public PcbDataService(BoschContext boschContext, ILoggingService loggingService) : base(boschContext,
        loggingService)
    {
    }

    public async Task<Response<List<PcbDTO>>> GetAllQueryable(int pageIndex, int pageSize, string orderByProperty,
        bool isAscending)
    {
        try
        {
            string queryString = buildQuery();
            Debug.WriteLine(queryString);
            var query = _boschContext.PcbsDTO
            .FromSqlRaw(queryString)
            .OrderBy(orderByProperty, isAscending)
            .Skip((pageIndex == 0 ? pageIndex : pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            var data = await query;
            return new Response<List<PcbDTO>>(ResponseCode.Success, data: data);
        }

        catch (DbUpdateException)
        {
            return new Response<List<PcbDTO>>(ResponseCode.Error, error: "GetAllQueryable() failed");
        }
    }

    public async Task<Response<int>> MaxEntries()
    {
        try
        {
            var data = await _boschContext.Set<T>()
                .Where(x => x.DeletedDate < x.CreatedDate)
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
                .Where(x => x.DeletedDate < x.CreatedDate)
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
            var query = _boschContext.PcbsDTO
                .FromSqlRaw(buildQuery(whereFilterOnStorageLocation: storageLocationId.ToString()))
                .CountAsync();
            int count = await query;
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
                .Where(x => x.DeletedDate < x.CreatedDate)
                .Where(x => EF.Functions.Like(x.SerialNumber, $"%{queryText}%"))
                .CountAsync();
            return new Response<int>(ResponseCode.Success, data: data);
        }
        catch (DbUpdateException)
        {
            return new Response<int>(ResponseCode.Error, error: "MaxEntries() failed");
        }
    }


    public async Task<Response<List<PcbDTO>>> Like(int pageIndex, int pageSize, string queryText)
    {
        try
        {
            var query = _boschContext.PcbsDTO
                       .FromSqlRaw(buildQuery(likeFilterOnPcb: queryText))
                       .Skip((pageIndex == 0 ? pageIndex : pageIndex - 1) * pageSize)
                       .Take(pageSize)
                       .ToListAsync();

            var data = await query;
            return new Response<List<PcbDTO>>(ResponseCode.Success, data: data);
        }
        catch (DbUpdateException)
        {
            return new Response<List<PcbDTO>>(ResponseCode.Error, error: "GetStorageLocationFiltered() failed");
        }
    }

    public async Task<Response<List<PcbDTO>>> GetWithFilter(int pageIndex, int pageSize, string value, string orderByProperty, bool isAscending, bool isFilterStoragLocation = false)
    {
        try
        {
            IQueryable<PcbDTO> query;
            if (isFilterStoragLocation)
            {
                query = _boschContext.PcbsDTO
                .FromSqlRaw(buildQuery(whereFilterOnStorageLocation: value));
            }
            else
            {
                query = _boschContext.PcbsDTO
                    .FromSqlRaw(buildQuery(whereFilterOnPcb: value));
            }

            var data = query
                .OrderBy(orderByProperty, isAscending)
                .Skip((pageIndex == 0 ? pageIndex : pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var result = await data;
            return new Response<List<PcbDTO>>(ResponseCode.Success, data: result);
        }
        catch (DbUpdateException)
        {
            return new Response<List<PcbDTO>>(ResponseCode.Error, error: "GetStorageLocationFiltered() failed");
        }
    }

    public async Task<Response<T>> Delete(int id)
    {
        try
        {
            _loggingService.Audit(LogLevel.Information, $"{typeof(T)} mit der ID {id} erfolgreich gelöscht.",
                null);
            //setzt alle DeletedDate der anhängenden Objekte null, die es auch nur für diese Leiterplatte gibt.

            Pcb pcb = _boschContext.Set<T>()
                .Where(x => x.Id.Equals(id))
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
            _loggingService.Audit(LogLevel.Error, $"Fehler beim Löschen von {typeof(T)} mit der ID {id}", null);
            return new Response<T>(ResponseCode.Error,
                error: $"Fehler beim Löschen von {typeof(T)} mit der ID {id}");
        }
    }

    public async Task<Response<T>> GetByIdEager(int id)
    {
        try
        {
            var entity = await _boschContext.Set<T>()
                .Where(x => x.DeletedDate < x.CreatedDate)
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

    private string buildQuery(string? whereFilterOnStorageLocation = null, string? whereFilterOnPcb = null, string? likeFilterOnPcb = null)
    {
        whereFilterOnPcb = whereFilterOnPcb is not null ? $"AND {whereFilterOnPcb}" : whereFilterOnPcb;
        whereFilterOnStorageLocation = whereFilterOnStorageLocation is not null ? $"WHERE Id = {whereFilterOnStorageLocation} " : whereFilterOnStorageLocation;
        likeFilterOnPcb = likeFilterOnPcb is not null ? $"AND SerialNumber LIKE '%{likeFilterOnPcb}%'" : likeFilterOnPcb;

        string query = $@"SELECT 
                    b.PcbId,
                    b.StorageName,
                    b.DwellTime,
                    b.DwellTimeRed,
                    b.DwellTimeYellow,
                    b.FailedAt,
                    b.Finalized as IsFinalized,
                    b.SerialNumber,
                    b.TransferCount,
                    b.PcbPartNumber,
                    b.MainErrorCode,
                    b.SubErrorCode,
                    IIF(NOT b.DwellTimeYellow = '--' AND NOT b.DwellTimeYellow = '--', 
	                    IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeYellow AS INT) AND CAST(b.DwellTime AS INT) < CAST(b.DwellTimeRed AS INT), 2,
	                    IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeRed AS INT), 3,
	                    1)),
                    0) AS DwellTimeStatus
                    FROM(
	                    SELECT t.*,
	                    s.StorageName,
	                    p.SerialNumber,
	                    p.Finalized,
	                    p.CreatedDate As FailedAt,
	                    s.DwellTimeRed,
	                    s.DwellTimeYellow,
	                    pt.PcbPartNumber,
	                    DATEDIFF(DAY, t.LastTransferDate, GETDATE()) as DwellTime,
	                    e.MainErrorCode,
	                    e.SubErrorCode
	                    FROM (
		                    SELECT
		                    PcbId,
		                    CreatedDate As LastTransferDate,
		                    StorageLocationId,
		                    ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn,
		                    COUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount,
		                    ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY PcbId) AS ErrorCount
		                    FROM Transfers 
		                    WHERE CreatedDate > DeletedDate) as t
	                    INNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate {whereFilterOnPcb} {likeFilterOnPcb}) AS p ON t.PcbId=p.Id
                        INNER JOIN(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations {whereFilterOnStorageLocation}) AS s ON t.StorageLocationId = s.Id
                        INNER JOIN(SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id
                        INNER JOIN(SELECT* FROM (
                                        SELECT
                                        PcbId,
                                        FIRST_VALUE(Code) OVER (PARTITION BY PcbId ORDER BY CreatedDate) AS MainErrorCode,
                                        FIRST_VALUE(Code) OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS SubErrorCode,
                                        ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate) AS ErrorRowNumber

                                        FROM ErrorTypes

                                        WHERE CreatedDate > DeletedDate AND PcbId IS NOT NULL) AS err

                                    WHERE err.ErrorRowNumber = 1
				                    ) AS e on p.Id = e.PcbId

                        WHERE rn = 1) as b ";

        return query;
    }


}
