﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Core.Helpers;
using App.Core.Models;
using App.Core.Models.Enums;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Services;
public class StorageLocationDataService<T> : CrudServiceBase<T>, IStorageLocationDataService<T> where T : StorageLocation
{
    public StorageLocationDataService(BoschContext boschContext, ILoggingService loggingService) : base(boschContext, loggingService) { }

    public async Task<Response<List<T>>> GetAllQueryable(int pageIndex, int pageSize)
    {
        try
        {
            var data = await _boschContext.Set<T>()
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

    private async Task<Response<List<T>>> GetStorageLocationFiltered(int pageIndex, int pageSize, Expression<Func<T,bool>> where)
    {
        try
        {
            //Expression<Func<T, bool>> filter = x => x.DwellTimeYellow < 10;
            var data = await _boschContext.Set<T>()
                .Where(where)
                .Skip((pageIndex -1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new Response<List<T>>(ResponseCode.Success, data: data);
        }
        catch (DbUpdateException)
        {
            return new Response<List<T>>(ResponseCode.Error, error: "GetStorageLocationFiltered() failed");
        }
    }

    public async Task<Response<List<T>>> GetWithFilter(int pageIndex, int pageSize, StorageLocationFilterOptions filterOptions, Expression<Func<T, bool>> where) 
        => filterOptions switch
            {
                StorageLocationFilterOptions.DwellTimeYellowLow => await GetStorageLocationFiltered(pageIndex, pageSize, where),
                StorageLocationFilterOptions.DwellTimeYellowHigh => await GetStorageLocationFiltered(pageIndex, pageSize, where),
                StorageLocationFilterOptions.DwellTimeRedHigh => await GetStorageLocationFiltered(pageIndex, pageSize, where),
                StorageLocationFilterOptions.DwellTimeRedLow => await GetStorageLocationFiltered(pageIndex, pageSize, where),
                _ => new Response<List<T>>(ResponseCode.Error, error: "Keine Daten gefunden")
            };

}
