using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

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
            _loggingService.Audit(LogLevel.Information, $"{typeof(T)} hinzugefügt", null);

            EntityEntry<T> entityEntry = await _boschContext.Set<T>().AddAsync(entity);
            await _boschContext.SaveChangesAsync();
            return new Response<T>(ResponseCode.Success, (T)entityEntry.Entity);
        }
        catch (DbUpdateException)
        {
            _loggingService.Audit(LogLevel.Error, $"Fehler beim Erstellen von {typeof(T)}", null);
            return new Response<T>(ResponseCode.Error, error: $"Fehler beim Erstellen von {typeof(T)}");
        }
    }

    public async Task<Response<T>> Update(int id, T entity)
    {
        try
        {
            _loggingService.Audit(LogLevel.Information, $"{typeof(T)} mit der ID {entity.Id} upgedated", null);

            entity.Id = id;

            _boschContext.Set<T>().Update(entity);
            await _boschContext.SaveChangesAsync();

            return new Response<T>(ResponseCode.Success, entity);
        }
        catch (DbUpdateException)
        {
            _loggingService.Log(LogLevel.Error, $"Fehler beim Update von {typeof(T)} mit der ID {entity.Id}");
            return new Response<T>(ResponseCode.Error, error: $"Fehler beim Update von {typeof(T)} mit der ID {entity.Id}");
        }
    }

    public async Task<Response<T>> Delete(T entity)
    {
        try
        {
            _loggingService.Audit(LogLevel.Information, $"{typeof(T)} mit der ID {entity.Id} erfolgreich gelöscht.", null);

            entity.DeletedDate = DateTime.Now;
            await _boschContext.SaveChangesAsync();
            return new Response<T>(ResponseCode.Success, $"{typeof(T)} erfolgreich gelöscht.");
        }
        catch (DbUpdateException)
        {
            _loggingService.Audit(LogLevel.Error, $"Fehler beim Löschen von {typeof(T)} mit der ID {entity.Id}", null);
            return new Response<T>(ResponseCode.Error, error: $"Fehler beim Löschen von {typeof(T)} mit der ID {entity.Id}");
        }
    }

    public async Task<Response<List<T>>> GetAll()
    {
        try
        {
            _loggingService.Log(LogLevel.Debug, $"GetAll()");

            var list = await _boschContext.Set<T>().ToListAsync();

            return new Response<List<T>>(ResponseCode.Success, data: list);
        }
        catch (DbUpdateException)
        {
            _loggingService.Log(LogLevel.Error, "Error GetAll()");
            return new Response<List<T>>(ResponseCode.Success, error: "Error GetAll()");
        }
    }

    public async Task<Response<T>> GetById(int id)
    {
        try
        {
            _loggingService.Log(LogLevel.Debug, $"GetById");
            var entity = await _boschContext.Set<T>().Where(x => x.Id == id).ToListAsync();
            return new Response<T>(ResponseCode.Success, entity.First());
        }
        catch (DbUpdateException)
        {
            _loggingService.Log(LogLevel.Error, "Error GetById()");
            return new Response<T>(ResponseCode.Error, error: $"Fehler beim abfragen von {typeof(T)} mit der ID {id}");
            
        }
    }
}
