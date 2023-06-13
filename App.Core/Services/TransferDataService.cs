using App.Core.DataAccess;
using App.Core.DTOs;
using App.Core.Models;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq.Expressions;

namespace App.Core.Services
{
    public class TransferDataService<T> : CrudServiceBase<T>, ITransferDataService<T> where T : Transfer
    {
        public TransferDataService(BoschContext boschContext, ILoggingService loggingService) : base(boschContext, loggingService) { }

        public async Task<Response<List<T>>> GetTransfersByPcb(int pcbId)
        {
            try
            {
                var data = await _boschContext
                    .Set<T>()
                    .Where(x => x.PcbId == pcbId)
                    .Include("Pcb")
                    .Include("StorageLocation")
                    .Include("NotedBy")
                    .ToListAsync();
                return new Response<List<T>>(ResponseCode.Success, data: data);
            }
            catch (DbUpdateException)
            {
                return new Response<List<T>>(ResponseCode.Error, error: "GetTransfersByPcb() failed");
            }
        }


        public async Task<Response<T>> CreateTransfer(T transfer, int? diagnoseId = null)
        {
            using (var transaction = await _boschContext.Database.BeginTransactionAsync())
            {
                try
                {
                    EntityEntry<T> entityEntry = await _boschContext.Set<T>().AddAsync(transfer);
                    var pcb = await _boschContext.Set<Pcb>().FirstOrDefaultAsync(x => x.Id == transfer.PcbId);
                    var storageLocation = await _boschContext.Set<StorageLocation>().FirstOrDefaultAsync(x => x.Id == transfer.StorageLocationId);
                    if (diagnoseId != null)
                    {
                        pcb.DiagnoseId = diagnoseId;
                    }

                    pcb.Finalized = storageLocation.IsFinalDestination;
                    await _boschContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new Response<T>(ResponseCode.Success, (T)entityEntry.Entity);

                }
                catch (Exception ex)
                {
                    return new Response<T>(ResponseCode.Error, error: $"Fehler beim Erstellen von {typeof(T)}");
                }
            }
        }

        public async Task<Response<List<IGrouping<int, T>>>> GetAllGroupedByStorageLocation()
        {
            try
            {
                var data = await _boschContext
                    .Set<T>()
                    .OrderBy(x => x.CreatedDate)
                    .GroupBy(x => x.StorageLocationId)
                    .ToListAsync();
                return new Response<List<IGrouping<int, T>>>(ResponseCode.Success, data: data);
            }
            catch (DbUpdateException)
            {
                return new Response<List<IGrouping<int, T>>>(ResponseCode.Error, error: "GetAllGroupedByStorageLocation() failed");
            }
        }

        public async Task<Response<List<T>>> GetAllEager()
        {
            try
            {
                var list = await _boschContext
                    .Set<T>()
                    .Include(x => x.StorageLocation)
                    .ToListAsync();
                var res = new List<T>();
                foreach (var item in list)
                {
                    if (item.DeletedDate == DateTime.MinValue)
                    {
                        res.Add(item);
                    }
                }

                return new Response<List<T>>(ResponseCode.Success, data: res);
            }
            catch (DbUpdateException)
            {
                return new Response<List<T>>(ResponseCode.Success, error: "GetAllEager() failed");
            }
        }

        public async Task<Response<List<DwellTimeEvaluationDTO>>> GetAvgDwellTimeByStorageLocation(DateTime? from, DateTime? to)
        {
            try
            {
                string queryString = string.Empty;
                if (from != null && to != null)
                {
                    queryString = BuildQuery(from, to);
                }
                else
                {
                    queryString = BuildQuery(null, null);
                }
                var data = await _boschContext
                    .DwellTimeEvaluationDTO
                    .FromSqlRaw(queryString)
                    .ToListAsync();
                return new Response<List<DwellTimeEvaluationDTO>>(ResponseCode.Success, data: data);
            }
            catch (DbUpdateException)
            {
                return new Response<List<DwellTimeEvaluationDTO>>(ResponseCode.Error, error: "GetAvgDwellTimeByStorageLocation() failed");
            }
        }

        private string BuildQuery(DateTime? from, DateTime? to)
        {
            string dateCheck = null;
            if (from != null && to != null)
            {
                var newFrom = (DateTime)from;
                var newTo = (DateTime)to;
                dateCheck = $"AND CreatedDate BETWEEN '{newFrom.ToString("yyyy-MM-dd HH:mm:ss")}' AND '{newTo.ToString("yyyy-MM-dd HH:mm:ss")}'";
            }
            return $@"SELECT b.StorageName,
                    ROUND(AVG(CAST(DwellTime AS FLOAT)), 2) AS AvgDwellTime
                    FROM
                        (SELECT PcbId,
                                t.Id,
                                CreatedDate AS TransferDate,
                                StorageLocationId,
                                s.StorageName,
                                DATEDIFF(DAY, CreatedDate, lag(CreatedDate, 1, GETDATE()) OVER(PARTITION BY PcbId
                                        ORDER BY PcbId DESC, CreatedDate DESC)) AS DwellTime
                        FROM Transfers AS t
                        INNER JOIN
                            (SELECT Id,
                                    StorageName
                            FROM StorageLocations) AS s ON s.Id = t.StorageLocationId
                        WHERE CreatedDate > DeletedDate {dateCheck}) AS b
                    GROUP BY b.StorageName";
        }
    }
}
