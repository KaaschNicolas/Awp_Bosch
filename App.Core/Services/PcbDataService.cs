
using App.Core.DataAccess;
using App.Core.DTOs;
using App.Core.Helpers;
using App.Core.Models;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

            var query = _boschContext.PcbsDTO
            .FromSqlInterpolated($"SELECT \r\nb.PcbId,\r\nb.StorageName,\r\nb.DwellTime,\r\nb.DwellTimeRed,\r\nb.DwellTimeYellow,\r\nb.FailedAt,\r\nb.Finalized as IsFinalized,\r\nb.SerialNumber,\r\nb.TransferCount,\r\nb.PcbPartNumber,\r\nIIF(NOT b.DwellTimeYellow = '--' AND NOT b.DwellTimeYellow = '--', \r\n\tIIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeYellow AS INT) AND CAST(b.DwellTime AS INT) < CAST(b.DwellTimeRed AS INT), 2,\r\n\tIIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeRed AS INT), 3,\r\n\t1)),\r\n0) AS DwellTimeStatus\r\nFROM(\r\n\tSELECT t.*,\r\n\ts.StorageName,\r\n\tp.SerialNumber,\r\n\tp.Finalized,\r\n\tp.CreatedDate As FailedAt,\r\n\ts.DwellTimeRed,\r\n\ts.DwellTimeYellow,\r\n\tpt.PcbPartNumber,\r\n\tDATEDIFF(day, t.LastTransferDate, GETDATE()) as DwellTime \r\n\tFROM (\r\n\t\tSELECT\r\n\t\tPcbId,\r\n\t\tCreatedDate As LastTransferDate,\r\n\t\tStorageLocationId,\r\n\t\tROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn,\r\n\t\tCOUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount\r\n\t\tFROM Transfers \r\n\t\tWHERE CreatedDate > DeletedDate) as t\r\n\tINNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate) AS p ON t.PcbId=p.Id\r\n\tINNER JOIN \t(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id\r\n\tINNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id\r\n\tWHERE rn=1) as b \r\n\r\n")
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
                .FromSqlInterpolated($"SELECT \r\nb.PcbId,\r\nb.StorageName,\r\nb.DwellTime,\r\nb.DwellTimeRed,\r\nb.DwellTimeYellow,\r\nb.FailedAt,\r\nb.Finalized as IsFinalized,\r\nb.SerialNumber,\r\nb.TransferCount,\r\nb.PcbPartNumber,\r\nIIF(NOT b.DwellTimeYellow = '--' AND NOT b.DwellTimeYellow = '--', \r\n\tIIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeYellow AS INT) AND CAST(b.DwellTime AS INT) < CAST(b.DwellTimeRed AS INT), 2,\r\n\tIIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeRed AS INT), 3,\r\n\t1)),\r\n0) AS DwellTimeStatus\r\nFROM(\r\n\tSELECT t.*,\r\n\ts.StorageName,\r\n\tp.SerialNumber,\r\n\tp.Finalized,\r\n\tp.CreatedDate As FailedAt,\r\n\ts.DwellTimeRed,\r\n\ts.DwellTimeYellow,\r\n\tpt.PcbPartNumber,\r\n\tDATEDIFF(day, t.LastTransferDate, GETDATE()) as DwellTime \r\n\tFROM (\r\n\t\tSELECT\r\n\t\tPcbId,\r\n\t\tCreatedDate As LastTransferDate,\r\n\t\tStorageLocationId,\r\n\t\tROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn,\r\n\t\tCOUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount\r\n\t\tFROM Transfers \r\n\t\tWHERE CreatedDate > DeletedDate) as t\r\n\tINNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate) AS p ON t.PcbId=p.Id\r\n\tINNER JOIN \t(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations WHERE Id = {storageLocationId}) AS s ON t.StorageLocationId=s.Id\r\n\tINNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id\r\n\tWHERE rn=1) as b \r\n\r\n")
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
                       .FromSqlInterpolated($"SELECT \r\nb.PcbId,\r\nb.StorageName,\r\nb.DwellTime,\r\nb.DwellTimeRed,\r\nb.DwellTimeYellow,\r\nb.FailedAt,\r\nb.Finalized as IsFinalized,\r\nb.SerialNumber,\r\nb.TransferCount,\r\nb.PcbPartNumber,\r\nIIF(NOT b.DwellTimeYellow = '--' AND NOT b.DwellTimeYellow = '--', \r\n\tIIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeYellow AS INT) AND CAST(b.DwellTime AS INT) < CAST(b.DwellTimeRed AS INT), 2,\r\n\tIIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeRed AS INT), 3,\r\n\t1)),\r\n0) AS DwellTimeStatus\r\nFROM(\r\n\tSELECT t.*,\r\n\ts.StorageName,\r\n\tp.SerialNumber,\r\n\tp.Finalized,\r\n\tp.CreatedDate As FailedAt,\r\n\ts.DwellTimeRed,\r\n\ts.DwellTimeYellow,\r\n\tpt.PcbPartNumber,\r\n\tDATEDIFF(day, t.LastTransferDate, GETDATE()) as DwellTime \r\n\tFROM (\r\n\t\tSELECT\r\n\t\tPcbId,\r\n\t\tCreatedDate As LastTransferDate,\r\n\t\tStorageLocationId,\r\n\t\tROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn,\r\n\t\tCOUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount\r\n\t\tFROM Transfers \r\n\t\tWHERE CreatedDate > DeletedDate) as t\r\n\tINNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate AND SerialNumber LIKE {queryText}) AS p ON t.PcbId=p.Id\r\n\tINNER JOIN \t(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id\r\n\tINNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id\r\n\tWHERE rn=1) as b ")
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
                    .FromSqlInterpolated($"SELECT \r\nb.PcbId,\r\nb.StorageName,\r\nb.DwellTime,\r\nb.DwellTimeRed,\r\nb.DwellTimeYellow,\r\nb.FailedAt,\r\nb.Finalized as IsFinalized,\r\nb.SerialNumber,\r\nb.TransferCount,\r\nb.PcbPartNumber,\r\nIIF(NOT b.DwellTimeYellow = '--' AND NOT b.DwellTimeYellow = '--', \r\n\tIIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeYellow AS INT) AND CAST(b.DwellTime AS INT) < CAST(b.DwellTimeRed AS INT), 2,\r\n\tIIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeRed AS INT), 3,\r\n\t1)),\r\n0) AS DwellTimeStatus\r\nFROM(\r\n\tSELECT t.*,\r\n\ts.StorageName,\r\n\tp.SerialNumber,\r\n\tp.Finalized,\r\n\tp.CreatedDate As FailedAt,\r\n\ts.DwellTimeRed,\r\n\ts.DwellTimeYellow,\r\n\tpt.PcbPartNumber,\r\n\tDATEDIFF(day, t.LastTransferDate, GETDATE()) as DwellTime \r\n\tFROM (\r\n\t\tSELECT\r\n\t\tPcbId,\r\n\t\tCreatedDate As LastTransferDate,\r\n\t\tStorageLocationId,\r\n\t\tROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn,\r\n\t\tCOUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount\r\n\t\tFROM Transfers \r\n\t\tWHERE CreatedDate > DeletedDate) as t\r\n\tINNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate) AS p ON t.PcbId=p.Id\r\n\tINNER JOIN \t(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations WHERE Id = {value}) AS s ON t.StorageLocationId=s.Id\r\n\tINNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id\r\n\tWHERE rn=1) as b \r\n\r\n");
            }
            else
            {
                query = _boschContext.PcbsDTO
                    .FromSqlRaw($"SELECT \r\nb.PcbId,\r\nb.StorageName,\r\nb.DwellTime,\r\nb.DwellTimeRed,\r\nb.DwellTimeYellow,\r\nb.FailedAt,\r\nb.Finalized as IsFinalized,\r\nb.SerialNumber,\r\nb.TransferCount,\r\nb.PcbPartNumber,\r\nIIF(NOT b.DwellTimeYellow = '--' AND NOT b.DwellTimeYellow = '--', \r\n\tIIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeYellow AS INT) AND CAST(b.DwellTime AS INT) < CAST(b.DwellTimeRed AS INT), 2,\r\n\tIIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeRed AS INT), 3,\r\n\t1)),\r\n0) AS DwellTimeStatus\r\nFROM(\r\n\tSELECT t.*,\r\n\ts.StorageName,\r\n\tp.SerialNumber,\r\n\tp.Finalized,\r\n\tp.CreatedDate As FailedAt,\r\n\ts.DwellTimeRed,\r\n\ts.DwellTimeYellow,\r\n\tpt.PcbPartNumber,\r\n\tDATEDIFF(day, t.LastTransferDate, GETDATE()) as DwellTime \r\n\tFROM (\r\n\t\tSELECT\r\n\t\tPcbId,\r\n\t\tCreatedDate As LastTransferDate,\r\n\t\tStorageLocationId,\r\n\t\tROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn,\r\n\t\tCOUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount\r\n\t\tFROM Transfers \r\n\t\tWHERE CreatedDate > DeletedDate) as t\r\n\tINNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate AND {value}) AS p ON t.PcbId=p.Id\r\n\tINNER JOIN \t(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations  ) AS s ON t.StorageLocationId=s.Id\r\n\tINNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id\r\n\tWHERE rn=1) as b \r\n\r\n");

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

}
