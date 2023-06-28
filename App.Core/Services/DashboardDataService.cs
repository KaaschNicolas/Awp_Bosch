﻿using App.Core.DataAccess;
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
                if (await CanConnect())
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
                throw new DbUpdateException();
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
                if (await CanConnect())
                {
                    var data = await _boschContext
                        .Pcbs
                        .AsNoTracking()
                        .Where(x => x.CreatedDate >= DateTime.Now.AddDays(-7))
                        .CountAsync();
                    return new Response<int>(ResponseCode.Success, data: data);
                }
                throw new DbUpdateException();
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
                if (await CanConnect())
                {
                    var data = await _boschContext
                        .Pcbs
                        .AsNoTracking()
                        .Where(x => x.CreatedDate.Date == DateTime.Now.Date)
                        .CountAsync();
                    return new Response<int>(ResponseCode.Success, data: data);
                }
                throw new DbUpdateException();
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
                if (await CanConnect())
                {
                    var data = await _boschContext
                        .Pcbs
                        .Where(x => x.CreatedDate == DateTime.Now)
                        .Where(x => x.Finalized == true)
                        .CountAsync();
                    return new Response<int>(ResponseCode.Success, data: data);
                }
                throw new DbUpdateException();
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
                if (await CanConnect())
                {
                    var data = await _boschContext
                        .Pcbs
                        .Where(x => x.Finalized == false)
                        .Where(x => x.CreatedDate > x.DeletedDate)
                        .CountAsync();
                    return new Response<int>(ResponseCode.Success, data: data);
                }
                throw new DbUpdateException();
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
                if (await CanConnect()) 
                {
                    var data =  await _boschContext
                        .DashboardStorageLocationDTO
                        .FromSqlRaw(BuildQueryDashboardStorageeLocationDTO())
                        .ToListAsync();
                    return new Response<List<DashboardStorageLocationDTO>>(ResponseCode.Success, data: data);
                }
                throw new DbUpdateException();
            }
            catch (DbUpdateException)
            {
                return new Response<List<DashboardStorageLocationDTO>>(ResponseCode.Error, error: "GetTop3StorageLocations() failed");
            }
        }

        public async Task<Response<List<DashboardDwellTimeDTO>>> GetDwellTimeDTO()
        {
            try
            {
                if (await CanConnect())
                {
                    var data = await _boschContext
                        .DashboardDwellTimeDTO
                        .FromSqlRaw(BuildQueryDwellTime())
                        .ToListAsync();
                    return new Response<List<DashboardDwellTimeDTO>>(ResponseCode.Success, data: data);
                }
                throw new DbUpdateException();
            }
            catch (DbUpdateException)
            {
                return new Response<List<DashboardDwellTimeDTO>>(ResponseCode.Error, error: "GetDwellTimeDTO() failed");
            }
        }

        private static string BuildQueryDashboardStorageeLocationDTO()
        {
            return $@"SELECT TOP(3)
                        c.StorageName,
                        COUNT(c.PcbId) AS CountPcbs,
                        SUM(CASE WHEN c.DwellTimeStatus = 1 THEN 1 ELSE 0 END) AS CountGreen,
                        SUM(CASE WHEN c.DwellTimeStatus = 2 THEN 1 ELSE 0 END) AS CountYellow,
                        SUM(CASE WHEN c.DwellTimeStatus = 3 THEN 1 ELSE 0 END) AS CountRed
                        FROM (

                            SELECT 
                            b.PcbId,
                            b.StorageName,
                            IIF(NOT b.DwellTimeYellow = '--' AND NOT b.DwellTimeYellow = '--', 
                                IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeYellow AS INT) AND CAST(b.DwellTime AS INT) < CAST(b.DwellTimeRed AS INT), 2,
                                IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeRed AS INT), 3,
                                1)),
                            0) AS DwellTimeStatus
                            FROM(
                                SELECT
                                t.PcbId,
                                s.StorageName,
                                p.CreatedDate As FailedAt,
                                s.DwellTimeRed,
                                s.DwellTimeYellow,
                                DATEDIFF(DAY, t.LastTransferDate, GETDATE()) as DwellTime
                                FROM (
                                    SELECT
                                    PcbId,
                                    CreatedDate As LastTransferDate,
                                    StorageLocationId,
                                    ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn
                                    FROM Transfers 
                                    WHERE CreatedDate > DeletedDate) as t
                                INNER JOIN  (SELECT CreatedDate, Finalized, Id FROM Pcbs WHERE CreatedDate > DeletedDate AND Finalized = 0) AS p ON t.PcbId=p.Id
                                INNER JOIN     (SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id

                                WHERE rn=1) as b )
                            as c
                        GROUP BY c.StorageName
                        ORDER BY CountPcbs DESC";
        }

        private static string BuildQueryDwellTime()
        {
            return $@"SELECT 
                        *,
                        COUNT(DwellTimeStatus) AS CountDwellTimeStatus
                        FROM (
                            SELECT 
                            IIF(NOT b.DwellTimeYellow = '--' AND NOT b.DwellTimeYellow = '--', 
                                IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeYellow AS INT) AND CAST(b.DwellTime AS INT) < CAST(b.DwellTimeRed AS INT), 2,
                                IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeRed AS INT), 3,
                                1)),
                            0) AS DwellTimeStatus
                            FROM(
                                SELECT 
                                s.DwellTimeRed,
                                s.DwellTimeYellow,
                                DATEDIFF(DAY, t.LastTransferDate, GETDATE()) as DwellTime
                                FROM (
                                    SELECT
                                    PcbId,
                                    StorageLocationId,
                                    CreatedDate As LastTransferDate,
                                    ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn
                                    FROM Transfers 
                                    WHERE CreatedDate > DeletedDate) as t
                                INNER JOIN  (SELECT  Id FROM Pcbs WHERE CreatedDate > DeletedDate AND Finalized = 0) AS p ON t.PcbId=p.Id
                                INNER JOIN     (SELECT Id, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
                                WHERE rn=1)
                                as b )
                            as c
                        GROUP BY c.DwellTimeStatus";
        }
    }
}
