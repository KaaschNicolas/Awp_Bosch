using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Services;
public class StorageLocationDataService<T> : CrudServiceBase<T>, IStorageLocationDataService<T> where T : StorageLocation
{
    public StorageLocationDataService(BoschContext boschContext, ILoggingService loggingService) : base(boschContext, loggingService) { }

    public IQueryable<T> GetAllQueryable()
    {
        return _boschContext.Set<T>().AsQueryable();
    }
}
