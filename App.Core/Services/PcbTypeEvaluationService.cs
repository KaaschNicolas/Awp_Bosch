using App.Core.DataAccess;
using App.Core.DTOs;
using App.Core.Models;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
       
        
        public async Task<Response<List<EvaluationStorageLocationDTO>>> GetAllByPcbType(string pcbType, DateTime deadline )
        {
            try
            {
                /*List<Transfer> lastTransfers = new();
                await _boschContext
               .Pcbs
               .Include(x => x.Transfers)
               .Where(x => x.CreatedDate < DateTime.Now) //TODO: Stichtag einbauen
               .Where(x => x.DeletedDate < x.CreatedDate)
               .ForEachAsync(x => lastTransfers.Add(x.Transfers.Last()));

                List<Pcb> pcbs = new();

                foreach (var transfers in lastTransfers)
                {
                    await _boschContext
                    .Transfers
                    .Include(x => x.Pcb)
                    .ThenInclude(x => x.PcbType)
                    .Where(x => x.Pcb.PcbType.PcbPartNumber == pcbType)
                    .Where(x => x.Id == transfers.PcbId)
                    .ForEachAsync(x => pcbs.Add(x.Pcb));
                }

                int total = pcbs.Count();

                EvaluationStorageLocationDTO pcbDto*/

                //var data = await query;
                //return new Response<List<EvaluationStorageLocationDTO>>(ResponseCode.Success, pcbs, total);


                string queryString = BuildQuery(pcbType, deadline);
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

        private string BuildQuery(string? pcbtype = null, DateTime? deadline = null)
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
    
    }
}
