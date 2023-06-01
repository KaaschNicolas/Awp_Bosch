using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace App.Core.Services.Base;
public abstract class CrudServiceBase<T> where T : BaseEntity
{
    protected BoschContext _boschContext;

    protected ILoggingService _loggingService;
    private DateTime checkDeletedDate = new DateTime(2004, 01, 01);
    public CrudServiceBase(BoschContext boschContext, ILoggingService loggingService)
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
            var res = new List<T>();
            foreach (var item in list)
            {
                if (item.DeletedDate < checkDeletedDate)
                {
                    res.Add(item);
                }
            }

            return new Response<List<T>>(ResponseCode.Success, data: res);
        }
        catch (DbUpdateException)
        {
            _loggingService.Log(LogLevel.Error, "Error GetAll()");
            return new Response<List<T>>(ResponseCode.Error, error: "Error GetAll()");
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
