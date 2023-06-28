using App.Core.DataAccess;
using App.Core.DTOs;
using App.Core.Models;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace App.Core.Services
{
    public class PcbTypeEvaluationService : IPcbTypeEvaluationService 
    {
        protected BoschContext _boschContext;
        protected ILoggingService _loggingService;

        public PcbTypeEvaluationService(BoschContext context, ILoggingService loggingService) 
        {
            _boschContext = context;
            _loggingService = loggingService;
        }


        // Methode zum Abrufen von EvaluationStorageLocationDTO-Objekten basierend auf dem PCB-Typ und dem DeadlineDate
        public async Task<Response<List<EvaluationStorageLocationDTO>>> GetAllByPcbType(string pcbType, DateTime deadline )
        {
            try
            {
                string queryString = BuildQuery1(pcbType, deadline);
                Debug.WriteLine(queryString);
                var query = _boschContext.EvaluationStorageLocationDTO
                .FromSqlRaw(queryString)
                .ToListAsync();

                var data = await query;
                return new Response<List<EvaluationStorageLocationDTO>>(ResponseCode.Success, data: data);
            }

            catch (DbUpdateException)
            {
                return new Response<List<EvaluationStorageLocationDTO>>(ResponseCode.Error, error: "GetAllByPcbType() failed");
            }
        }

        // Methode zum Abrufen von EvaluationFinalizedDTO-Objekten basierend auf dem PCB-Typ und dem DeadlineDate
        public async Task<Response<List<EvaluationFinalizedDTO>>> GetFinalizedByPcbType(string pcbType, DateTime deadline)
        {
            try
            {
                string queryString = BuildQuery2(pcbType, deadline);
                Debug.WriteLine(queryString);
                var query = _boschContext.EvaluationFinalizedDTO
                .FromSqlRaw(queryString)
                .ToListAsync();

                var data = await query;
                return new Response<List<EvaluationFinalizedDTO>>(ResponseCode.Success, data: data);
            }

            catch (DbUpdateException)
            {
                return new Response<List<EvaluationFinalizedDTO>>(ResponseCode.Error, error: "GetAllByPcbType() failed");
            }
        }


        // Methode zum Erstellen der SQL-Abfrage für EvaluationStorageLocationDTO basierend auf dem PCB-Typ und dem DeadlineDate
        private string BuildQuery1(string? pcbtype = null, DateTime? deadline = null)
        {
            if (pcbtype != null && deadline != null)
            {
                var date = ((DateTime)deadline).ToString("yyyy-MM-dd HH:mm:ss");
                string query = $@"SELECT *, f.CountBefore + f.CountAfter AS SumCount
                                    FROM(
                                        SELECT 
                                        g.CurrentStorageLocation AS StorageName,
                                        SUM(g.IsAfterDate) AS CountAfter,
                                        SUM(g.IsBeforeDate) AS CountBefore
                                        FROM(
                                            SELECT b.PcbId, b.IsAfterDate, b.IsBeforeDate, MAX(b.StorageName) AS CurrentStorageLocation
                                            FROM (
                                                    SELECT 
                                                    t.PcbId,
                                                    pt.PcbPartNumber,
                                                    s.StorageName,
                                                    CreatedDate, 
                                                    CASE WHEN CreatedDate >= '{date}' THEN 1 ELSE 0 END AS IsAfterDate, 
                                                    CASE WHEN CreatedDate < '{date}' THEN 1 ELSE 0 END AS IsBeforeDate,
                                                    ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS RnAfter FROM Transfers AS t
                                                    INNER JOIN (SELECT Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate) AS p ON p.Id = PcbId 
                                                    INNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes WHERE PcbPartNumber = '{pcbtype}') AS pt ON pt.Id = p.PcbTypeId
                                                    INNER JOIN (SELECT Id, StorageName FROM StorageLocations) AS s on s.Id = StorageLocationId) AS b
                                            GROUP BY PcbId, IsAfterDate, b.IsBeforeDate) AS g
                                        GROUP BY g.CurrentStorageLocation) AS f";
                return query;
            }
            return null;
        }

        // Methode zum Erstellen der SQL-Abfrage für EvaluationFinalizedDTO basierend auf dem PCB-Typ und dem DeadlineDate
        private string BuildQuery2(string? pcbtype = null, DateTime? deadline = null)
        {
            if (pcbtype != null && deadline != null)
            {
                var date = ((DateTime)deadline).ToString("yyyy-MM-dd HH:mm:ss");
                string query = $@"SELECT *,
                                    t.TotalCount - t.TotalFinalized AS TotalInProgress
                                    FROM (
                                        SELECT
                                        COUNT(Finalized) AS TotalCount,
                                        SUM(CAST(Finalized AS INT)) AS TotalFinalized
                                        FROM Pcbs AS p
                                        INNER JOIN (SELECT 
                                                    Id,
                                                    PcbPartNumber
                                                    FROM PcbTypes
                                                    WHERE PcbPartNumber = '{pcbtype}') AS pt ON p.PcbTypeId = pt.Id
                                       WHERE CreatedDate > DeletedDate ) as t";
                return query;
            }
            return null;
        }

    }
}
