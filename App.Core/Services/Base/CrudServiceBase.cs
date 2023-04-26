using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace App.Core.Services.Base;
public abstract class CrudServiceBase<T> where T : BaseEntity
{
    protected BoschContext _boschContext;

    protected LoggingService _loggingService;
    public CrudServiceBase(BoschContext boschContext, LoggingService loggingService)
    {
        _boschContext = boschContext;
        _loggingService = loggingService;
    }

    public async Task<Response<T>> Create(T entity)
    {
        try
        {
            EntityEntry entityEntry = await _boschContext.Set<T>().AddAsync(entity);
            await _boschContext.SaveChangesAsync();
            return new Response<T>(ResponseCode.Success, (T)entityEntry.Entity);
        }
        catch (DbUpdateException)
        {
            return new Response<T>(ResponseCode.Error, error: $"Fehler beim Erstellen von {nameof(T)}");
        }
    }

    public async Task<Response<T>> Update(int id, T entity)
    {
        try
        {
            entity.Id = id;

            _boschContext.Set<T>().Update(entity);
            await _boschContext.SaveChangesAsync();

            return new Response<T>(ResponseCode.Success, entity);
        }
        catch (DbUpdateException)
        {
            return new Response<T>(ResponseCode.Error, error: $"Fehler beim Update von {nameof(T)}");
        }
    }

    public async Task<Response<T>> Delete(T entity)
    {
        try
        {
            entity.DeletedDate = DateTime.Now;
            await _boschContext.SaveChangesAsync();
            return new Response<T>(ResponseCode.Success, $"{nameof(entity)} erfolgreich gelöscht.");
        }
        catch (DbUpdateException)
        {
            return new Response<T>(ResponseCode.Error, error: $"Fehler beim Löschen von {nameof(entity)} mit der ID {entity.Id} ");
        }
    }
}
