using App.Core.DataAccess;
using App.Core.DTOs;
using App.Core.Models;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class PcbTypeEvaluationService<T> : CrudServiceBase<T> where T : EvaluationStorageLocationDTO
    {
        public PcbTypeEvaluationService(BoschContext context, ILoggingService loggingService) : base(context, loggingService) { }
        /*public async Task<Response<List<EvaluationStorageLocationDTO>>> GetAllLocationsOfPcbType(int pageIndex, int pageSize, string orderByProperty,
                                                                                                    bool isAscending, string pcbType)
        {
            try
            {

                *//*                var query = _boschContext.EvaluationStorageLocationDTO
                                .FromSqlRaw($"SELECT ")
                                //.OrderBy(orderByProperty, isAscending)
                                .Skip((pageIndex == 0 ? pageIndex : pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();*//*
                List<Transfer> lastTransfers = new();
                await _boschContext
               .Pcbs
               .Include(x => x.Transfers)
               .Where(x => x.DeletedDate < x.CreatedDate)
               .ForEachAsync(x => lastTransfers.Add(x.Transfers.Last()));

                List<Pcb> pcbs = new();

                foreach (var transfers in lastTransfers)
                {
                    *//*var res = await _boschContext.Transfers
                    .Include(x => x.Pcb)
                    .ThenInclude(x => x.PcbType)
                    .Where(x => x.Pcb.PcbType.PcbPartNumber == pcbType)
                    .Where(x => x.Id == transfers.PcbId)
                    .ForEachAsync(x => pcbs.Add(x.PcbId));*//*
                }

                int total = pcbs.Count();

                //var data = await query;
                //return new Response<List<EvaluationStorageLocationDTO>>(ResponseCode.Success, pcbs, total);
            }

            catch (DbUpdateException)
            {
                return new Response<List<EvaluationStorageLocationDTO>>(ResponseCode.Error, error: "GetAllQueryable() failed");
            }
        }*/
    }
}
