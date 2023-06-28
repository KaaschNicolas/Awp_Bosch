using App.Core.DataAccess;
using App.Core.DTOs;
using App.Core.Models;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;

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


        public async Task<Response<List<EvaluationStorageLocationDTO>>> GetAllByPcbType(string pcbType, DateTime deadline)
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

        public async Task<Response<List<Dictionary<string, object>>>> GetPcbTypePosition(List<string> pcbTypeList, DateTime start, DateTime end)
        {
            try
            {
                string queryString = BuildQuery3(pcbTypeList, start, end);
                Debug.WriteLine(queryString);
                var data = _boschContext.GenerateDTO(queryString);

                return new Response<List<Dictionary<string, object>>>(ResponseCode.Success, data: data);
            }

            catch (DbUpdateException)
            {
                return new Response<List<Dictionary<string, object>>>(ResponseCode.Error, error: "GetPcbTypePosition() failed");
            }
        }

        /*public async Task<Response<List<EvaluationPcbTypeI_ODTO>>> GetPcbTypeI_O(string? pcbType, DateTime start, DateTime end)
        {
            try
            {
                string queryString = BuildQuery4(pcbType, start, end);
                Debug.WriteLine(queryString);
                var query = _boschContext.EvaluationPcbTypeI_ODTO
                    .FromSqlRaw(queryString)
                    .ToListAsync();

                var data = await query;
                return new Response<List<EvaluationPcbTypeI_ODTO>>(ResponseCode.Success, data: data);
            }
            catch (DbUpdateException)
            {
                return new Response<List<EvaluationPcbTypeI_ODTO>>(ResponseCode.Error, error: "GetPcbTypeI_O() failed");
            }
        }*/


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

        private string BuildQuery3(List<string>? pcbTypeList = null, DateTime? start = null, DateTime? end = null)
        {
            if (pcbTypeList != null && start != null && end != null)
            {
                var startDate = ((DateTime)start).ToString("yyyy-MM-dd HH:mm:ss");
                var endDate = ((DateTime)end).ToString("yyyy-MM-dd HH:mm:ss");

                StringBuilder list = new();
                // pcbTypeList.ForEach(x => list.Add("[" + x + "]"));
                for (var i = 0; i < pcbTypeList.Count; i++)
                {
                    if (i == pcbTypeList.Count - 1)
                    {
                        list.Append("[" + pcbTypeList[i] + "]");
                    }
                    else
                    {
                        list.Append("[" + pcbTypeList[i] + "],");
                    }
                }

                var listString = list.ToString();

                string query = $@"SELECT
                                *
                                FROM (
	                                SELECT
	                                StorageName  + ' ' + TotalName AS PVBs ,
	                                PcbPartNumber,
	                                TotalValue
	                                FROM (
		                                SELECT
		                                StorageName,
		                                PcbPartNumber,
		                                TotalName,
		                                TotalValue
		                                FROM ( 
			                                SELECT
			                                StorageName,
			                                PcbPartNumber,
			                                TotalIncoming AS Eingang,
			                                TotalCurrent AS Aktuell,
			                                TotalIncoming - TotalCurrent AS Ausgang
			                                FROM(
				                                SELECT
				                                StorageName,
				                                PcbPartNumber,
				                                COUNT(rn) AS TotalIncoming,
				                                SUM(CASE WHEN rn = 1 THEN 1 ELSE 0 END) AS TotalCurrent
				                                FROM(
						                                SELECT
						                                PcbId,
						                                t.CreatedDate AS LastTransferDate,
						                                StorageName,
						                                pt.PcbPartNumber,
						                                ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY t.CreatedDate DESC) AS rn
		
						                                FROM Transfers  AS t
						                                INNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate) AS p ON t.PcbId=p.Id
						                                INNER JOIN 	(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
						                                INNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id -- hier die sachnummern filtern
					                                WHERE t.CreatedDate > t.DeletedDate AND t.CreatedDate > '{startDate}'  AND t.CreatedDate <= '{endDate}') AS c -- hier das start und enddatum dynamisch machen
				                                GROUP BY StorageName, PcbPartNumber ) AS g 
			                                 )
		                                AS s 
		                                unpivot
		                                (
		                                  TotalValue
		                                  for TotalName in (Aktuell, Eingang, Ausgang)
		                                ) AS unpiv 
	                                ) AS j
                                ) AS f
                                PIVOT (
                                  MAX([TotalValue])
                                  FOR [PcbPartNumber]
                                  IN ( -- das muss dynamisch generiert werden je nach auswahl der sachnummern
                                    {list}
                                  )
                                ) AS PivotTables";
                return query;
            }
            return null;
        }

        /*Type != null && start != null && end != null)
            {
                var startDate = ((DateTime)start).ToString("yyyy-MM-dd HH:mm:ss");
                var endDate = ((DateTime)end).ToString("yyyy-MM-dd HH:mm:ss");

                string query = $@"SELECT
                                *
                                FROM (
	                                SELECT
	                                StorageName  + ' ' + TotalName AS PVBMovement ,
	                                PcbPartNumber,
	                                TotalValue
	                                FROM (
		                                SELECT
		                                StorageName,
		                                PcbPartNumber,
		                                TotalName,
		                                TotalValue
		                                FROM ( 
			                                SELECT
			                                StorageName,
			                                PcbPartNumber,
			                                TotalIncoming AS Eingang,
			                                TotalCurrent AS Aktuell,
			                                TotalIncoming - TotalCurrent AS Ausgang
			                                FROM(
				                                SELECT
				                                StorageName,
				                                PcbPartNumber,
				                                COUNT(rn) AS TotalIncoming,
				                                SUM(CASE WHEN rn = 1 THEN 1 ELSE 0 END) AS TotalCurrent
				                                FROM(
						                                SELECT
						                                PcbId,
						                                t.CreatedDate AS LastTransferDate,
						                                StorageName,
						                                pt.PcbPartNumber,
						                                ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY t.CreatedDate DESC) AS rn
		
						                                FROM Transfers  AS t
						                                INNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate) AS p ON t.PcbId=p.Id
						                                INNER JOIN 	(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
						                                INNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id -- hier die sachnummern filtern
					                                WHERE t.CreatedDate > t.DeletedDate AND t.CreatedDate > '{startDate}'  AND t.CreatedDate <= '{endDate}') AS c -- hier das start und enddatum dynamisch machen
				                                GROUP BY StorageName, PcbPartNumber ) AS g 
			                                 )
		                                AS s 
		                                unpivot
		                                (
		                                  TotalValue
		                                  for TotalName in (Aktuell, Eingang, Ausgang)
		                                ) AS unpiv 
	                                ) AS j
                                ) AS f
                                PIVOT (
                                  MAX([TotalValue])
                                  FOR [PcbPartNumber]
                                  IN ( -- das muss dynamisch generiert werden je nach auswahl der sachnummern
                                    '[{pcbType}]'
                                  )
                                ) AS PivotTables";
                return query;
            }
            return null;
        }*/

    }
}
