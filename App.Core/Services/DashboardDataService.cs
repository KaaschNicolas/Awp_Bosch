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
    public class DashboardDataService<T> : CrudServiceBase<T>, IDashboardDataService<T> where T : BaseEntity
    {
        public DashboardDataService(BoschContext boschContext, ILoggingService loggingService) : base(boschContext, loggingService) {  }

        public async Task<Response<List<DashboardPcbTypeDTO>>> GetTop3PcbTypes()
        {
            try
            {
                var data = await _boschContext
                    .Pcbs
                    .AsNoTracking()
                    .Include(x => x.PcbType)
                    .Where(x => x.CreatedDate >= DateTime.Now.AddDays(-7))
                    .GroupBy(x => x.PcbType.PcbPartNumber)
                    .Select(x => new DashboardPcbTypeDTO() { PcbPartNumber = x.Key, Count = x.Count() })
                    .Take(3)
                    .ToListAsync();
                                  
                return new Response<List<DashboardPcbTypeDTO>>(ResponseCode.Success, data: data);
            }
            catch (DbUpdateException)
            {
                return new Response<List<DashboardPcbTypeDTO>>(ResponseCode.Error, error: "GetTop3PcbType() failed");
            }
        }

        public async Task<Response<int>> GetPcbCountLast7Days()
        {
            try
            {
                var data = await _boschContext
                    .Pcbs
                    .AsNoTracking()
                    .Where(x => x.CreatedDate >= DateTime.Now.AddDays(-7))
                    .CountAsync();
                return new Response<int>(ResponseCode.Success, data: data);
            }
            catch (DbUpdateException)
            {
                return new Response<int>(ResponseCode.Error, error: "GetPcbCountLast7Days() failed");
            }
        }

        public async Task<Response<int>> GetPcbsCreatedToday()
        {
            try
            {
                var data = await _boschContext
                    .Pcbs
                    .AsNoTracking()
                    .Where(x => x.CreatedDate.Date == DateTime.Now.Date)
                    .CountAsync();
                return new Response<int>(ResponseCode.Success, data: data);
            }
            catch (DbUpdateException)
            {
                return new Response<int>(ResponseCode.Error, error: "GetPcbsCreatedToday() failed");
            }
        }

        public async Task<Response<int>> GetFinalizedPcbsToday()
        {
            try
            {
                var data = await _boschContext
                    .Pcbs
                    .Where(x => x.CreatedDate == DateTime.Now)
                    .Where(x => x.Finalized == true)
                    .CountAsync();
                return new Response<int>(ResponseCode.Success, data: data);
            }
            catch (DbUpdateException)
            {
                return new Response<int>(ResponseCode.Error, "GetFinalizedPcbsToday() failed");
            }
        }

        public async Task<Response<int>> GetPcbsInCirculation()
        {
            try
            {
                var data = await _boschContext
                    .Pcbs
                    .Where(x => x.Finalized == false)
                    .CountAsync();
                return new Response<int>(ResponseCode.Success, data: data);
            }
            catch (DbUpdateException)
            {
                return new Response<int>(ResponseCode.Error, error: "GetPcbsInCirculation() failed");
            }
        }

        public async Task<Response<List<DashboardStorageLocationDTO>>> GetTop3StorageLocations()
        {
            try
            {
                var data =  await _boschContext
                    .DashboardStorageLocationDTO
                    .FromSqlRaw(BuildQueryDashboardStorageeLocationDTO())
                    .ToListAsync();
                return new Response<List<DashboardStorageLocationDTO>>(ResponseCode.Success, data: data);
            }
            catch (DbUpdateException)
            {
                return new Response<List<DashboardStorageLocationDTO>>(ResponseCode.Error, error: "GetTop3StorageLocations() failed");
            }
        }

        private static string BuildQueryDashboardStorageeLocationDTO()
        {
            return $@"SELECT TOP(3)  b.StorageName,
                                    COUNT(b.StorageName) AS CountStorageName
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
                           WHERE CreatedDate > DeletedDate) AS b
                        GROUP BY b.StorageName ORDER BY  CountStorageName DESC";
        }
    }
}
